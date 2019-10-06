using System.Collections.Generic;
using System.Linq;
using Analysis.AST.Statement;

namespace Analysis.AST
{
    // Encapsulates the top level statements as global scope (Metadata could be added here).
    public class Program : IAstNode
    {
        public IEnumerable<IStatement> TopLevelStmts { get; set; }

        public Program(IEnumerable<IStatement> topLevelStmts) => TopLevelStmts = topLevelStmts;

        public override string ToString()
        {
            var statements = string.Join("\n", TopLevelStmts.Select(s => s.ToString()));
            return $@"{{ {statements} }}";
        }
    }
}
