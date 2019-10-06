using System.Collections.Generic;
using System.Linq;
using Analysis.AST.Statement;

namespace Analysis.AST
{
    public class UnscopedBlock : IAstNode
    {
        public IEnumerable<IStatement> Statements;

        public UnscopedBlock(IEnumerable<IStatement> statements) => this.Statements = statements;

        public override string ToString()
        {
            var statements = string.Join("\n", Statements.Select(s => s.ToString()));
            return $@"{{ {statements} }}";
        }
    }
}
