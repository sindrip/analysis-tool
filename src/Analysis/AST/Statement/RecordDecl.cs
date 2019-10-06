using System.Collections.Generic;
using System.Linq;

namespace Analysis.AST.Statement
{
    public class RecordDecl : IStatement
    {
        public int Label { get; set; }
        public string Name { get; set; }
        public IEnumerable<Identifier> Fields { get; set; }
        public int Size { get; set; }

        public RecordDecl(string name, IEnumerable<Identifier> fields)
        {
            Name = name;
            Fields = fields;
            Size = Fields.Count();
        }

        public override string ToString()
        {
            return $"{{{string.Join(";", Fields.Select(x => x.ToString()))}}} {Name};";
        }

        public string PrintBlock() => $"[{this}]";
    }
}
