using System;
using Analysis.AST.BExpr;

namespace Analysis.AST.Statement
{
    public class IfStmt : IStatement
    {
        public int Label { get; set; }
        public IBexpr Condition { get; set; }
        public UnscopedBlock Body { get; set; }

        public IfStmt(IBexpr condition, UnscopedBlock body)
        {
            Condition = condition;
            Body = body;
        }

        public override string ToString()
        {
            return $@"if ({Condition.ToString()}) {Body.ToString()}";
        }
        
        public string PrintBlock() => $@"{Label.ToString()}[{Condition.ToString()}]";

    }
}