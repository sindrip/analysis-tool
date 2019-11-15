using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using Analysis.AST;
using Analysis.AST.AExpr;
using Analysis.AST.BExpr;
using Analysis.AST.Statement;
using Parser.Generated;

namespace Parser
{
    public class ParseToAstVisitor : MicroCBaseVisitor<IAstNode>
    {
        private SymbolTable _symbolTable = new SymbolTable();
        private int _label = -1;
        // Quick fix to be able to access the identifiers of a record
        private IList<RecordDecl> _recordDecls = new List<RecordDecl>();

        public override IAstNode VisitParse(MicroCParser.ParseContext context)
        {
            return Visit(context.globalBlock());
        }

        public override IAstNode VisitGlobalBlock(MicroCParser.GlobalBlockContext context)
        {
            _symbolTable.AddScope();
            IList<IStatement> declarations = context.declaration().Select(x => Visit(x) as IStatement).ToList();
            IList<IStatement> statements = context.statement().Select(x => Visit(x) as IStatement).ToList();
            _symbolTable.RemoveScope();
            return new Program(declarations.Concat(statements));
        }

        public override IAstNode VisitScopedBlock(MicroCParser.ScopedBlockContext context)
        {
            _symbolTable.AddScope();
            IList<IStatement> declarations = context.declaration().Select(x => Visit(x) as IStatement).ToList();
            IList<IStatement> statements = context.statement().Select(x => Visit(x) as IStatement).ToList();
            _symbolTable.RemoveScope();
            return new ScopedBlock(declarations.Concat(statements));
        }

        public override IAstNode VisitUnscopedBlock(MicroCParser.UnscopedBlockContext context)
        {
            IList<IStatement> statements = context.statement().Select(x => Visit(x) as IStatement).ToList();
            return new UnscopedBlock(statements);
        }

        public override IAstNode VisitIntDecl(MicroCParser.IntDeclContext context)
        {
            var label = ++_label;
            var name = context.IDENT().GetText();
            var id = _symbolTable.InsertSymbol(name, VarType.Int);
            var intDecl = new IntDecl(name);
            intDecl.Label = label;
            intDecl.Id = id;
            return intDecl;
        }

        public override IAstNode VisitArrayDecl(MicroCParser.ArrayDeclContext context)
        {
            var label = ++_label;
            var name = context.IDENT().GetText();
            var size = int.Parse(context.NUMBER().GetText());
            var id = _symbolTable.InsertSymbol(name, VarType.Array);
            var arrayDecl = new ArrayDecl(name, size);
            arrayDecl.Label = label;
            arrayDecl.Id = id;
            return arrayDecl;
        }

        public override IAstNode VisitRecDecl(MicroCParser.RecDeclContext context)
        {
            var label = ++_label;
            _symbolTable.AddScope();
            IList<Identifier> fields = context.fieldDeclaration().Select(x => Visit(x) as Identifier).ToList();
            var children = _symbolTable.RemoveScope();

            string name = context.name.Text;
            var id =_symbolTable.InsertSymbol(name, children);
            var recDecl = new RecordDecl(name, fields);
            recDecl.Label = label;
            recDecl.Id = id;
            _recordDecls.Add(recDecl);
            return recDecl;
        }

        public override IAstNode VisitFieldDeclaration(MicroCParser.FieldDeclarationContext context)
        {
            var name = context.IDENT().GetText();
            var id = _symbolTable.InsertSymbol(name, VarType.Int);
            return new Identifier(name, VarType.Int, id);
        }

        public override IAstNode VisitAssignStmt(MicroCParser.AssignStmtContext context)
        {
            var label = ++_label;
            var name = context.IDENT().GetText();
            var symbol = _symbolTable.LookupSymbol(name);
            var ident = new Identifier(name, symbol.Type, symbol.Id);
            // TODO: Type check the symbol

            VarAccess left = new VarAccess(ident);
            IAExpr right = Visit(context.a_expr()) as IAExpr;

            var assignStmt = new AssignStmt(left, right);
            assignStmt.Label = label;
            return assignStmt;
        }

