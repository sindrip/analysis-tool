namespace Analysis.AST.AExpr
{
    public class ABinOp : IAExpr
    {
        public IAExpr Left { get; set; }
        public IAExpr Right { get; set; }
        public ABinOperator Op { get; set; }
        
        public ABinOp(IAExpr left, IAExpr right, ABinOperator op)
        {
            Left = left;
            Right = right;
            Op = op;
        }
    }
}