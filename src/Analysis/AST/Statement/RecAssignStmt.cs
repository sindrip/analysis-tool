using System.Collections.Generic;
using System.Linq;
using Analysis.AST.AExpr;

namespace Analysis.AST.Statement
{
    public class RecAssignStmt : IStatement
    {
        public int Label { get; set; }

        public Identifier Left { get; set; }

        public IEnumerable<IAExpr> Right { get; set; }

        public RecAssignStmt(Identifier left, IEnumerable<IAExpr> right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return $@"{Left} := ({string.Join(",", Right.Select(x => x.ToString()))});";
        }

        public string PrintBlock() => $"[{this}]";
    }
}
