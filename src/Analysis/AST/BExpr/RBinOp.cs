using Analysis.AST.AExpr;

namespace Analysis.AST.BExpr
{
    public class RBinOp : IBExpr
    {
        public IAExpr Left { get; set; }
        public IAExpr Right { get; set; }
        public RBinOperator Op { get; set; }

        public RBinOp(IAExpr left, IAExpr right, RBinOperator op)
        {
            Left = left;
            Right = right;
            Op = op;
        }

        public override string ToString()
        {
            return $"({Left} {Op} {Right})";
        }
    }
}
