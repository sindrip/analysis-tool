using System;

namespace Analysis.AST.AExpr
{
    public class RecordAccess : IAExpr, IStateAccess, IEquatable<RecordAccess>
    {
        public Identifier Left { get; set; }
        public Identifier Right { get; set; }
        
        public RecordAccess(Identifier left, Identifier right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return $"({Right.Id}, {Left.Name}.{Right.Name})";
        }

        public bool Equals(RecordAccess other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Left, other.Left) && Equals(Right, other.Right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RecordAccess) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Left != null ? Left.GetHashCode() : 0) * 397) ^ (Right != null ? Right.GetHashCode() : 0);
            }
        }
    }
}