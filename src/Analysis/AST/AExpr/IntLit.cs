namespace Analysis.AST.AExpr
{
    public class IntLit : IAExpr
    {
        public int Value { get; set; }

        public IntLit(int value) => Value = value;

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}