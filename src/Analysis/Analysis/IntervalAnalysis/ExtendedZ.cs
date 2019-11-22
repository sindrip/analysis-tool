using System;
using System.Numerics;

namespace Analysis.Analysis.IntervalAnalysis
{
    public class ExtendedZ
    {
        public BigInteger Value { get; private set; }
        public bool NegativeInf { get; private set; }
        public bool PositiveInf { get; private set; }

        private ExtendedZ()
        {
        }

        public ExtendedZ(BigInteger value)
        {
            Value = value;
            NegativeInf = false;
            PositiveInf = false;
        }

        public static ExtendedZ NegativeInfinity()
        {
            var nega = new ExtendedZ();
            nega.NegativeInf = true;
            return nega;
        }

        public static ExtendedZ PositiveInfinity()
        {
            var pos = new ExtendedZ();
            pos.PositiveInf = true;
            return pos;
        }

        public static ExtendedZ Min(ExtendedZ left, ExtendedZ right)
        {
            if (left.NegativeInf || right.NegativeInf)
                return ExtendedZ.NegativeInfinity();

            if (left.PositiveInf)
                return right;

            if (right.PositiveInf)
                return left;

            return new ExtendedZ(BigInteger.Min(left.Value, right.Value));
        }

        public static ExtendedZ Max(ExtendedZ left, ExtendedZ right)
        {
            if (left.PositiveInf || right.PositiveInf)
                return ExtendedZ.PositiveInfinity();

            if (left.NegativeInf)
                return right;

            if (right.NegativeInf)
                return left;
            
            return new ExtendedZ(BigInteger.Max(left.Value, right.Value));
        }

        public ExtendedZ Copy()
        {
            if (NegativeInf)
                return NegativeInfinity();
            else if (PositiveInf)
                return PositiveInfinity();
            
            else return new ExtendedZ(Value);
        }

        public static bool operator <=(ExtendedZ left, ExtendedZ right)
        {
            if (left.NegativeInf)
                return true;

            if (right.PositiveInf)
                return true;

            if (left.PositiveInf)
            {
                if (right.PositiveInf)
                {
                    return true;
                }

                return false;
            }

            if (right.NegativeInf)
                return false;

            return left.Value <= right.Value;
        }

        public static bool operator >=(ExtendedZ left, ExtendedZ right)
        {
            if (left.PositiveInf)
                return true;

            if (right.NegativeInf)
                return true;

            if (left.NegativeInf)
            {
                if (right.NegativeInf)
                {
                    return true;
                }

                return false;
            }

            return left.Value >= right.Value;
        }

        public static ExtendedZ operator +(ExtendedZ left, ExtendedZ right)
        {
            if (left.NegativeInf || right.NegativeInf)
                return NegativeInfinity();

            if (left.PositiveInf || right.PositiveInf)
                return PositiveInfinity();
            
            return new ExtendedZ(left.Value + right.Value);
        }
        
        public static ExtendedZ operator -(ExtendedZ left, ExtendedZ right)
        {
            if (left.NegativeInf || right.NegativeInf)
                return NegativeInfinity();

            if (left.PositiveInf || right.PositiveInf)
                return PositiveInfinity();
            
            return new ExtendedZ(left.Value - right.Value);
        }
        
        public static ExtendedZ operator *(ExtendedZ left, ExtendedZ right)
        {
            if (left.NegativeInf || right.NegativeInf)
                return NegativeInfinity();

            if (left.PositiveInf || right.PositiveInf)
                return PositiveInfinity();
            
            return new ExtendedZ(left.Value * right.Value);
        }
        
        public static ExtendedZ operator /(ExtendedZ left, ExtendedZ right)
        {
            if (left.NegativeInf || right.NegativeInf)
                return NegativeInfinity();

            if (left.PositiveInf || right.PositiveInf)
                return PositiveInfinity();
            
            return new ExtendedZ(left.Value / right.Value);
        }

        public override string ToString()
        {
            if (PositiveInf)
                return "+inf";
            else if (NegativeInf)
                return "-inf";
            
            return Value.ToString();
        }
    }
}