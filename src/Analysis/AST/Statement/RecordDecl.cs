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
        public int Id { get; set; }

        public RecordDecl(string name, IEnumerable<Identifier> fields)
        {
            Name = name;
            Fields = fields;
            Size = Fields.Count();
        }

        public override string ToString()
        {
            var fields = string.Join("; ", Fields.Select(f => $@"({f.Id}, ""{Name}.{f.Name}"""")"));
            return $@"{{{fields}}} ({Id}, ""{Name}"");";
        }

        public string PrintBlock() => $"[{this}]";
    }
}
