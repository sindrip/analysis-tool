using Analysis.AST.BExpr;

namespace Analysis.AST.Statement
{
    public class WhileStmt : IStatement
    {
        public int Label { get; set; }
        public IBexpr Condition { get; set; }
        public UnscopedBlock Body { get; set; }
        
        public WhileStmt(IBexpr condition, UnscopedBlock body)
        {
            Condition = condition;
            Body = body;
        }
        
        public override string ToString()
        {
            return $@"while ({Condition}) {Body}";
        }
        
        public string PrintBlock() => $@"{Label.ToString()}[{Condition}]";

    }
}