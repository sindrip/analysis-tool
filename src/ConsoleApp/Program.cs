using System;
using System.Collections.Generic;
using System.Linq;
using Analysis.CFG;
using Analysis.AST;
using Analysis.AST.AExpr;
using Analysis.AST.BExpr;
using Analysis.AST.Statement;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            //var input = "{ int x; x := 3; }";
            var input = @"
{
    int x;
    int [10] a;
    int y;
    
    x := 0;
    y := x;
    if (not true & false) {
        x := 3;
    } else {
        y := 3;
    }
    a[1+1] := (1+1)*2;
}
";

            var result = Parser.Util.StringToAst(input);
            Console.WriteLine(result);
            var fg = new FlowGraph(result);
            Console.WriteLine(fg.Inital);
            Console.WriteLine(string.Join(" ", fg.Final));
            Console.WriteLine(string.Join("\n", fg.Blocks.Select(s => s.PrintBlock())));
            Console.WriteLine(fg.Edges.Count());
            Console.WriteLine(string.Join("\r\n", fg.Edges));
            var fv = Analysis.Analysis.Util.FreeVariables(result);
            var fv1 = Analysis.Analysis.Util.FreeVariables(result);
            
            Console.WriteLine(fv.Count);

        }
    }
}