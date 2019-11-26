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
        protected List<ILattice<T>> _analysisCircle;
        protected List<ILattice<T>> _analysisFilled;
        protected Program _program;
        protected IWorkList _worklist;
        protected string _worklistMethodName;
        protected List<IterationStep> _iterationSteps;
        protected abstract ILattice<T> TransferFunctions(int label);
        protected abstract ILattice<T> Iota();
        protected abstract ILattice<T> Bottom();

        public Analysis(Program program, AnalysisDirection direction, string worklistName)
        {
            _program = program;
            _blocks = FlowUtil.Blocks(program);
            _worklistMethodName = worklistName;
            
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


            _analysisCircle = new List<ILattice<T>>();
            _analysisFilled = new List<ILattice<T>>();

            _iterationSteps = new List<IterationStep>();
        }
        
        public List<ILattice<T>> GetCircleLattice() => _analysisCircle; 
        public List<ILattice<T>> GetFilledLattice() => _analysisFilled; 
        protected IStatement GetBlock(int label) => _blocks.First(x => x.Label == label);
        protected IEnumerable<int> GetLabels() => _blocks.Select(b => b.Label);

        protected void InitializeAnalysis()
        {
            var orderedBlocks = _blocks.OrderBy(x => x.Label);
            foreach (var b in orderedBlocks)
            {
                if (_extremalLabels.Contains(b.Label))
                {
                    _analysisCircle.Add(Iota());
                }
                else
                {
                    _analysisCircle.Add(Bottom());
                }

                _analysisFilled.Add(Bottom());
            }
        }

        protected void RunAnalysis()
        {
            switch (_worklistMethodName)
            {
                case "FIFOWorklist":
                    _worklist = new FIFOWorklist(_flow);
                    break;
                case "LIFOWorklist":
                    _worklist = new LIFOWorklist(_flow);
                    break;
                case "ChaoticIteration":
                    _worklist = new ChaoticIteration(_flow);
                    break;
                case "RoundRobin":
                    DepthFirstSpanningTree dfst = new DepthFirstSpanningTree(new FlowGraph(_program));
                    _worklist = new RoundRobin(_flow, dfst.GetRP());
                    break;
                default:
                    break;
            }

            WorkThroughWorklist(_worklist);

            var labels = FlowUtil.Labels(_blocks);
            foreach (var lab in labels)
            {
                var edges = _flow.Where(x => x.Dest == lab);
                _analysisFilled[lab] = TransferFunctions(lab);
            }
        }

        private int WorkThroughWorklist(IWorkList _workList)
        {
            int numberOfOperations = 0;

            while (!_workList.Empty())
            {
                var edge = _workList.Extract();
                numberOfOperations++;

                var sourceTransfer = TransferFunctions(edge.Source);
                var target = _analysisCircle[edge.Dest];
                if (!sourceTransfer.PartialOrder(target))
                {
                    _analysisCircle[edge.Dest] = target.Join(sourceTransfer);
                    var edgesToAdd = _flow.Where(x => x.Source == edge.Dest);
                    foreach (var e in edgesToAdd)
                    {
                        _workList.Insert(e);
                    }
                }

                List<(int, string)> analysisCircleList = new List<(int, string)>();

                foreach(int lab in FlowUtil.Labels(_blocks))
                {
                    analysisCircleList.Add((lab, _analysisCircle[lab].ToString()));
                }

                _iterationSteps.Add(new IterationStep(numberOfOperations, edge.Source, _workList.GetCurrentEdges(), analysisCircleList));
            }

            return numberOfOperations;
        }

        public List<IterationStep> GetIterationSteps()
        {
            return _iterationSteps;
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