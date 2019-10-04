namespace Analysis.AST.Statement
{
    public class IntDecl : IStatement
    {
        public int Label { get; set; }
        public string Name { get; set; }
        
        public IntDecl(string id) => Name = id;

        public override string ToString()
        {
            return $@"int {this.Name.ToString()};";
        }
        
        public string PrintBlock() => $"[{this}]";
    }
}