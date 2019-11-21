using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Analysis.AST;
using Analysis.AST.AExpr;
using Analysis.AST.BExpr;
using Analysis.AST.Statement;

namespace Analysis.Analysis
{
    public static class AnalysisUtil
    {
        // It would probably be wise to split this up, for now with a single global scope, it is fine as is
        public static HashSet<Identifier> FreeVariables(IAstNode node) =>
            node switch
            {
                // Sequence Statements
                Program p => p.TopLevelStmts.SelectMany(stmt => FreeVariables(stmt)).ToHashSet(),
                ScopedBlock _ => Enumerable.Empty<Identifier>().ToHashSet(),
                UnscopedBlock ub => ub.Statements.SelectMany(stmt => FreeVariables(stmt)).ToHashSet(),
                // Declarations
                //IntDecl id =>
                //ArrayDecl ad => 
                //RecordDecl rc =>
                // Statements
                IfStmt ifStmt => FreeVariables(ifStmt.Condition).Union(FreeVariables(ifStmt.Body)).ToHashSet(),
                IfElseStmt ifElseStmt => FreeVariables(ifElseStmt.Condition)
                    .Union(FreeVariables(ifElseStmt.IfBody))
                    .Union(FreeVariables(ifElseStmt.ElseBody))
                    .ToHashSet(),
                WhileStmt whileStmt => FreeVariables(whileStmt.Condition)
                    .Union(FreeVariables(whileStmt.Body))
                    .ToHashSet(),
                AssignStmt assignStmt => FreeVariables(assignStmt.Left)
                    .Union(FreeVariables(assignStmt.Right))
                    .ToHashSet(),
                RecAssignStmt recAssignStmt => recAssignStmt.Right.SelectMany(stmt => FreeVariables(stmt)).ToHashSet(),
                ReadStmt readStmt => FreeVariables(readStmt.Left),
                WriteStmt writeStmt => FreeVariables(writeStmt.Left),
                // Boolean nodes
                BUnOp bunop => FreeVariables(bunop.Left),
                BBinOp bbinop => FreeVariables(bbinop.Left).Union(FreeVariables(bbinop.Right)).ToHashSet(),
                RBinOp rbinop => FreeVariables(rbinop.Left).Union(FreeVariables(rbinop.Right)).ToHashSet(),
                // Arithmetic nodes
                AUnaryMinus aUnaryMinus => FreeVariables(aUnaryMinus.Left).ToHashSet(),
                ABinOp abinop => FreeVariables(abinop.Left).Union(FreeVariables(abinop.Right)).ToHashSet(),
                VarAccess va => FreeVariables(va.Left),
                RecordAccess ra => FreeVariables(ra.Right),
                ArrayAccess aa => FreeVariables(aa.Left).Union(FreeVariables(aa.Right)).ToHashSet(),
                Identifier ident => ident.Singleton().ToHashSet(),
                _ => Enumerable.Empty<Identifier>().ToHashSet(),
            };

