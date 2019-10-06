using System;

namespace Analysis.AST.AExpr
{
    public class VarAccess : IAExpr, IStateAccess, IEquatable<VarAccess>
    {
        public Identifier Ident;

        public VarAccess(Identifier ident) => Ident = ident;

        public override string ToString()
        {
            return Ident.ToString();
        }

        public bool Equals(VarAccess other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Ident, other.Ident);
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
            return (Ident != null ? Ident.GetHashCode() : 0);
        }
    }
}