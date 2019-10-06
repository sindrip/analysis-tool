namespace Analysis.AST.Statement
{
    public class ArrayDecl : IStatement
    {
        public int Label { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }

        public ArrayDecl(string name, int size)
        {
            Name = name;
            Size = size;
        }

        public override string ToString()
        {
            return $@"int[{Size.ToString()}] {Name};";
        }

        public string PrintBlock() => $"[{this}]";
    }
}
