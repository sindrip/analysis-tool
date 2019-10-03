namespace Analysis.AST.AExpr
{
    public class VarAccess : IAExpr, IStateAccess
    {
        public string Name;

        public VarAccess(string name) => Name = name;

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}