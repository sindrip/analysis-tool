namespace Analysis.AST.AExpr
{
    public class VarAccess : IAExpr, IStateAccess
    {
        public string Name;

        public VarAccess(string id) => Name = id;

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}