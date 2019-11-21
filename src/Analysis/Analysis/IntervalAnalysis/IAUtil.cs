using Analysis.AST.AExpr;

namespace Analysis.Analysis.IntervalAnalysis
{
    public class IAUtil
    {
        public static Interval Arithmetic(IAExpr aExpr, IADomain domain) => aExpr switch
        {
            IntLit intLit => ArithmeticLit(intLit),
            IStateAccess stateAccess => ArithmeticStateAccess(stateAccess, domain),
            ABinOp aBinOp => ArithmeticBinop(aBinOp, domain),
        };
        
        private static Interval ArithmeticLit(IntLit intLit) => 
            new Interval(new ExtendedZ(intLit.Value), new ExtendedZ(intLit.Value));

        private static Interval ArithmeticStateAccess(IStateAccess stateAccess, IADomain domain) => stateAccess switch
        {
            VarAccess varAccess => domain[varAccess.Left],
            ArrayAccess arrayAccess => ArithmeticArrayAccess(arrayAccess, domain),
            RecordAccess recordAccess => domain[recordAccess.Right],
        };

        private static Interval ArithmeticArrayAccess(ArrayAccess arrayAccess, IADomain domain)
        {
            var index = Arithmetic(arrayAccess.Right, domain);
            var arraySize = arrayAccess.Left.Size;
            var arrayIndices = new Interval(new ExtendedZ(0), new ExtendedZ(arraySize - 1));
            if (index.Meet(arrayIndices).IsBottom)
            {
                return Interval.Bottom();
            }

            return domain[arrayAccess.Left];
        }

        private static Interval ArithmeticBinop(ABinOp aBinOp, IADomain domain) => aBinOp.Op switch
        {
            ABinOperator.Plus => Arithmetic(aBinOp.Left, domain) + Arithmetic(aBinOp.Right, domain),
            ABinOperator.Minus => Arithmetic(aBinOp.Left, domain) - Arithmetic(aBinOp.Right, domain),
            ABinOperator.Mult => Arithmetic(aBinOp.Left, domain) * Arithmetic(aBinOp.Right, domain),
            ABinOperator.Div => Arithmetic(aBinOp.Left, domain) / Arithmetic(aBinOp.Right, domain),
        };
    }
}