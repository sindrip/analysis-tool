namespace Analysis.AST.AExpr
{
    public class RecordAccess : IAExpr, IStateAccess
    {
        public Identifier Left { get; set; }
        public Identifier Right { get; set; }
        
        public RecordAccess(Identifier left, Identifier right)
        {
            Left = left;
            Right = right;
        }
    }
}