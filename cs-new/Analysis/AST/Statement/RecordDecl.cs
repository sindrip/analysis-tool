namespace Analysis.AST.Statement
{
    public class RecordDecl : IStatement
    {
        public int Label { get; set; }

        public string PrintBlock() => $@"{Label.ToString()}[{this.ToString()}]";

        // TODO: Fill this out
    }
}