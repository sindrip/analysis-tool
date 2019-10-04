using Analysis.AST.BExpr;

namespace Analysis.AST.Statement
{
    public class WhileStmt : IStatement
    {
        public int Label { get; set; }
        public IBExpr Condition { get; set; }
        public UnscopedBlock Body { get; set; }
        
        public WhileStmt(IBExpr condition, UnscopedBlock body)
        {
            Condition = condition;
            Body = body;
        }
        
        public override string ToString()
        {
            return $@"while ({Condition}) {Body}";
        }
        
        public string PrintBlock() => $"[{Condition}]";

    }
}