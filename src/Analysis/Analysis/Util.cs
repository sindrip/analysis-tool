using System;
using System.Collections.Generic;
using System.Linq;
using Analysis.AST;
using Analysis.AST.AExpr;
using Analysis.AST.BExpr;
using Analysis.AST.Statement;

namespace Analysis.Analysis
{
    public static class Util
    {
        public static HashSet<Identifier> FreeVariables(IAstNode node) => node switch
        {
                Program p => p.TopLevelStmts.SelectMany(stmt => FreeVariables(stmt)).ToHashSet(),
                ScopedBlock _ => Enumerable.Empty<Identifier>().ToHashSet(),
                UnscopedBlock ub => ub.Statements.SelectMany(stmt => FreeVariables(stmt)).ToHashSet(),
                //IntDecl id =>
                //ArrayDecl ad => 
                //RecordDecl rc =>
                IfStmt ifStmt => FreeVariables(ifStmt.Condition).Union(FreeVariables(ifStmt.Body)).ToHashSet(),
                IfElseStmt ifElseStmt => FreeVariables(ifElseStmt.Condition).Union(FreeVariables(ifElseStmt.IfBody))
                    .Union(FreeVariables(ifElseStmt.ElseBody)).ToHashSet(),
                WhileStmt whileStmt => FreeVariables(whileStmt.Condition).Union(FreeVariables(whileStmt.Body)).ToHashSet(),
                AssignStmt assignStmt => FreeVariables(assignStmt.Left).Union(FreeVariables(assignStmt.Right)).ToHashSet(),
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
                Identifier ident => new HashSet<Identifier>() {ident},
                _ => Enumerable.Empty<Identifier>().ToHashSet(),
        };
    }
}