        public override IAstNode VisitAssignArrayStmt(MicroCParser.AssignArrayStmtContext context)
        {
            var label = ++_label;
            string name = context.IDENT().GetText();
            var symbol = _symbolTable.LookupSymbol(name);
            var ident = new Identifier(name, symbol.Type, symbol.Id);
            // TODO: Type check the symbol

            IAExpr index = Visit(context.index) as IAExpr;
            ArrayAccess left = new ArrayAccess(ident, index);
            IAExpr right = Visit(context.value) as IAExpr;

            var assignStmt = new AssignStmt(left, right);
            assignStmt.Label = label;
            return assignStmt;
        }

        public override IAstNode VisitAssignFieldStmt(MicroCParser.AssignFieldStmtContext context)
        {
            var label = ++_label;
            string recName = context.name.Text;
            string fieldName = context.field.Text;
            var recSymbol = _symbolTable.LookupSymbol(recName);
            if (recSymbol == null)
            {
                throw new ArgumentException($"Record: {recName} does not exist");
            }
            var fieldSymbol = recSymbol.Children.SingleOrDefault(f => f.Name == fieldName);
            if (fieldSymbol == null)
            {
                throw new ArgumentException($"Record: {recName} does not include a field: {fieldName}");
            }

            var recIdent = new Identifier(recName, recSymbol.Type, recSymbol.Id);
            var fieldIdent = new Identifier(fieldName, fieldSymbol.Type, fieldSymbol.Id);

            RecordAccess left = new RecordAccess(recIdent, fieldIdent);
            IAExpr right = Visit(context.a_expr()) as IAExpr;

            var assignStmt = new AssignStmt(left, right);
            assignStmt.Label = label;
            return assignStmt;
        }

        public override IAstNode VisitAssignRecStmt(MicroCParser.AssignRecStmtContext context)
        {
            var label = ++_label;
            string name = context.IDENT().GetText();
            IList<IAExpr> expressions = context.a_expr().Select(x => Visit(x) as IAExpr).ToList();

            var symbol = _symbolTable.LookupSymbol(name);
            if (symbol.Size != expressions.Count)
            {
                throw new ArgumentException(
                    $"Cannot assign {expressions.Count} values to record of size {symbol.Size}");
            }

            var children = _recordDecls.First(r => r.Id == symbol.Id).Fields.ToList();
            var ident = new Identifier(name, symbol.Type, symbol.Id);
            ident.Children = children;

            // TODO: Type check the symbol

            var recAssignStmt = new RecAssignStmt(ident, expressions);
            recAssignStmt.Label = label;
            return recAssignStmt;
        }

        public override IAstNode VisitIfStmt(MicroCParser.IfStmtContext context)
        {
            var label = ++_label;
            IBExpr condition = Visit(context.b_expr()) as IBExpr;
            UnscopedBlock body = Visit(context.unscopedBlock()) as UnscopedBlock;
            var ifStmt = new IfStmt(condition, body);
            ifStmt.Label = label;
            return ifStmt;
        }

        public override IAstNode VisitIfElseStmt(MicroCParser.IfElseStmtContext context)
        {
            var label = ++_label;
            IBExpr condition = Visit(context.b_expr()) as IBExpr;
            UnscopedBlock ifBody = Visit(context.ifBody) as UnscopedBlock;
            UnscopedBlock elseBody = Visit(context.elseBody) as UnscopedBlock;
            var ifElseStmt = new IfElseStmt(condition, ifBody, elseBody);
            ifElseStmt.Label = label;
            return ifElseStmt;
        }

        public override IAstNode VisitWhileStmt(MicroCParser.WhileStmtContext context)
        {
            var label = ++_label;
            IBExpr condition = Visit(context.b_expr()) as IBExpr;
            UnscopedBlock body = Visit(context.unscopedBlock()) as UnscopedBlock;
            var whileStmt = new WhileStmt(condition, body);
            whileStmt.Label = label;
            return whileStmt;
        }

