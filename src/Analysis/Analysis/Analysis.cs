using System;
using System.Collections.Generic;
using System.Linq;
using Analysis.AST;
using Analysis.AST.Statement;
using Analysis.CFG;

namespace Analysis.Analysis
{
    public abstract class Analysis<T>
    {
        protected IEnumerable<int> _extremalLabels;
        protected IEnumerable<FlowEdge> _flow;
        protected IEnumerable<IStatement> _blocks;
        protected List<ILattice<T>> _analysisFilled;
        protected List<ILattice<T>> _analysisCircle;
        protected Program _program;
        protected IWorkList _workList;
        
        protected abstract ILattice<T> TransferFunctions(int label);
        protected abstract ILattice<T> Iota();
        protected abstract ILattice<T> Bottom();

        public Analysis(Program program, AnalysisDirection direction)
        {
            _program = program;
            _blocks = FlowUtil.Blocks(program);
            
            var flow = FlowUtil.Flow(program);
            if (direction == AnalysisDirection.Forward)
            {
                _flow = flow;
                _extremalLabels = FlowUtil.Init(program).Singleton();
            }
            else
            {
                _flow = FlowUtil.FlowR(flow);
                _extremalLabels = FlowUtil.Final(program);
            }
            
            _analysisFilled = new List<ILattice<T>>();
            _analysisCircle = new List<ILattice<T>>();
        }
        
        protected IStatement GetBlock(int label) => _blocks.First(x => x.Label == label);
        protected IEnumerable<int> GetLabels() => _blocks.Select(b => b.Label);

        protected void InitalizeAnalysis()
        {
            var orderedBlocks = _blocks.OrderBy(x => x.Label);
            foreach (var b in orderedBlocks)
            {
                if (_extremalLabels.Contains(b.Label))
                {
                    _analysisFilled.Add(Iota());
                }
                else
                {
                    _analysisFilled.Add(Bottom());
                }

                _analysisCircle.Add(Bottom());
            }
        }

        protected void RunAnalysis()
        {
            DepthFirstSpanningTree dfst = new DepthFirstSpanningTree(new FlowGraph(_program));
            _workList = new RoundRobin(_flow, dfst.GetRP());

            int numberOfOperations = 0;

            while (!_workList.Empty())
            {
                var edge = _workList.Extract();
                numberOfOperations++;
                var sourceTransfer = TransferFunctions(edge.Source);
                var target = _analysisFilled[edge.Dest];
                if (!sourceTransfer.PartialOrder(target))
                {
                    _analysisFilled[edge.Dest] = target.Join(sourceTransfer);
                    var edgesToAdd = _flow.Where(x => x.Source == edge.Dest);
                    foreach (var e in edgesToAdd)
                    {
                        _workList.Insert(e);
                    }
                }
            }
            Console.WriteLine("number of operations in worklist " + numberOfOperations);

            
            var labels = FlowUtil.Labels(_blocks);
            foreach (var lab in labels)
            {
                var edges = _flow.Where(x => x.Dest == lab);
                _analysisCircle[lab] = TransferFunctions(lab);
            }
        }
        
        public override string ToString()
        {
            var circle = string.Join("\n", _analysisCircle.Select(x => x.ToString()));
            var filled = string.Join("\n", _analysisFilled.Select(x => x.ToString()));
            return $"circle: {circle} \n filled: {filled}";
        }

    }

    public enum AnalysisDirection
    {
        Forward,
        Backward,
    }
}