using System;
using System.Collections.Generic;
using System.Linq;
using Analysis.AST;
using Analysis.AST.Statement;
using Analysis.CFG;

namespace Analysis.Analysis.AvailableExpressions
{
    public class AEAnalysis
    {
        private IEnumerable<int> _extremalLabels;
        private IEnumerable<FlowEdge> _flow;
        private IEnumerable<IStatement> _blocks;
        private IWorkList _workList;
        private List<AELattice> _analysisFilled;
        private List<AELattice> _analysisCircle;
        private Program _program;
        private AELattice _dummyLattice;

        public AEAnalysis(Program program)
        {
            _program = program;
            _flow = FlowUtil.Flow(program); // Forward
            _extremalLabels = FlowUtil.Init(program).Singleton();
            _blocks = FlowUtil.Blocks(program);
            _analysisFilled = new List<AELattice>();
            _analysisCircle = new List<AELattice>();
            _dummyLattice = new AELattice(program);
            
            InitalizeAnalysis();
            RunAnalysis();
        }

        private IStatement GetBlock(int label) => _blocks.First(x => x.Label == label);
        private AELattice Iota() => _dummyLattice.Top(_program);

        private AELattice Bottom() => _dummyLattice.Bottom(_program);

        private AELattice LeastUpperBound(int label)
        {
            var edges = _flow.Where(x => x.Dest == label);
            return edges.Aggregate(Bottom(), (prod, e) => prod.Join(TransferFunctions(e.Source)));
        }

        private AELattice TransferFunctions(int label)
        {
            var block = GetBlock(label);
            var kill = Kill(block);
            var gen = Gen(block);
            var domain = _analysisFilled[label].Domain;
            var newDomain = (AEDomain)domain.Except(kill).Union(gen);
            return new AELattice(newDomain);
        }
        
        public AEDomain Kill(IStatement block) => block switch
        {
            AssignStmt assignStmt => (AEDomain)AnalysisUtil.AvailableExpressions(_program)
                .Where(x => AnalysisUtil.FreeVariables(x).Contains(assignStmt.Left.Left)),
            IStatement iStmt => Iota().Domain,
            _ => throw new ArgumentException("bla")
        };

        public AEDomain Gen(IStatement block) => block switch
        {
            AssignStmt assignStmt => (AEDomain)AnalysisUtil.AvailableExpressions(_program)
                .Where(x => !AnalysisUtil.FreeVariables(x).Contains(assignStmt.Left.Left)),
            IfStmt ifStmt => (AEDomain)AnalysisUtil.AvailableExpressions(ifStmt.Condition),
            IfElseStmt ifElseStmt => (AEDomain)AnalysisUtil.AvailableExpressions(ifElseStmt.Condition),
            WhileStmt whileStmt => (AEDomain)AnalysisUtil.AvailableExpressions(whileStmt.Condition),
            IStatement iStmt => Iota().Domain,
            _ => throw new ArgumentException("bla")
        };

        private void InitalizeAnalysis()
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

        private void RunAnalysis()
        {
            
            _workList = new ChaoticIteration(_flow);

            while (!_workList.Empty())
            {
                var edge = _workList.Extract();
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
        }
        
        public override string ToString()
        {
            var circle = string.Join("\n", _analysisCircle.Select(x => x.ToString()));
            var filled = string.Join("\n", _analysisFilled.Select(x => x.ToString()));
            return $"circle: {circle} \n filled: {filled}";
        }
    }
}