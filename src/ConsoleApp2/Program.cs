using System;
using Analysis.Analysis.IntervalAnalysis;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            var val3 = new ExtendedZ(3);
            var val4 = new ExtendedZ(4);
            var posinf = ExtendedZ.PositiveInfinity();
            var negainf = ExtendedZ.NegativeInfinity();
            
            Console.WriteLine($"3 <= 4: {val3 <= val4}");
            Console.WriteLine($"4 <= 3: {val4 <= val3}");
            Console.WriteLine($"3 <= 3: {val3 <= val3}");
            Console.WriteLine($"negaInf <= 4: {negainf <= val4}");
            Console.WriteLine($"4 <= negaInf: {val4 <= negainf}");
            Console.WriteLine($"negainf <= posinf: {negainf <= posinf}");
            Console.WriteLine($"negainf <= negainf: {negainf <= negainf}");
            Console.WriteLine($"posinf <= 4: {posinf <= val4}");
            Console.WriteLine($"4 <= posinf: {val4 <= posinf}");
            Console.WriteLine($"posinf <= negainf: {posinf <= negainf}");
            Console.WriteLine($"posinf <= posinf: {posinf <= posinf}");
            
        }
    }
}