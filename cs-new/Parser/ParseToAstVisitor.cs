using System;
using System.Collections.Generic;
using System.Linq;
using Analysis.AST;
using Analysis.AST.AExpr;
using Analysis.AST.BExpr;
using Analysis.AST.Statement;
using Antlr4.Runtime;
using Parser.Generated;

namespace Parser
{
    public class ParseToAstVisitor : MicroCBaseVisitor<IAstNode>
    {
        public override IAstNode VisitParse(MicroCParser.ParseContext context)
        {
            ScopedBlock scopedBLock = Visit(context.scopedBlock()) as ScopedBlock;
            return new Program(scopedBLock);
        }

        public override IAstNode VisitScopedBlock(MicroCParser.ScopedBlockContext context)
        {
            IList<IStatement> declarations = context.declaration().Select(x => Visit(x) as IStatement).ToList();
            IList<IStatement> statements = context.statement().Select(x => Visit(x) as IStatement).ToList();
            return new ScopedBlock(declarations.Concat(statements));
        }

        public override IAstNode VisitUnscopedBlock(MicroCParser.UnscopedBlockContext context)
        {
            IList<IStatement> statements = context.statement().Select(x => Visit(x) as IStatement).ToList();
            return new UnscopedBlock(statements);
        }

        public override IAstNode VisitIntDecl(MicroCParser.IntDeclContext context)
        {
            var name = context.IDENT().GetText();
            return new IntDecl(name);
        }

        public override IAstNode VisitArrayDecl(MicroCParser.ArrayDeclContext context)
        {
            var name = context.IDENT().GetText();
            var size = int.Parse(context.NUMBER().GetText());
            return new ArrayDecl(name, size);
        }

        public override IAstNode VisitRecDecl(MicroCParser.RecDeclContext context)
        {
            return base.VisitRecDecl(context);
        }
        public override IAstNode VisitAssignStmt(MicroCParser.AssignStmtContext context)
        {
            VarAccess left = new VarAccess(context.IDENT().GetText());
            IAExpr right = Visit(context.a_expr()) as IAExpr;
            
            return new AssignStmt(left, right);
        }

        public override IAstNode VisitAssignArrayStmt(MicroCParser.AssignArrayStmtContext context)
        {
            string name = context.IDENT().GetText();
            IAExpr index = Visit(context.index) as IAExpr;
            ArrayAccess left = new ArrayAccess(name, index);
            IAExpr right = Visit(context.value) as IAExpr;
            return new AssignStmt(left, right);
        }

        public override IAstNode VisitAssignFieldStmt(MicroCParser.AssignFieldStmtContext context)
        {
            string name = context.name.Text;
            string field = context.field.Text;
            RecordAccess left = new RecordAccess(name, field);
            IAExpr right = Visit(context.a_expr()) as IAExpr;
            return new AssignStmt(left, right);
        }

        public override IAstNode VisitAssignRecStmt(MicroCParser.AssignRecStmtContext context)
        {
            return base.VisitAssignRecStmt(context);
        }

        public override IAstNode VisitIfStmt(MicroCParser.IfStmtContext context)
        {
            IBExpr condition = Visit(context.b_expr()) as IBExpr;
            UnscopedBlock body = Visit(context.unscopedBlock()) as UnscopedBlock;
            return new IfStmt(condition, body);
        }

        public override IAstNode VisitIfElseStmt(MicroCParser.IfElseStmtContext context)
        {
            IBExpr condition = Visit(context.b_expr()) as IBExpr;
            UnscopedBlock ifBody = Visit(context.ifBody) as UnscopedBlock;
            UnscopedBlock elseBody = Visit(context.elseBody) as UnscopedBlock;
            return new IfElseStmt(condition, ifBody, elseBody);
        }

        public override IAstNode VisitWhileStmt(MicroCParser.WhileStmtContext context)
        {
            IBExpr condition = Visit(context.b_expr()) as IBExpr;
            UnscopedBlock body = Visit(context.unscopedBlock()) as UnscopedBlock;
            return new WhileStmt(condition, body);
        }

