using Analysis.CFG;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Analysis.Analysis
{
    class DepthFirstSpanningTree
    {
        private FlowGraph flowGraph; // input flowgraph
        private HashSet<FlowEdge> T; // the set of edges in the tree
        private List<(int, int)> rP; // a reverse postorder numbering of the nodes in the flowgraph
        private HashSet<int> V; // nodes visited so far
        private int k; // the number so far 

        public DepthFirstSpanningTree(FlowGraph flowGraph)
        {
            this.flowGraph = flowGraph;
            T = new HashSet<FlowEdge>();
            V = new HashSet<int>();
            k = flowGraph.Blocks.Count();

            rP = new List<(int, int)>();

            DFS(this.flowGraph.Inital);
        }

        private void DFS(int label)
        {
            V.Add(label);

            List<FlowEdge> list = FlowEdgeHasntDestInV(label);

            foreach (FlowEdge edge in list)
            {
                this.T.Add(edge);
                DFS(edge.Dest);
            }
            rP.Add((label, k));
            k--;
        }

        private List<FlowEdge> FlowEdgeHasntDestInV (int label)
        {
            return flowGraph.Edges.Where(x => x.Source == label && !V.Contains(x.Dest)).ToList();
        }

        public List<(int, int)> GetRP()
        {
            return this.rP;
        }
    }
}
