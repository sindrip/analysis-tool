using System;

namespace Analysis.AST
{
    public class Identifier : IAstNode, IEquatable<Identifier>
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public int Id { get; }

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

        public bool Equals(Identifier other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Identifier) obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
