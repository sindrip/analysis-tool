using System;

namespace Analysis.AST
{

    public class Identifier
    {
        public string Name { get; set; }
        public int Id { get; set; }

        public Identifier(string name) => Name = name;
    }
}