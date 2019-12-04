using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Analysis.CFG;

namespace Analysis.Analysis
{
    public interface IWorkList
    {   
        FlowEdge Extract();
        void Insert(FlowEdge flowEdge);
        bool Empty();
        List<FlowEdge> GetCurrentEdges();
        LinkedList<FlowEdge> GetV();
        LinkedList<FlowEdge> GetP();
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

        public List<FlowEdge> GetCurrentEdges()
        {
            return _edgeList.ToList();
        }

        public LinkedList<FlowEdge> GetV()
        {
            return new LinkedList<FlowEdge>();
        }

        public LinkedList<FlowEdge> GetP()
        {
            return new LinkedList<FlowEdge>();
        }
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

        public List<FlowEdge> GetCurrentEdges()
        {
            return _edgeList.ToList();
        }

        public LinkedList<FlowEdge> GetP()
        {
            return new LinkedList<FlowEdge>();
        }

        public LinkedList<FlowEdge> GetV()
        {
            return new LinkedList<FlowEdge>();
        }

        public void Insert(FlowEdge flowEdge)
        {
            if (!_edgeList.Contains(flowEdge))
            {
                _edgeList.Enqueue(flowEdge);
            }
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

        public List<FlowEdge> GetCurrentEdges()
        {
            return _edgeList.ToList();
        }

        public LinkedList<FlowEdge> GetP()
        {
            return new LinkedList<FlowEdge>();
        }

        public LinkedList<FlowEdge> GetV()
        {
            return new LinkedList<FlowEdge>();
        }

        public void Insert(FlowEdge flowEdge)
        {
            _edgeList.Push(flowEdge);
        }

    }

    public class RoundRobin : IWorkList
    {
        private VContainer V;
        private LinkedList<FlowEdge> P;
        private List<(int, int)> rP;

        public RoundRobin(IEnumerable<FlowEdge> edgeList, List<(int, int)> rP)
        {
            this.rP = rP;
            LinkedList<FlowEdge> sortedEdgeList = SortRP(new LinkedList<FlowEdge>(edgeList));

            this.P = new LinkedList<FlowEdge>();
            this.V = new VContainer(sortedEdgeList);
        }

        public bool Empty() => V.IsEmpty() && P.Count == 0;

        public FlowEdge Extract()
        {
            if (V.IsEmpty())
            {
                V = new VContainer(SortRP(P));
                FlowEdge q = V.PopFirst();
                P.Clear();
                return q;
            }
            else
            {
                FlowEdge q = V.PopFirst();
                return q;
            }
        }

        public List<FlowEdge> GetCurrentEdges()
        {
            return V.GetLinkedList().ToList();
        }

        public LinkedList<FlowEdge> GetP()
        {
            return P;
        }

        public LinkedList<FlowEdge> GetV()
        {
            return V.GetLinkedList();
        }

        public void Insert(FlowEdge flowEdge)
        {
            if (!V.Contains(flowEdge) && !P.Contains(flowEdge))
            {
                P.AddLast(flowEdge);
            }
        }

        private LinkedList<FlowEdge> SortRP(LinkedList<FlowEdge> listToSort)
        {
            List<int> rPSortOrder = rP.Select(x => x.Item1).ToList();

            return new LinkedList<FlowEdge>(listToSort.OrderBy(x => rPSortOrder.IndexOf(x.Source)).ToList());
        }
    }

    class VContainer
    {
        LinkedList<FlowEdge> VLinkedList = new LinkedList<FlowEdge>();
        Dictionary<FlowEdge, int> VDictionary = new Dictionary<FlowEdge, int>();

        public VContainer(LinkedList<FlowEdge> linkedList)
        {
            VLinkedList = new LinkedList<FlowEdge>(linkedList);

            foreach (var edge in linkedList)
            {
                int ret;
                if (VDictionary.TryGetValue(edge, out ret))
                {
                    VDictionary[edge] = ret + 1;
                } else
                {
                    VDictionary[edge] = 1;
                }
            }
        }

        public bool Contains(FlowEdge edge)
        {
            int ret;
            return VDictionary.TryGetValue(edge, out ret);
        }

        public bool IsEmpty() => VLinkedList.Count == 0;

        public LinkedList<FlowEdge> GetLinkedList()
        {
            return VLinkedList;
        }

        internal FlowEdge PopFirst()
        {
            FlowEdge q = VLinkedList.First();
            VLinkedList.RemoveFirst();

            VDictionary[q] -= 1;

            if (VDictionary[q] == 0)
            {
                VDictionary.Remove(q);
            }

            return q;
        }

    }
}