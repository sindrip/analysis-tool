namespace Analysis.AST.AExpr
{
    public class VarAccess : IAExpr, IStateAccess
    {
        public Identifier Ident;

        public VarAccess(Identifier ident) => Ident = ident;

        public override string ToString()
        {
            return Ident.ToString();
        }
    }
}