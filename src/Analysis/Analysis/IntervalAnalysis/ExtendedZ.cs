using System.Numerics;

namespace Analysis.Analysis.IntervalAnalysis
{
    public class ExtendedZ
    {
        public BigInteger Value { get; private set; }
        public bool NegativeInf { get; private set; }
        public bool PositiveInf { get; private set; }

         ExtendedZ()
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

        public void Deconstruct(out BigInteger val, out bool nega, out bool pos)
        {
            val = Value;
            nega = NegativeInf;
            pos = PositiveInf;
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
            
        }
        
        public static ExtendedZ operator -(ExtendedZ left, ExtendedZ right)
        {
            
        }
        public static ExtendedZ operator *(ExtendedZ left, ExtendedZ right)
        {
            
        }
        public static ExtendedZ operator /(ExtendedZ left, ExtendedZ right)
        {
            
        }

    }
}