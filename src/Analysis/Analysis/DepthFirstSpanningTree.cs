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
        private Dictionary<int, int> uP; // the initial value of k when calling DFS (label, value)
        private Dictionary<int, Interval<int>> iP; // the interval between rP and uP 
        private int k; // the number so far 

        public DepthFirstSpanningTree(FlowGraph flowGraph)
        {
            this.flowGraph = flowGraph;
            T = new HashSet<FlowEdge>();
            V = new HashSet<int>();
            k = flowGraph.Blocks.Count();

            rP = new List<(int, int)>();
            uP = new Dictionary<int, int>();
            iP = new Dictionary<int, Interval<int>>();

            DFS(this.flowGraph.Inital);

            foreach (var element in flowGraph.Blocks)
            {
                Console.WriteLine("blocklabels: " + element.Label);
            }

            Console.WriteLine("rp:");
            foreach (var rpelement in rP)
            {
                Console.WriteLine("label: " + rpelement.Item1);
                Console.WriteLine("reverse postorder number: " + rpelement.Item2);
            }
        }

        private void DFS(int label)
        {
            V.Add(label);

            while (flowEdgeHasntDestInV() != null)
            {
                FlowEdge edge = flowEdgeHasntDestInV().Value;
                this.T.Add(edge);
                DFS(edge.Dest);
                rP.Add((label, k));
                Console.WriteLine("label: " + label);
                k--;
            }
        }

        private FlowEdge? flowEdgeHasntDestInV ()
        {
            foreach (FlowEdge flowEdge in flowGraph.Edges)
            {
                if (!V.Contains(flowEdge.Dest))
                {
                    return flowEdge;
                }
            }
            return null;
        }

        public List<(int, int)> getRP()
        {
            return this.rP;
        }

        public class Interval<T> where T : struct, IComparable
        {
            public T start { get; set; }
            public T end { get; set; }

            public Interval(T start, T end)
            {
                this.start = start;
                this.end = end;
            }

            public bool InRange(T value)
            {
                return (value.CompareTo(start) > 0) && (end.CompareTo(value) > 0);
            }
        }
    }
}
