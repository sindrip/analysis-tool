using System;

namespace Analysis.AST.AExpr
{
    public class AUnaryMinus : IAExpr, IEquatable<AUnaryMinus>
    {
        public IAExpr Left { get; set; }

        public AUnaryMinus(IAExpr left) => Left = left;

        public override string ToString() => $"-({Left}";

        public bool Equals(AUnaryMinus other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Left, other.Left);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((AUnaryMinus) obj);
        }

        public override int GetHashCode()
        {
            return (Left != null ? Left.GetHashCode() : 0);
        }
    }
}