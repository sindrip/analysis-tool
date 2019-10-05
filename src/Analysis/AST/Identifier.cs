using System;

namespace Analysis.AST
{

    public class Identifier : IAstNode
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Id { get; set; }

        public Identifier(string name, string type, int id)
        {
            Name = name;
            Type = type;
            Id = id;
        }

        public override string ToString()
        {
            return $"({Id}, {Name})";
        }
    }
}