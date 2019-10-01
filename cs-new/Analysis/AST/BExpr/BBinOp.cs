namespace Analysis.AST.BExpr
{
    public class BBinOp : IBExpr
    {
        public IBExpr Left { get; set; }
        public IBExpr Right { get; set; }
        public BBinOperator Op { get; set; }
        

        public BBinOp(IBExpr left, IBExpr right, BBinOperator op)
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