        public override IAstNode VisitReadStmt(MicroCParser.ReadStmtContext context)
        {
            var label = ++_label;
            // TODO all of read possibilites
            string name = context.IDENT().GetText();
            var symbol = _symbolTable.LookupSymbol(name);
            var ident = new Identifier(symbol.Name, symbol.Type, symbol.Id);
            IStateAccess sa = new VarAccess(ident);
            var readStmt = new ReadStmt(sa);
            readStmt.Label = label;
            return readStmt;
        }

        public override IAstNode VisitWriteStmt(MicroCParser.WriteStmtContext context)
        {
            var label = ++_label;
            IAExpr value = Visit(context.a_expr()) as IAExpr;
            var writeStmt = new WriteStmt(value);
            writeStmt.Label = label;
            return writeStmt;
        }

        public override IAstNode VisitAexprParen(MicroCParser.AexprParenContext context)
        {
            return Visit(context.a_expr()) as IAExpr;
        }

        public override IAstNode VisitAexprUnaryMinus(MicroCParser.AexprUnaryMinusContext context)
        {
            IAExpr left = Visit(context.left) as IAExpr;
            if (left is IntLit)
            {
                var intLit = left as IntLit;
                return new IntLit(-intLit.Value);
            }
            return new AUnaryMinus(left);
        }

        public override IAstNode VisitAexprProduct(MicroCParser.AexprProductContext context)
        {
            IAExpr left = Visit(context.left) as IAExpr;
            IAExpr right = Visit(context.right) as IAExpr;
            ABinOperator op = context.op.Text switch
            {
                "*" => ABinOperator.Mult,
                "/" => ABinOperator.Div,
                _ => throw new ArgumentException("Invalid ABinOp Product Operator")
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
                _ => throw new ArgumentException("Invalid ABinOp Sum Operator")
            };
            return new ABinOp(left, right, op);
        }

        public override IAstNode VisitAexprVar(MicroCParser.AexprVarContext context)
        {
            string name = context.IDENT().GetText();
            var symbol = _symbolTable.LookupSymbol(name);
            var ident = new Identifier(symbol.Name, symbol.Type, symbol.Id);
            return new VarAccess(ident);
        }

        public override IAstNode VisitAexprArrayAccess(MicroCParser.AexprArrayAccessContext context)
        {
            string name = context.IDENT().GetText();
            var symbol = _symbolTable.LookupSymbol(name);
            var ident = new Identifier(symbol.Name, symbol.Type, symbol.Id);
            IAExpr index = Visit(context.a_expr()) as IAExpr;
            return new ArrayAccess(ident, index);
        }

        public override IAstNode VisitAexprRecAccess(MicroCParser.AexprRecAccessContext context)
        {
            string recName = context.name.Text;
            string fieldName = context.field.Text;
            var recSymbol = _symbolTable.LookupSymbol(recName);
            var fieldSymbol = recSymbol.Children.SingleOrDefault(f => f.Name == fieldName);
            if (fieldSymbol == null)
            {
                throw new ArgumentException($"Record: {recName} does not include a field: {fieldName}");
            }

            var recIdent = new Identifier(recName, recSymbol.Type, recSymbol.Id);
            var fieldIdent = new Identifier(fieldName, fieldSymbol.Type, fieldSymbol.Id);

            return new RecordAccess(recIdent, fieldIdent);
        }

        public override IAstNode VisitAexprLiteral(MicroCParser.AexprLiteralContext context)
        {
            var value = BigInteger.Parse(context.NUMBER().GetText());
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
                _ => throw new ArgumentException("Invalid BBinOpOperator")
            };
            return new BBinOp(left, right, op);
        }

        public override IAstNode VisitRexprBinop(MicroCParser.RexprBinopContext context)
        {
            IAExpr left = Visit(context.left) as IAExpr;
            IAExpr right = Visit(context.right) as IAExpr;
            RBinOperator op = context.rop.Text switch
            {
                "<" => RBinOperator.LessThan,
                ">" => RBinOperator.GreaterThan,
                "<=" => RBinOperator.LessThanEq,
                ">=" => RBinOperator.GreaterThanEq,
                "==" => RBinOperator.Eq,
                "!=" => RBinOperator.NotEq,
                _ => throw new ArgumentException("Invalid RBinOpOperator")
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
