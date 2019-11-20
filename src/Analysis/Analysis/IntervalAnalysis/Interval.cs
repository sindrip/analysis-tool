using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Intrinsics;

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

            return (left.LowerBound <= right.LowerBound) && (left.LowerBound <= right.LowerBound);
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
            return new Interval(l.Min(), l.Max());
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
            return new Interval(l.Min(), l.Max());
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

                return UpperBound.Value > 0;
            }

            return LowerBound.Value > 0;
        }

        public override string ToString() => $"[{LowerBound}, {UpperBound}]";
    }
    
}