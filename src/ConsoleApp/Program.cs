using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Analysis.Analysis;
using Analysis.Analysis.AvailableExpressions;
using Analysis.Analysis.DetectionOfSigns;
using Analysis.Analysis.FaintVariables;
using Analysis.Analysis.LiveVariables;
using Analysis.Analysis.ReachingDefinitions;
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
    
    x := 0 + 1 + 2;
    y := x;
    if (not true & false) {
        x := 3;
    } else {
        y := 3;
    }
    a[1+1] := (1+1)*2;
}
";
            
            var aeinput = @"
{
    int x;
    int y;
    int a;
    int b;
    x := a + b;
    y := a * b;
    while (y > a + b) {
        a := a + 1;
        x := a + b;
    }
}";

            var rdinput = @"
{
    int fst;
    int snd;
    int x;

    x := 5;
    fst := 0;
    snd := 1;

    while (x > 0) {
        fst := fst + x;
        snd := snd * x;
        x := x - 1;
    }
    fst := 0;
    snd := 0;
}
";

            var lvinput = @"
{
    { int fst; int snd } r;
    int x;
    
    r := (0,1);

    read x;

    while (x > 0) {
        r.fst := r.fst + x;
        r.snd := r.snd * x;
        x := x - 1;
    }

    r := (0, 0);
}
";

            // var result = Parser.Util.StringToAst(input);
            // var aeresult = Parser.Util.StringToAst(aeinput);
            var rdresult = Parser.Util.StringToAst(rdinput);
            var lvresult = Parser.Util.StringToAst(lvinput);
            // Console.WriteLine(result);
            // var fg = new FlowGraph(result);
            // Console.WriteLine(fg.Inital);
            // Console.WriteLine(string.Join(" ", fg.Final));
            // Console.WriteLine(string.Join("\n", fg.Blocks.Select(s => s.PrintBlock())));
            // Console.WriteLine(fg.Edges.Count());
            // Console.WriteLine(string.Join("\r\n", fg.Edges));
            // var fv = Analysis.Analysis.AnalysisUtil.FreeVariables(result);

            // Console.WriteLine(fv.Count);

            // var exp1 = new ABinOp(new IntLit(3), new IntLit(4), ABinOperator.Plus);
            // var exp2 = new ABinOp(new IntLit(3), new IntLit(4), ABinOperator.Plus);
            // var exp3 = new ABinOp(new IntLit(4), new IntLit(4), ABinOperator.Plus);

            // var exp4 = new ABinOp(exp1, exp2, ABinOperator.Mult);
            // var exp5 = new ABinOp(exp4, exp3, ABinOperator.Div);


            // Console.WriteLine(exp1 == exp2); // Should be false as we are not overloading the operator
            // Console.WriteLine(exp1.Equals(exp2)); // Should be true as we have implemented the IEquatable interface
            // Console.WriteLine(exp1.Equals(exp3)); // Should be false, different expressions

            // var ae = Analysis.Analysis.AnalysisUtil.AvailableExpressions(exp5);
            // var ae2 = Analysis.Analysis.AnalysisUtil.AvailableExpressions(result);
            // Console.WriteLine("=========");
            // Console.WriteLine(ae);
            // Console.WriteLine(ae2);

            //// Overflowing program debug
            //var overflow = "{int x; x:=0; Point.x := 0;}";
            //var overflowParse = Parser.Util.StringToAst(overflow);
            //
            //Console.WriteLine(overflowParse);

            //var x = new Identifier("x", "int", 0);
            //var y = new Identifier("y", "int", 2);
            //var A = new Identifier("A", "array", 1);
            //var d1 = new Dictionary<Identifier, HashSet<int>>();
            //d1[x] = new HashSet<int> {1,2};
            //d1[A] = new HashSet<int> {1};
            //d1[y] = new HashSet<int>() {3};
            //var d2 = new Dictionary<Identifier, HashSet<int>>();
            //d2[x] = new HashSet<int> {1,2,3};
            //d2[A] = new HashSet<int> {2};
            //var l1 = new RDLattice(d1);
            //var l2 = new RDLattice(d2);
            //Console.WriteLine(l1.PartialOrder(l2));
            //var joined = l1.Join(l2);
            //Console.WriteLine(joined);
            
           // var n = d2.Except(d1);
           // var s = string.Join("\n", n.Select(x => $"{x.Key}: {string.Join(",", x.Value)}"));
           // Console.WriteLine(s);

            //var hs = new HashSet<int?>();
            //hs.Add(1);
            //hs.Add(null);
            //hs.Add(null);
            //hs.Add(2);
            //Console.WriteLine(hs.Count);
            
            
            //var t = new AELattice(ae2);
            //var t2 = new AELattice(ae);
            //Console.WriteLine(t);
            //Console.WriteLine(t2);
            //Console.WriteLine(t <= t);
            //Console.WriteLine(t2 <= (t & t2));
            //
            // var analysis = new AEAnalysis(aeresult);
            // Console.WriteLine(analysis);
            // var analysis = new RDAnalysis(rdresult);
            // Console.WriteLine(analysis);

            var lvAnalysis = new LVAnalysis(lvresult);
            // var analysis3 = new FVAnalysis(lvresult);
            // var analysis4 = new DSAnalysis(lvresult);
            Console.WriteLine(lvAnalysis);
            // Console.WriteLine(analysis3);
            // Console.WriteLine(analysis4);
        }
    }
}
