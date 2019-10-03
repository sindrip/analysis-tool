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
        
    }
}