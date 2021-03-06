using System.Collections.Generic;
using System.Linq;
using Analysis.AST.Statement;

namespace Analysis.AST
{
    // A block that introduces a variable scope
    // Holds a list of statements
    public class ScopedBlock : IAstNode
    {
        public IEnumerable<IStatement> Statements;

        public ScopedBlock(IEnumerable<IStatement> statements) => this.Statements = statements;

        public override string ToString()
        {
            var statements = string.Join("\n", Statements.Select(s => s.ToString()));
            return $@"{{ {statements} }}";
        }
    }
}
