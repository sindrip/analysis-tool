using System.ComponentModel.Design;

namespace Analysis.AST
{
    // Encapsulates the top level statement (Metadata could be added here).
    public class Program : IAstNode
    {
        public ScopedBlock TopLevelStmt { get; set; }

        public Program(ScopedBlock topLevelStmt) => TopLevelStmt = topLevelStmt;
    }
}