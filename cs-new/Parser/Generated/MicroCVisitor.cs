//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     ANTLR Version: 4.7.2
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from /home/sindrip/school/programanalysis/analysis-tool/cs-new/Parser/MicroC.g4 by ANTLR 4.7.2

// Unreachable code detected
#pragma warning disable 0162
// The variable '...' is assigned but its value is never used
#pragma warning disable 0219
// Missing XML comment for publicly visible type or member '...'
#pragma warning disable 1591
// Ambiguous reference in cref attribute
#pragma warning disable 419

namespace Parser.Generated {
using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;
using IToken = Antlr4.Runtime.IToken;

/// <summary>
/// This interface defines a complete generic visitor for a parse tree produced
/// by <see cref="MicroCParser"/>.
/// </summary>
/// <typeparam name="Result">The return type of the visit operation.</typeparam>
[System.CodeDom.Compiler.GeneratedCode("ANTLR", "4.7.2")]
[System.CLSCompliant(false)]
public interface IMicroCVisitor<Result> : IParseTreeVisitor<Result> {
	/// <summary>
	/// Visit a parse tree produced by <see cref="MicroCParser.parse"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitParse([NotNull] MicroCParser.ParseContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MicroCParser.scopedBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitScopedBlock([NotNull] MicroCParser.ScopedBlockContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MicroCParser.unscopedBlock"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitUnscopedBlock([NotNull] MicroCParser.UnscopedBlockContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>intDecl</c>
	/// labeled alternative in <see cref="MicroCParser.declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIntDecl([NotNull] MicroCParser.IntDeclContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>arrayDecl</c>
	/// labeled alternative in <see cref="MicroCParser.declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitArrayDecl([NotNull] MicroCParser.ArrayDeclContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>recDecl</c>
	/// labeled alternative in <see cref="MicroCParser.declaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRecDecl([NotNull] MicroCParser.RecDeclContext context);
	/// <summary>
	/// Visit a parse tree produced by <see cref="MicroCParser.fieldDeclaration"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitFieldDeclaration([NotNull] MicroCParser.FieldDeclarationContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>assignStmt</c>
	/// labeled alternative in <see cref="MicroCParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignStmt([NotNull] MicroCParser.AssignStmtContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>assignArrayStmt</c>
	/// labeled alternative in <see cref="MicroCParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignArrayStmt([NotNull] MicroCParser.AssignArrayStmtContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>assignFieldStmt</c>
	/// labeled alternative in <see cref="MicroCParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignFieldStmt([NotNull] MicroCParser.AssignFieldStmtContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>assignRecStmt</c>
	/// labeled alternative in <see cref="MicroCParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAssignRecStmt([NotNull] MicroCParser.AssignRecStmtContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ifStmt</c>
	/// labeled alternative in <see cref="MicroCParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIfStmt([NotNull] MicroCParser.IfStmtContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>ifElseStmt</c>
	/// labeled alternative in <see cref="MicroCParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitIfElseStmt([NotNull] MicroCParser.IfElseStmtContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>whileStmt</c>
	/// labeled alternative in <see cref="MicroCParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWhileStmt([NotNull] MicroCParser.WhileStmtContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>readStmt</c>
	/// labeled alternative in <see cref="MicroCParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitReadStmt([NotNull] MicroCParser.ReadStmtContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>writeStmt</c>
	/// labeled alternative in <see cref="MicroCParser.statement"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitWriteStmt([NotNull] MicroCParser.WriteStmtContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>aexprRecAccess</c>
	/// labeled alternative in <see cref="MicroCParser.a_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAexprRecAccess([NotNull] MicroCParser.AexprRecAccessContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>aexprSum</c>
	/// labeled alternative in <see cref="MicroCParser.a_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAexprSum([NotNull] MicroCParser.AexprSumContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>aexprParen</c>
	/// labeled alternative in <see cref="MicroCParser.a_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAexprParen([NotNull] MicroCParser.AexprParenContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>aexprVar</c>
	/// labeled alternative in <see cref="MicroCParser.a_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAexprVar([NotNull] MicroCParser.AexprVarContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>aexprArrayAccess</c>
	/// labeled alternative in <see cref="MicroCParser.a_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAexprArrayAccess([NotNull] MicroCParser.AexprArrayAccessContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>aexprProduct</c>
	/// labeled alternative in <see cref="MicroCParser.a_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAexprProduct([NotNull] MicroCParser.AexprProductContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>aexprLiteral</c>
	/// labeled alternative in <see cref="MicroCParser.a_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitAexprLiteral([NotNull] MicroCParser.AexprLiteralContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>bexprNot</c>
	/// labeled alternative in <see cref="MicroCParser.b_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBexprNot([NotNull] MicroCParser.BexprNotContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>bexprParen</c>
	/// labeled alternative in <see cref="MicroCParser.b_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBexprParen([NotNull] MicroCParser.BexprParenContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>rexprBinop</c>
	/// labeled alternative in <see cref="MicroCParser.b_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitRexprBinop([NotNull] MicroCParser.RexprBinopContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>bexprLit</c>
	/// labeled alternative in <see cref="MicroCParser.b_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBexprLit([NotNull] MicroCParser.BexprLitContext context);
	/// <summary>
	/// Visit a parse tree produced by the <c>bexprBinop</c>
	/// labeled alternative in <see cref="MicroCParser.b_expr"/>.
	/// </summary>
	/// <param name="context">The parse tree.</param>
	/// <return>The visitor result.</return>
	Result VisitBexprBinop([NotNull] MicroCParser.BexprBinopContext context);
}
} // namespace Parser.Generated
