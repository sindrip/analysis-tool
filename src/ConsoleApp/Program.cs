using System;
using System.Linq;
using Analysis.Analysis;
using Analysis.Analysis.FaintVariables;
using Analysis.Analysis.IntervalAnalysis;
using Analysis.CFG;
using Analysis.AST.AExpr;
using Analysis.Analysis.ReachingDefinitions;

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
}";

            var rdinput = @"
{
int x;
int y;
int q;
int r;
if (x >= 0 & y >0) {
q := 0;
r := x;
while ( r >= y) {
r := r - y;
q := q + 1;
while ( r >= y) {
r := r - y;
q := q + 1;
while ( r >= y) {
r := r - y;
q := q + 1;
}
}
}
} else {
while ( r >= y) {
r := r - y;
q := q + 1;
while ( r >= y) {
    r := r - y;
    q := q + 1;
    while ( r >= y) {
        r := r - y;
        q := q + 1;
    }
}
}
while ( r >= y) {
r := r - y;
q := q + 1;
while ( r >= y) {
    r := r - y;
    q := q + 1;
}           
}

}
write r;
}
";

            var lvinput = @"
{
int x;
int y;
int q;
int r;
if (x >= 0 & y >0) {
q := 0;
r := x;
while ( r >= y) {
r := r - y;
q := q + 1;
while ( r >= y) {
r := r - y;
q := q + 1;
while ( r >= y) {
r := r - y;
q := q + 1;
}
}
}
} else {
while ( r >= y) {
r := r - y;
q := q + 1;
while ( r >= y) {
    r := r - y;
    q := q + 1;
    while ( r >= y) {
        r := r - y;
        q := q + 1;
    }
}
}
while ( r >= y) {
r := r - y;
q := q + 1;
while ( r >= y) {
    r := r - y;
    q := q + 1;
}           
}

}
write r;
}

";

            var input1 = @"
{
{ int fst; int snd } r;
int x;

r := (0,1);


while (x > 0) {
r.fst := r.fst + x;
r.snd := r.snd * x;
x := x - 1;
}

r := (0, 0);
}
";

            var input2 = @"
{
int x;
int y;
int q;
int r;
if (x >= 0 & y >0) {
q := 0;
r := x;
while ( r >= y) {
r := r - y;
q := q + 1;
while ( r >= y) {
r := r - y;
q := q + 1;
while ( r >= y) {
r := r - y;
q := q + 1;
}
}
}
} 
write r;
}
";
            var input3 = @"
{
int x;
int y;
int q;
int r;
if (x >= 0 & y >0) {
q := 0;
r := x;
while ( r >= y) {
r := r - y;
q := q + 1;
while ( r >= y) {
r := r - y;
q := q + 1;
while ( r >= y) {
r := r - y;
q := q + 1;
}
}
}
} else {
while ( r >= y) {
r := r - y;
q := q + 1;
while ( r >= y) {
    r := r - y;
    q := q + 1;
    while ( r >= y) {
        r := r - y;
        q := q + 1;
    }
}
}
while ( r >= y) {
r := r - y;
q := q + 1;
while ( r >= y) {
    r := r - y;
    q := q + 1;
}           
}

}
write r;
}
";


            var result1 = Parser.Util.StringToAst(input1);
            var result2 = Parser.Util.StringToAst(input2);
            var result3 = Parser.Util.StringToAst(input3);

            var result = Parser.Util.StringToAst(input);
            var aeresult = Parser.Util.StringToAst(aeinput);
            var rdresult = Parser.Util.StringToAst(rdinput);
            var lvresult = Parser.Util.StringToAst(lvinput);
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
            Console.WriteLine("=========");
            Console.WriteLine(ae);
            Console.WriteLine(ae2);


            Console.WriteLine("------- Analysis 1 --------");
            var analysis1 = new RDAnalysis(result1, "RoundRobin");
            Console.WriteLine(analysis1);

        }
    }
}
