using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Analysis.AST;
using Analysis.AST.AExpr;
using Analysis.AST.Statement;

namespace Analysis.Analysis.DetectionOfSigns
{
    public static class DSUtil
    {
        public static HashSet<DSSign> Arithmetic(IAExpr aExpr, DSDomain domain) => aExpr switch
        {
            IntLit intLit => ArithmeticLit(intLit),
            IStateAccess stateAccess => ArithmeticStateAccess(stateAccess, domain),
            ABinOp aBinOp => ArithmeticBinop(aBinOp, domain),
            _ => new HashSet<DSSign>(),
        };
        
        private static HashSet<DSSign> ArithmeticStateAccess(IStateAccess stateAccess, DSDomain domain) => stateAccess switch
        {
            VarAccess varAccess => domain[varAccess.Left],
            ArrayAccess arrayAccess => domain[arrayAccess.Left],
            RecordAccess recordAccess => domain[recordAccess.Right],
        };

        private static HashSet<DSSign> ArithmeticLit(IntLit intLit)
        {
            if (intLit.Value > 0)
            {
                return DSSign.Positive.Singleton().ToHashSet();
            }
            else if (intLit.Value == 0)
            {
                return DSSign.Zero.Singleton().ToHashSet();
            }

            return DSSign.Negative.Singleton().ToHashSet();
        }
        
        private static HashSet<DSSign> ArithmeticBinop(ABinOp aBinOp, DSDomain domain) => aBinOp.Op switch
        {
            ABinOperator.Plus => ABinopPlus(aBinOp, domain),
            ABinOperator.Minus => ABinopMinus(aBinOp, domain),
            ABinOperator.Mult => ABinopMult(aBinOp, domain),
            ABinOperator.Div => ABinopDiv(aBinOp, domain),
        };
        
        private static HashSet<DSSign> ABinopPlus(ABinOp aBinOp, DSDomain domain)
        {
            var left = Arithmetic(aBinOp.Left, domain);
            var right = Arithmetic(aBinOp.Right, domain);
            var signs = new HashSet<DSSign>();
            foreach (var lSign in left)
            {
                foreach (var rSign in right)
                {
                    signs.UnionWith(PlusDSSigns(lSign, rSign));
                }
            }
            return signs;
        }
        
        private static HashSet<DSSign> ABinopMinus(ABinOp aBinOp, DSDomain domain)
        {
            var left = Arithmetic(aBinOp.Left, domain);
            var right = Arithmetic(aBinOp.Right, domain);
            var signs = new HashSet<DSSign>();
            foreach (var lSign in left)
            {
                foreach (var rSign in right)
                {
                    signs.UnionWith(MinusDSSigns(lSign, rSign));
                }
            }
            return signs;
        }
        
        private static HashSet<DSSign> ABinopMult(ABinOp aBinOp, DSDomain domain)
        {
            var left = Arithmetic(aBinOp.Left, domain);
            var right = Arithmetic(aBinOp.Right, domain);
            var signs = new HashSet<DSSign>();
            foreach (var lSign in left)
            {
                foreach (var rSign in right)
                {
                    signs.UnionWith(MultiplyDSSigns(lSign, rSign));
                }
            }
            return signs;
        }
        
        private static HashSet<DSSign> ABinopDiv(ABinOp aBinOp, DSDomain domain)
        {
            var left = Arithmetic(aBinOp.Left, domain);
            var right = Arithmetic(aBinOp.Right, domain);
            var signs = new HashSet<DSSign>();
            foreach (var lSign in left)
            {
                foreach (var rSign in right)
                {
                    signs.UnionWith(DivideDSSigns(lSign, rSign));
                }
            }
            return signs;
        }
        

