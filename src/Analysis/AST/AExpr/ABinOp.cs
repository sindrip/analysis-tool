using System;

namespace Analysis.AST.AExpr
{
    public class ABinOp : IAExpr, IEquatable<ABinOp>
    {
        public IAExpr Left { get; set; }
        public IAExpr Right { get; set; }
        public ABinOperator Op { get; set; }
        
        public ABinOp(IAExpr left, IAExpr right, ABinOperator op)
        {
            Left = left;
            Right = right;
            Op = op;
        }

        public override string ToString()
        {
            return $"({Left} {Op} {Right})";
        }

        public bool Equals(ABinOp other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Left, other.Left) && Equals(Right, other.Right) && Op == other.Op;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ABinOp) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Left != null ? Left.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Right != null ? Right.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) Op;
                return hashCode;
            }
        }
    }
}