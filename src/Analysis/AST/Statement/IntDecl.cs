namespace Analysis.AST.Statement
{
    public class IntDecl : IStatement
    {
        public int Label { get; set; }
        public string Name { get; set; }
        public int Id { get; set; }

        public IntDecl(string id) => Name = id;

        public override string ToString()
        {
            return $@"int ({Id}, ""{Name}"");";
        }

        public string PrintBlock() => $"[{this}]";
    }
}
