using Analysis.AST.BExpr;

namespace Analysis.AST.Statement
{
    public class IfElseStmt : IStatement
    {
        public int Label { get; set; }
        public IBexpr Condition { get; set; }
        public UnscopedBlock IfBody { get; set; }
        public UnscopedBlock ElseBody { get; set; }

        public IfElseStmt(IBexpr condition, UnscopedBlock ifBody, UnscopedBlock elseBody)
        {
            Condition = condition;
            IfBody = ifBody;
            ElseBody = elseBody;
        }

        public override string ToString()
        {
            return $@"if ({Condition.ToString()}) {IfBody.ToString()} else {ElseBody.ToString()}";
        }
        
        public string PrintBlock() => $@"{Label.ToString()}[{Condition.ToString()}]";

    }
}