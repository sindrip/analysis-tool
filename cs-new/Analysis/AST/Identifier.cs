using System;

namespace Analysis.AST
{

    public class Identifier : IAstNode
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public Identifier(string name) => Name = name;

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}