using System;
using Analysis.Analysis.IntervalAnalysis;
using Analysis.Analysis.ReachingDefinitions;

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

            var input4 = @"
{
    int x;
    y := 0;
}
";

            var input5 = @"
{
  { int fst; int snd } r;
  int x;
  r := (0,1);
  read x;
  while (x > 0) {
    r.fst := r.fst + x;
    r.snd := r.snd + x;
    x := x-1;
  }
  r := (0,0);
}
";
            //var result = Parser.Util.StringToAst(input);
            //var analysis = new IAAnalysis(result);
            //Console.WriteLine(analysis);
            //var result2 = Parser.Util.StringToAst(input2);
            //var analysis2 = new IAAnalysis(result2);
            //Console.WriteLine(analysis2);
            //var result3 = Parser.Util.StringToAst(input3);
            //var analysis3 = new IAAnalysis(result3, "FIFOWorklist");
            //Console.WriteLine(analysis3);
            //var result4 = Parser.Util.StringToAst(input4);
            //var analysis4 = new IAAnalysis(result4, "FIFOWorklist");
            //Console.WriteLine(analysis4);
            var result5 = Parser.Util.StringToAst(input5);
            var analysis5 = new RDAnalysis(result5, "FIFOWorklist");
            Console.WriteLine(analysis5);

        }
    }
}