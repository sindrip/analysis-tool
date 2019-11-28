using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using Analysis.AST;

namespace Analysis.Analysis.IntervalAnalysis
{
    public class Interval
    {
        public ExtendedZ LowerBound { get; private set; }
        public ExtendedZ UpperBound { get; private set; }
        public bool IsBottom { get; private set; }

        public Interval(ExtendedZ lower, ExtendedZ upper)
        {
            LowerBound = lower;
            UpperBound = upper;
        }

        private Interval() => IsBottom = true;

        public static Interval Bottom() => new Interval();

        public static Interval Top() =>
            new Interval(ExtendedZ.NegativeInfinity(), ExtendedZ.PositiveInfinity());

        public bool IsTop() => LowerBound.NegativeInf && UpperBound.PositiveInf;

        public Interval Join(Interval right)
        {
            if (IsBottom)
            {
                return right;
            }

            if (right.IsBottom)
            {
                return new Interval(LowerBound, UpperBound);
            }

            var min = ExtendedZ.Min(LowerBound, right.LowerBound);
            var max = ExtendedZ.Max(UpperBound, right.UpperBound);

            return new Interval(min, max);
        }

        public Interval Meet(Interval right)
        {
            if (IsBottom || right.IsBottom)
                return Bottom();

            var max = ExtendedZ.Max(LowerBound, right.LowerBound);
            var min = ExtendedZ.Min(UpperBound, right.UpperBound);
            // TODO: fix this
            if (max <= min)
            {
                return new Interval(max, min);
            }

            return Bottom();
        }

        public Interval Copy()
        {
            if (IsBottom)
                return new Interval();

            var lb = LowerBound.Copy();
            var ub = UpperBound.Copy();
            
            return new Interval(lb, ub);
        }

        public static bool operator <=(Interval left, Interval right)
        {
            if (left.IsBottom)
                return true;

            if (right.IsBottom)
                return false;

            return (right.LowerBound <= left.LowerBound) && (left.UpperBound <= right.UpperBound);
        }

        public static bool operator >=(Interval left, Interval right)
        {
            throw new NotImplementedException();
        }

        public static Interval operator +(Interval left, Interval right)
        {
            if (left.IsBottom || right.IsBottom)
                return Bottom();

            return new Interval(left.LowerBound + right.LowerBound, left.UpperBound + right.LowerBound);
        }
        
        public static Interval operator -(Interval left, Interval right)
        {
            if (left.IsBottom || right.IsBottom)
                return Bottom();

            return new Interval(left.LowerBound - right.UpperBound, left.UpperBound - right.LowerBound);
        }
        public static Interval operator *(Interval left, Interval right)
        {
            if (left.IsBottom || right.IsBottom)
                return Bottom();

            var llb = left.LowerBound;
            var lub = left.UpperBound;
            var rlb = right.LowerBound;
            var rub = right.UpperBound;

            var l = new List<ExtendedZ> {llb * rlb, llb * rub, lub * rlb, lub * rub};
            var min = l.Aggregate(ExtendedZ.PositiveInfinity(), (x, y) => ExtendedZ.Min(x, y));
            var max = l.Aggregate(ExtendedZ.NegativeInfinity(), (x, y) => ExtendedZ.Max(x, y));
            return new Interval(min, max);
        }
        public static Interval operator /(Interval left, Interval right)
        {
            if (left.IsBottom || right.IsBottom || right.ContainsZero())
                return Bottom();
            
            var llb = left.LowerBound;
            var lub = left.UpperBound;
            var rlb = right.LowerBound;
            var rub = right.UpperBound;
            
            var l = new List<ExtendedZ> {llb / rlb, llb / rub, lub / rlb, lub / rub};
            var min = l.Aggregate(ExtendedZ.PositiveInfinity(), (x, y) => ExtendedZ.Min(x, y));
            var max = l.Aggregate(ExtendedZ.NegativeInfinity(), (x, y) => ExtendedZ.Max(x, y));
            return new Interval(min, max);
        }

        public Interval ToIntervalK(Program program)
        {
            var iv = AnalysisUtil.InterestingValues(program).ToList();
            
            if (IsBottom)
                return Bottom();
            var lb = Sup(iv, LowerBound);
            var ub = Inf(iv, UpperBound);
            return new Interval(lb, ub);
        }

        private ExtendedZ Sup(List<BigInteger> iv, ExtendedZ n)
        {
            if (n.NegativeInf)
                return ExtendedZ.NegativeInfinity();

            if (n.PositiveInf)
                return new ExtendedZ(iv.Max());

            var lt = iv.Where(k => k <= n.Value).ToList();
            if (lt.Count == 0)
                return ExtendedZ.NegativeInfinity();
            
            return new ExtendedZ(lt.Max());
        }

        private ExtendedZ Inf(List<BigInteger> iv, ExtendedZ n)
        {
            if (n.PositiveInf)
                return ExtendedZ.PositiveInfinity();
            
            if (n.NegativeInf)
                return new ExtendedZ(iv.Min());

            var gt = iv.Where(k => n.Value <= k).ToList();
            if (gt.Count == 0)
                return ExtendedZ.PositiveInfinity();
            
            return new ExtendedZ(gt.Min());
        }

        private bool ContainsZero()
        {
            if (LowerBound.PositiveInf)
                return false;
            
            if (LowerBound.NegativeInf)
            {
                if (UpperBound.PositiveInf)
                    return true;
                if (UpperBound.NegativeInf)
                    return false;

                return UpperBound.Value >= 0;
            }

            if (UpperBound.PositiveInf)
            {
                return LowerBound.Value <= 0;
            }

            return LowerBound.Value == 0
                   || UpperBound.Value == 0
                   || ((LowerBound.Value <= 0) && (UpperBound.Value >= 0));
        }

        public override string ToString() => $"[{LowerBound}, {UpperBound}]";
    }
    
}