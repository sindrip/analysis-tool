namespace Analysis.AST.Statement
{
    public interface IStatement : IAstNode
    {
        int Label { get; set; }
        string PrintBlock();
    }
}
