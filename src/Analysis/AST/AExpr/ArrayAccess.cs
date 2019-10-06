using System;

namespace Analysis.AST.AExpr
{
    public class ArrayAccess: IAExpr, IStateAccess, IEquatable<ArrayAccess>
    {
        public Identifier Left { get; set; }
        public IAExpr Right { get; set; }
        
        public ArrayAccess(Identifier left, IAExpr right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return $"({Left.Id}, {Left}[{Right}])";
        }

        public bool Equals(ArrayAccess other)
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
            return Equals((ArrayAccess) obj);
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