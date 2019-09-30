namespace Analysis.AST.BExpr
{
    public class BoolLit : IBExpr
    {
        public bool Value { get; set; }

        public BoolLit(bool value) => Value = value;

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}