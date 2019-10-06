using Analysis.AST.AExpr;

namespace Analysis.AST.Statement
{
    public class ReadStmt : IStatement
    {
        public int Label { get; set; }

        public IStateAccess Left;

        public ReadStmt(IStateAccess left) => Left = left;

        public override string ToString()
        {
            return $@"read {Left.ToString()};";
        }

        public string PrintBlock() => $"[{this}]";
    }
}
