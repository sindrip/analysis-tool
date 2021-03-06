using System;
using System.Collections.Generic;
using System.Linq;
using Analysis.AST;
using Analysis.AST.Statement;

namespace Analysis.CFG
{
    public static class FlowUtil
    {
        // Done during the parsing phase now so redundant code, keeping for now
        //public static void LabelProgram(IEnumerable<IStatement> blocks)
        //{
        //    var labelCounter = 0;
        //    foreach (var statement in blocks)
        //    {
        //        statement.Label = ++labelCounter;
        //    }
        //}

        public static int Init(IAstNode node)
        {
            return node switch
            {
                Program program => Init(program.TopLevelStmts.First()),
                ScopedBlock scopedBlock => Init(scopedBlock.Statements.First()),
                UnscopedBlock unscopedBlock => Init(unscopedBlock.Statements.First()),
                IStatement statement => statement.Label,
                _ => throw new ArgumentException("Init can only accept Meta Nodes and IStatement Nodes")
            };
        }

        public static IEnumerable<int> Final(IAstNode node)
        {
            return node switch
            {
                Program program => Final(program.TopLevelStmts.Last()),
                ScopedBlock scopedBlock => Final(scopedBlock.Statements.Last()),
                UnscopedBlock unscopedBlock => Final(unscopedBlock.Statements.Last()),
                IfStmt ifStmt => Final(ifStmt.Body).Union(ifStmt.Label.Singleton()),
                IfElseStmt ifElseStmt => Final(ifElseStmt.IfBody).Union(Final(ifElseStmt.ElseBody)),
                IStatement statement => new HashSet<int> {statement.Label},
                _ => throw new ArgumentException("Final can only accept Meta Nodes and IStatement Nodes")
            };
        }

        public static IEnumerable<IStatement> Blocks(IAstNode node)
        {
            return node switch
            {
                Program program => program.TopLevelStmts.SelectMany(Blocks),
                ScopedBlock scopedBlock => scopedBlock.Statements.SelectMany(Blocks),
                UnscopedBlock unscopedBlock => unscopedBlock.Statements.SelectMany(Blocks),
                IfStmt ifStmt => (new[] {ifStmt}).Union(Blocks(ifStmt.Body)),
                WhileStmt whileStmt => (new[] {whileStmt}).Union(Blocks(whileStmt.Body)),
                IfElseStmt ifElseStmt => (new[] {ifElseStmt}).Union(Blocks(ifElseStmt.IfBody))
                .Union(Blocks(ifElseStmt.ElseBody)),
                IStatement statement => new List<IStatement>() {statement},
                _ => throw new ArgumentException(
                    $"Blocks can only accept Meta Nodes and IStatement Nodes but argument is of type: {node.GetType()}")
            };
        }

        public static IEnumerable<int> Labels(IEnumerable<IStatement> blocks) => blocks.Select(x => x.Label);

        public static IEnumerable<FlowEdge> Flow(IAstNode node)
        {
            switch (node)
            {
                case Program program:
                {
                    var s1 = program.TopLevelStmts.First();
                    if (program.TopLevelStmts.Count() == 1)
                    {
                        return Flow(s1);
                    }

                    var s2 = new Program(program.TopLevelStmts.Skip(1));
                    var initS2 = Init(s2);
                    var finalS1 = Final(s1);
                    var f1 = Flow(s1);
                    var f2 = Flow(s2);
                    var newEdges = finalS1.Select(l => new FlowEdge(l, initS2));
                    return f1.Union(f2).Union(newEdges);
                }
                case ScopedBlock scopedBlock:
                {
                    var s1 = scopedBlock.Statements.First();
                    if (scopedBlock.Statements.Count() == 1)
                    {
                        return Flow(s1);
                    }

                    var s2 = new ScopedBlock(scopedBlock.Statements.Skip(1));
                    var initS2 = Init(s2);
                    var finalS1 = Final(s1);
                    var f1 = Flow(s1);
                    var f2 = Flow(s2);
                    var newEdges = finalS1.Select(l => new FlowEdge(l, initS2));
                    return f1.Union(f2).Union(newEdges);
                }
                case UnscopedBlock unscopedBlock:
                {
                    var s1 = unscopedBlock.Statements.First();
                    if (unscopedBlock.Statements.Count() == 1)
                    {
                        return Flow(s1);
                    }

                    var s2 = new UnscopedBlock(unscopedBlock.Statements.Skip(1));
                    var initS2 = Init(s2);
                    var finalS1 = Final(s1);
                    var f1 = Flow(s1);
                    var f2 = Flow(s2);
                    var newEdges = finalS1.Select(l => new FlowEdge(l, initS2));
                    return f1.Union(f2).Union(newEdges);
                }
                case IfStmt ifStmt:
                {
                    var f1 = Flow(ifStmt.Body);
                    var initS0 = Init(ifStmt.Body);
                    var newEdge = new List<FlowEdge> {new FlowEdge(ifStmt.Label, initS0)};
                    return f1.Union(newEdge);
                }
                case IfElseStmt ifElseStmt:
                {
                    var f1 = Flow(ifElseStmt.IfBody);
                    var f2 = Flow(ifElseStmt.ElseBody);
                    var initS1 = Init(ifElseStmt.IfBody);
                    var initS2 = Init(ifElseStmt.ElseBody);
                    var l = ifElseStmt.Label;
                    var newEdges = new List<FlowEdge> {new FlowEdge(l, initS1), new FlowEdge(l, initS2)};
                    return f1.Union(f2).Union(newEdges);
                }
                case WhileStmt whileStmt:
                {
                    var f0 = Flow(whileStmt.Body);
                    var initS0 = Init(whileStmt.Body);
                    // {(l,init(S0)}
                    var newEdge = new List<FlowEdge> {new FlowEdge(whileStmt.Label, initS0)};
                    var finalS0 = Final(whileStmt.Body);
                    var newEdges = finalS0.Select(lp => new FlowEdge(lp, whileStmt.Label));
                    return f0.Union(newEdge).Union(newEdges);
                }
                case IStatement statement:
                    return new List<FlowEdge>();
                default:
                    throw new ArgumentException("f");
            }
        }

        public static HashSet<FlowEdge> FlowR(IEnumerable<FlowEdge> flowSet) =>
            flowSet.Select(tuple => new FlowEdge(tuple.Dest, tuple.Source)).ToHashSet();
    }
}
