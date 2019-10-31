namespace Analysis.AST.AExpr
{
    public interface IStateAccess : IAstNode
    {
        Identifier Left { get; set; }
    }
}
