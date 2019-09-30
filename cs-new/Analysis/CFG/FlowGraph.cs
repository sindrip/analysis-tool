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
            Blocks = FlowUtil.Blocks(program);
            FlowUtil.LabelProgram(Blocks);
            Inital = FlowUtil.Init(program);
            Final = FlowUtil.Final(program).ToHashSet();
            Edges = FlowUtil.Flow(program).ToHashSet();
            ReverseEdges = FlowUtil.FlowR(Edges);
        }
    }
}