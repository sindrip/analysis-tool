using System;
using System.Numerics;

namespace Analysis.AST.AExpr
{
    public class IntLit : IAExpr, IEquatable<IntLit>
    {
        public BigInteger Value { get; set; }

        public IntLit(BigInteger value) => Value = value;

        public override string ToString()
        {
            return Value.ToString();
        }

        public bool Equals(IntLit other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((IntLit) obj);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
