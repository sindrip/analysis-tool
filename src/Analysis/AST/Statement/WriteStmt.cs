using Analysis.AST.AExpr;
using Analysis.AST.BExpr;

namespace Analysis.AST.Statement
{
    public class WriteStmt : IStatement
    {
        public int Label { get; set; }
        public IAExpr Left;

        public WriteStmt(IAExpr left) => Left = left;

        public override string ToString()
        {
            return $@"write {Left.ToString()};";
        }
        
        public string PrintBlock() => $"[{this}]";
    }
}