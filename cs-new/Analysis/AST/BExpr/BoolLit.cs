namespace Analysis.AST.BExpr
{
    public class BoolLit : IBexpr
    {
        public bool Value { get; set; }

        public BoolLit(bool value) => Value = value;

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}