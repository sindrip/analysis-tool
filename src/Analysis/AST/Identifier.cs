using System;
using System.Collections.Generic;

namespace Analysis.AST
{
    public class Identifier : IAstNode, IEquatable<Identifier>
    {
        public string Name { get; set; }
        public VarType Type { get; set; }
        public int Id { get; }
        public int Size { get; }
        public List<Identifier> Children { get; set; }

        public Identifier(string name, VarType type, int id)
        {
            Name = name;
            Type = type;
            Id = id;
        }
        
        public Identifier(string name, VarType type, int id, int size)
        {
            Name = name;
            Type = type;
            Id = id;
            Size = size;
        }

        public override string ToString()
        {
            return $@"({Id}, ""{Name}"")";
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