        public override IAstNode VisitReadStmt(MicroCParser.ReadStmtContext context)
        {
            // TODO all of read possibilites
            string name = context.IDENT().GetText();
            IStateAccess sa = new VarAccess(name);
            return new ReadStmt(sa);
        }

        public override IAstNode VisitWriteStmt(MicroCParser.WriteStmtContext context)
        {
            IAExpr value = Visit(context.a_expr()) as IAExpr;
            return new WriteStmt(value);
        }

        public override IAstNode VisitAexprParen(MicroCParser.AexprParenContext context)
        {
            return Visit(context.a_expr()) as IAExpr;
        }

        public override IAstNode VisitAexprProduct(MicroCParser.AexprProductContext context)
        {
            IAExpr left = Visit(context.left) as IAExpr;
            IAExpr right = Visit(context.right) as IAExpr;
            ABinOperator op = context.op.Text switch
            {
                "*" => ABinOperator.Mult,
                "/" => ABinOperator.Div,
            };
            return new ABinOp(left, right, op);
        }

        public override IAstNode VisitAexprSum(MicroCParser.AexprSumContext context)
        {
            IAExpr left = Visit(context.left) as IAExpr;
            IAExpr right = Visit(context.right) as IAExpr;
            ABinOperator op = context.op.Text switch
            {
                "+" => ABinOperator.Plus,
                "-" => ABinOperator.Minus,
            };
            return new ABinOp(left, right, op);
        }

        public override IAstNode VisitAexprVar(MicroCParser.AexprVarContext context)
        {
            string name = context.IDENT().GetText();
            return new VarAccess(name);
        }

        public override IAstNode VisitAexprArrayAccess(MicroCParser.AexprArrayAccessContext context)
        {
            string name = context.IDENT().GetText();
            IAExpr index = Visit(context.a_expr()) as IAExpr;
            return new ArrayAccess(name, index);
        }

        public override IAstNode VisitAexprRecAccess(MicroCParser.AexprRecAccessContext context)
        {
            string name = context.name.Text;
            string field = context.field.Text;
            return new RecordAccess(name, field);
        }

        public override IAstNode VisitAexprLiteral(MicroCParser.AexprLiteralContext context)
        {
            var value = int.Parse(context.NUMBER().GetText());
            return new IntLit(value);
        }

        public override IAstNode VisitBexprParen(MicroCParser.BexprParenContext context)
        {
            return Visit(context.b_expr()) as IBExpr;
        }

        public override IAstNode VisitBexprNot(MicroCParser.BexprNotContext context)
        {
            IBExpr left = Visit(context.b_expr()) as IBExpr;
            return new BUnOp(left, BUnOperator.Not);
        }

        public override IAstNode VisitBexprBinop(MicroCParser.BexprBinopContext context)
        {
            IBExpr left = Visit(context.left) as IBExpr;
            IBExpr right = Visit(context.right) as IBExpr;
            BBinOperator op = context.bop.Text switch
            {
                "&" => BBinOperator.And,
                "|" => BBinOperator.Or,
            };
            return new BBinOp(left, right, op);
        }

        public override IAstNode VisitRexprBinop(MicroCParser.RexprBinopContext context)
        {
            IAExpr left = Visit(context.left) as IAExpr;
            IAExpr right = Visit(context.right) as IAExpr;
            RBinOperator op = context.rop.Text switch
            {
                "<"  => RBinOperator.LessThan,
                ">"  => RBinOperator.GreaterThan,
                "<=" => RBinOperator.LessThanEq,
                ">=" => RBinOperator.GreaterThanEq,
                "==" => RBinOperator.Eq,
                "!=" => RBinOperator.NotEq,
            };
            return new RBinOp(left, right, op);
        }

        public override IAstNode VisitBexprLit(MicroCParser.BexprLitContext context)
        {
            bool value = bool.Parse(context.value.Text);
            return new BoolLit(value);
        }
    }
}