namespace Analysis.AST.AExpr
{
    public class ArrayAccess: IAExpr, IStateAccess
    {
        public string Left { get; set; }
        public IAExpr Right { get; set; }
        
        public ArrayAccess(string left, IAExpr right)
        {
            Left = left;
            Right = right;
        }
    }
}