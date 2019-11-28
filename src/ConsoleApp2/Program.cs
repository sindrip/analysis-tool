﻿using System;
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

            var input = @"
{
    int x;
    x := 1;
    while (x > 0) {
        x := x - 1;
    }
}
";

            var input2 = @"
{
    int x;
    int[1] a;
    read a[0];
    a[0] := 1;
    read a[0];
    if (x > 0) {
        a[-1] := 0;
    }
    read a[0];
}
";

            var input3 = @"
{
int x;
x := -1;
while (x > 0) {
    x := x - 1;
}
x := x * x;
}
";
            //var result = Parser.Util.StringToAst(input);
            //var analysis = new IAAnalysis(result);
            //Console.WriteLine(analysis);
            //var result2 = Parser.Util.StringToAst(input2);
            //var analysis2 = new IAAnalysis(result2);
            //Console.WriteLine(analysis2);
            var result3 = Parser.Util.StringToAst(input3);
            var analysis3 = new IAAnalysis(result3, "FIFOWorklist");
            Console.WriteLine(analysis3);

        }
    }
}