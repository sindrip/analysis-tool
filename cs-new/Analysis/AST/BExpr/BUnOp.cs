namespace Analysis.AST.BExpr
{
    public class BUnOp : IBExpr
    {
        public IBExpr Left { get; set; }
        public BUnOperator Op { get; set; }

        public BUnOp(IBExpr left, BUnOperator op)
        {
            Left = left;
            Op = op;
        }
    }
}