using System;
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
                return Interval.Bottom();

            
        }
        
        public static Interval operator -(Interval left, Interval right)
        {
            
        }
        public static Interval operator *(Interval left, Interval right)
        {
            
        }
        public static Interval operator /(Interval left, Interval right)
        {
            
        }

        public override string ToString() => $"[{LowerBound}, {UpperBound}]";
    }
    
}