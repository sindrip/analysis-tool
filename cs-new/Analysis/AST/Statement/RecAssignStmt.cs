using System.Collections.Generic;
using Analysis.AST.AExpr;

namespace Analysis.AST.Statement
{
    public class RecAssignStmt : IStatement
    {
        public int Label { get; set; }
        
        public string Left { get; set; }
        
        public IEnumerable<IAExpr> Right { get; set; }

        public RecAssignStmt(string left, IEnumerable<IAExpr> right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return $@"{Left.ToString()} := {Right.ToString()};";
        }

        public string PrintBlock() => $"{Label.ToString()}{this}";
    }
}