        private static HashSet<DSSign> PlusDSSigns(DSSign left, DSSign right) => (left, right) switch
        {
            (DSSign.Negative, DSSign.Negative) => DSSign.Negative.Singleton().ToHashSet(),
            (DSSign.Negative, DSSign.Zero) => DSSign.Negative.Singleton().ToHashSet(),
            (DSSign.Negative, DSSign.Positive) => new HashSet<DSSign>() { DSSign.Negative, DSSign.Positive, DSSign.Zero},
            (DSSign.Zero, DSSign.Negative) => DSSign.Negative.Singleton().ToHashSet(),
            (DSSign.Zero, DSSign.Zero) => DSSign.Zero.Singleton().ToHashSet(),
            (DSSign.Zero, DSSign.Positive) => DSSign.Positive.Singleton().ToHashSet(),
            (DSSign.Positive, DSSign.Negative) => new HashSet<DSSign>() { DSSign.Negative, DSSign.Positive, DSSign.Zero},
            (DSSign.Positive, DSSign.Zero) => DSSign.Positive.Singleton().ToHashSet(),
            (DSSign.Positive, DSSign.Positive) => DSSign.Positive.Singleton().ToHashSet(),
        };
        
        private static HashSet<DSSign> MinusDSSigns(DSSign left, DSSign right) => (left, right) switch
        {
            (DSSign.Negative, DSSign.Negative) => new HashSet<DSSign>() { DSSign.Negative, DSSign.Positive, DSSign.Zero},
            (DSSign.Negative, DSSign.Zero) => DSSign.Negative.Singleton().ToHashSet(),
            (DSSign.Negative, DSSign.Positive) => DSSign.Negative.Singleton().ToHashSet(),
            (DSSign.Zero, DSSign.Negative) => DSSign.Positive.Singleton().ToHashSet(),
            (DSSign.Zero, DSSign.Zero) => DSSign.Zero.Singleton().ToHashSet(),
            (DSSign.Zero, DSSign.Positive) => DSSign.Negative.Singleton().ToHashSet(),
            (DSSign.Positive, DSSign.Negative) => DSSign.Positive.Singleton().ToHashSet(),
            (DSSign.Positive, DSSign.Zero) => DSSign.Positive.Singleton().ToHashSet(),
            (DSSign.Positive, DSSign.Positive) => new HashSet<DSSign>() { DSSign.Negative, DSSign.Positive, DSSign.Zero},
        };
        
        private static HashSet<DSSign> MultiplyDSSigns(DSSign left, DSSign right) => (left, right) switch
        {
            (DSSign.Negative, DSSign.Negative) => DSSign.Positive.Singleton().ToHashSet(),
            (DSSign.Negative, DSSign.Zero) => DSSign.Zero.Singleton().ToHashSet(),
            (DSSign.Negative, DSSign.Positive) => DSSign.Negative.Singleton().ToHashSet(),
            (DSSign.Zero, DSSign.Negative) => DSSign.Zero.Singleton().ToHashSet(),
            (DSSign.Zero, DSSign.Zero) => DSSign.Zero.Singleton().ToHashSet(),
            (DSSign.Zero, DSSign.Positive) => DSSign.Zero.Singleton().ToHashSet(),
            (DSSign.Positive, DSSign.Negative) => DSSign.Negative.Singleton().ToHashSet(),
            (DSSign.Positive, DSSign.Zero) => DSSign.Zero.Singleton().ToHashSet(),
            (DSSign.Positive, DSSign.Positive) => DSSign.Positive.Singleton().ToHashSet(),
        };

        private static HashSet<DSSign> DivideDSSigns(DSSign left, DSSign right) => (left, right) switch
        {
            (DSSign.Negative, DSSign.Negative) => DSSign.Positive.Singleton().ToHashSet(),
            (DSSign.Negative, DSSign.Zero) => new HashSet<DSSign>(),
            (DSSign.Negative, DSSign.Positive) => DSSign.Negative.Singleton().ToHashSet(),
            (DSSign.Zero, DSSign.Negative) => DSSign.Zero.Singleton().ToHashSet(),
            (DSSign.Zero, DSSign.Zero) => new HashSet<DSSign>(),
            (DSSign.Zero, DSSign.Positive) => DSSign.Zero.Singleton().ToHashSet(),
            (DSSign.Positive, DSSign.Negative) => DSSign.Negative.Singleton().ToHashSet(),
            (DSSign.Positive, DSSign.Zero) => new HashSet<DSSign>(),
            (DSSign.Positive, DSSign.Positive) => DSSign.Positive.Singleton().ToHashSet(),
        };
        
    }
}