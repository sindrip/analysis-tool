using System.Collections.Generic;
using System.Linq;
using Analysis.CFG;

namespace Analysis.Analysis
{
    public interface IWorkList
    {   
        FlowEdge Extract();
        void Insert(FlowEdge flowEdge);
        bool Empty();
    }

    public class ChaoticIteration : IWorkList
    {
        private HashSet<FlowEdge> _edgeList;
        
        public ChaoticIteration(IEnumerable<FlowEdge> edges) => _edgeList = edges.ToHashSet();
            
        public FlowEdge Extract()
        {
            var edge = _edgeList.First();
            _edgeList.Remove(edge);
            return edge;
        }

        public void Insert(FlowEdge flowEdge)
        {
            _edgeList.Add(flowEdge);
        }

        public bool Empty() => _edgeList.Count == 0;
    }

    public class FIFOWorklist : IWorkList
    {
        private Queue<FlowEdge> _edgeList;

        public FIFOWorklist(IEnumerable<FlowEdge> edges)
        {
            _edgeList = new Queue<FlowEdge>(edges);
        }
        public bool Empty() => _edgeList.Count == 0;

        public FlowEdge Extract()
        {
            return _edgeList.Dequeue();
        }

        public void Insert(FlowEdge flowEdge)
        {
            _edgeList.Enqueue(flowEdge);
        }

    }

    public class LIFOWorklist : IWorkList
    {
        private Stack<FlowEdge> _edgeList;

        public LIFOWorklist(IEnumerable<FlowEdge> edgeList)
        {
            _edgeList = new Stack<FlowEdge>(edgeList);
        }

        public bool Empty() => _edgeList.Count == 0;

        public FlowEdge Extract()
        {
            return _edgeList.Pop();
        }

        public void Insert(FlowEdge flowEdge)
        {
            _edgeList.Push(flowEdge);
        }

    }
}