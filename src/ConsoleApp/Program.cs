using System;
using System.Collections.Generic;
using System.Linq;
using Analysis.Analysis;
using Analysis.AST;
using Analysis.CFG;
using Analysis.AST.AExpr;

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
            var fv = Analysis.Analysis.AnalysisUtil.FreeVariables(result);

            Console.WriteLine(fv.Count);

            var exp1 = new ABinOp(new IntLit(3), new IntLit(4), ABinOperator.Plus);
            var exp2 = new ABinOp(new IntLit(3), new IntLit(4), ABinOperator.Plus);
            var exp3 = new ABinOp(new IntLit(4), new IntLit(4), ABinOperator.Plus);

            var exp4 = new ABinOp(exp1, exp2, ABinOperator.Mult);
            var exp5 = new ABinOp(exp4, exp3, ABinOperator.Div);


            Console.WriteLine(exp1 == exp2); // Should be false as we are not overloading the operator
            Console.WriteLine(exp1.Equals(exp2)); // Should be true as we have implemented the IEquatable interface
            Console.WriteLine(exp1.Equals(exp3)); // Should be false, different expressions

            var ae = Analysis.Analysis.AnalysisUtil.AvailableExpressions(exp5);
            var ae2 = Analysis.Analysis.AnalysisUtil.AvailableExpressions(result);
            Console.WriteLine(ae);
            Console.WriteLine(ae2);

            //// Overflowing program debug
            //var overflow = "{int x; x:=0; Point.x := 0;}";
            //var overflowParse = Parser.Util.StringToAst(overflow);
            //
            //Console.WriteLine(overflowParse);

            var x = new Identifier("x", "int", 0);
            var A = new Identifier("A", "array", 1);
            var d1 = new Dictionary<Identifier, HashSet<int>>();
            d1[x] = new HashSet<int> {1,2};
            d1[A] = new HashSet<int> {1};
            var d2 = new Dictionary<Identifier, HashSet<int>>();
            d2[x] = new HashSet<int> {1,2,3};
            d2[A] = new HashSet<int> {2};
            var l1 = new RDLattice(d1);
            var l2 = new RDLattice(d2);
            Console.WriteLine(l1.PartialOrder(l2));
            var joined = l1.Join(l2);
            Console.WriteLine(joined);
        }
    }
}
