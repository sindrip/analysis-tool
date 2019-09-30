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
            

            var b = new ScopedBlock(new List<IStatement>()
            {
                new IntDecl("x"),
                new ArrayDecl("a", 10),
                new WhileStmt(new BoolLit(false), new UnscopedBlock(new List<IStatement>()
                {
                    new AssignStmt(new VarAccess("x"), new IntLit(3)),
                })),
                new AssignStmt(new VarAccess("x"), new IntLit(5)),
            });
            
            var p = new Analysis.AST.Program(b);
            
            Console.WriteLine(b);
            
            var fg = new FlowGraph(p);
            Console.WriteLine(fg.Inital);
            Console.WriteLine(string.Join(" ", fg.Final));
            Console.WriteLine(string.Join("\n", fg.Blocks.Select(s => s.PrintBlock())));
            Console.WriteLine(fg.Edges.Count());
            Console.WriteLine(string.Join("\r\n", fg.Edges));
            

        }
    }
}