        public static HashSet<IExpression> AvailableExpressions(IAstNode node) =>
            node switch
            {
                // Sequence Statements
                Program p => p.TopLevelStmts.SelectMany(stmt => AvailableExpressions(stmt)).ToHashSet(),
                ScopedBlock _ => Enumerable.Empty<IExpression>().ToHashSet(),
                UnscopedBlock ub => ub.Statements.SelectMany(stmt => AvailableExpressions(stmt)).ToHashSet(),
                // Declarations
                //IntDecl id =>
                //ArrayDecl ad => 
                //RecordDecl rc =>
                // Statements
                IfStmt ifStmt => AvailableExpressions(ifStmt.Condition).Union(AvailableExpressions(ifStmt.Body))
                    .ToHashSet(),
                IfElseStmt ifElseStmt => AvailableExpressions(ifElseStmt.Condition)
                    .Union(AvailableExpressions(ifElseStmt.IfBody))
                    .Union(AvailableExpressions(ifElseStmt.ElseBody))
                    .ToHashSet(),
                WhileStmt whileStmt => AvailableExpressions(whileStmt.Condition)
                    .Union(AvailableExpressions(whileStmt.Body))
                    .ToHashSet(),
                AssignStmt assignStmt => AvailableExpressions(assignStmt.Left)
                    .Union(AvailableExpressions(assignStmt.Right))
                    .ToHashSet(),
                RecAssignStmt recAssignStmt => recAssignStmt.Right.SelectMany(stmt => AvailableExpressions(stmt))
                    .ToHashSet(),
                ReadStmt readStmt => AvailableExpressions(readStmt.Left),
                WriteStmt writeStmt => AvailableExpressions(writeStmt.Left),
                // Expressions
                ArrayAccess aa => AvailableExpressions(aa.Left).Union(AvailableExpressions(aa.Right)).ToHashSet(),
                AUnaryMinus aUnaryMinus => AvailableExpressions(aUnaryMinus.Left)
                    .Union(aUnaryMinus.Singleton())
                    .ToHashSet(),
                ABinOp abinop => AvailableExpressions(abinop.Left)
                    .Union(AvailableExpressions(abinop.Right))
                    .Union(abinop.Singleton())
                    .ToHashSet(),
                BBinOp bbinop => AvailableExpressions(bbinop.Left)
                    .Union(AvailableExpressions(bbinop.Right))
                    .Union(bbinop.Singleton())
                    .ToHashSet(),
                RBinOp rbinop => AvailableExpressions(rbinop.Left)
                    .Union(AvailableExpressions(rbinop.Right))
                    .Union(rbinop.Singleton())
                    .ToHashSet(),
                _ => Enumerable.Empty<IExpression>().ToHashSet(),
            };

        public static HashSet<BigInteger> InterestingValues(IAstNode node) =>
            IV(node).SelectMany(x => new HashSet<BigInteger> {x, x - 1, x + 1}).ToHashSet();
        
        private static HashSet<BigInteger> IV(IAstNode node) =>
            node switch
            {
                // Sequence Statements
                Program p => p.TopLevelStmts.SelectMany(stmt => IV(stmt)).ToHashSet(),
                ScopedBlock _ => new HashSet<BigInteger>(),
                UnscopedBlock ub => ub.Statements.SelectMany(stmt => IV(stmt)).ToHashSet(),
                // Declarations
                IntDecl id => (new BigInteger(0)).Singleton().ToHashSet(),
                ArrayDecl ad => new HashSet<BigInteger> {0, ad.Size},
                RecordDecl rc => (new BigInteger(0)).Singleton().ToHashSet(),
                // Statements
                IfStmt ifStmt => IV(ifStmt.Condition).Union(IV(ifStmt.Body))
                    .ToHashSet(),
                IfElseStmt ifElseStmt => IV(ifElseStmt.Condition)
                    .Union(IV(ifElseStmt.IfBody))
                    .Union(IV(ifElseStmt.ElseBody))
                    .ToHashSet(),
                WhileStmt whileStmt => IV(whileStmt.Condition)
                    .Union(IV(whileStmt.Body))
                    .ToHashSet(),
                AssignStmt assignStmt => IV(assignStmt.Left)
                    .Union(IV(assignStmt.Right))
                    .ToHashSet(),
                RecAssignStmt recAssignStmt => recAssignStmt.Right.SelectMany(stmt => IV(stmt))
                    .ToHashSet(),
                ReadStmt readStmt => IV(readStmt.Left),
                WriteStmt writeStmt => IV(writeStmt.Left),
                // Expressions
                ArrayAccess aa => IV(aa.Left).Union(IV(aa.Right)).ToHashSet(),
                AUnaryMinus aUnaryMinus => IV(aUnaryMinus.Left)
                    .ToHashSet(),
                ABinOp abinop => IV(abinop.Left)
                    .Union(IV(abinop.Right))
                    .ToHashSet(),
                BBinOp bbinop => IV(bbinop.Left)
                    .Union(IV(bbinop.Right))
                    .ToHashSet(),
                RBinOp rbinop => IV(rbinop.Left)
                    .Union(IV(rbinop.Right))
                    .ToHashSet(),
                IntLit intlit => intlit.Value.Singleton().ToHashSet(),
                _ => Enumerable.Empty<BigInteger>().ToHashSet(),
            };
    }
}
