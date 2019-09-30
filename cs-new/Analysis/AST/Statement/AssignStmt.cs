using Analysis.AST.AExpr;

namespace Analysis.AST.Statement
{
    public class AssignStmt : IStatement
    {
        public int Label { get; set; }
        public IStateAccess Left { get; set; }
        public IAExpr Right { get; set; }
        
        public AssignStmt(IStateAccess left, IAExpr right)
        {
            Left = left;
            Right = right;
        }

        public override string ToString()
        {
            return $@"{Left.ToString()} = {Right.ToString()};";
        }
        
        public string PrintBlock() => $@"{Label.ToString()}[{this.ToString()}]";

    }
}