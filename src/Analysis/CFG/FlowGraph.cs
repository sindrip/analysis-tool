using System;
using System.Collections.Generic;
using System.Linq;
using Analysis.AST;
using Analysis.AST.Statement;

namespace Analysis.CFG
{
    public class FlowGraph
    {
        public IAstNode Program;
        public int Inital;
        public HashSet<int> Final;
        public HashSet<(int, int)> Edges;
        public HashSet<(int, int)> ReverseEdges;
        public IEnumerable<IStatement> Blocks;

        public FlowGraph(IAstNode program)
        {
            Program = program;
            Blocks = FlowUtil.Blocks(Program);
            FlowUtil.LabelProgram(Blocks);
            Inital = FlowUtil.Init(Program);
            Final = FlowUtil.Final(Program).ToHashSet();
            Edges = FlowUtil.Flow(Program).ToHashSet();
            ReverseEdges = FlowUtil.FlowR(Edges);
        }

        public string ToGraphvizFormat()
        {
            const string firstNode = "first[label=\"\",shape=none,height=0,width=0]";
            var nodes = string.Join(" ", Blocks.Select(BlockToNode));
            var relations = string.Join(" ", Edges.Select(e => $"{e.Item1} -> {e.Item2};"));
            var entryArrow = $"first -> {Inital};";
            return $"digraph {{ node [shape=box] {firstNode} {nodes} {relations} {entryArrow}}}";
        }

        private string BlockToNode(IStatement block)
        {
            return $"{block.Label}[label=<[{block.ToString()}]<SUP>{block.Label}</SUP>>];";
        }
        
    }
}