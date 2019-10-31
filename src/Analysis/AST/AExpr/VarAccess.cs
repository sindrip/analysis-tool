using System;

namespace Analysis.AST.AExpr
{
    public class VarAccess : IAExpr, IStateAccess, IEquatable<VarAccess>
    {
        public Identifier Left { get; set; }

        public VarAccess(Identifier ident) => Left = ident;

        public override string ToString()
        {
            return Left.ToString();
        }

        public bool Equals(VarAccess other)
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
            return Equals((VarAccess) obj);
        }

        public override int GetHashCode()
        {
            return (Left != null ? Left.GetHashCode() : 0);
        }
    }
}
