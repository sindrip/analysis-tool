using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                ABinOp abinop => FreeVariables(abinop.Left).Union(FreeVariables(abinop.Right)).ToHashSet(),
                VarAccess va => FreeVariables(va.Ident),
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
    }
}
