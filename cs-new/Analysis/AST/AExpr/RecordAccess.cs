namespace Analysis.AST.AExpr
{
    public class RecordAccess : IAExpr, IStateAccess
    {
        public string Left { get; set; }
        public string Right { get; set; }
        
        public RecordAccess(string left, string right)
        {
            Left = left;
            Right = right;
        }
    }
}