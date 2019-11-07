using System;
using System.Collections.Generic;
using System.Linq;
using Analysis.Analysis.AvailableExpressions;
using Analysis.AST;
using Analysis.AST.AExpr;
using Analysis.AST.Statement;
using Analysis.CFG;

namespace Analysis.Analysis.ReachingDefinitions
{
    public class RDAnalysis
    {
        private IEnumerable<int> _extremalLabels;
        private IEnumerable<FlowEdge> _flow;
        private IEnumerable<IStatement> _blocks;
        private IWorkList _workList;
        private List<RDLattice> _analysisFilled;
        private List<RDLattice> _analysisCircle;
        private Program _program;
        private RDLattice _dummyLattice;

        public RDAnalysis(Program program)
        {
            _program = program;
            _flow = FlowUtil.Flow(program); // Forward
            _extremalLabels = FlowUtil.Init(program).Singleton();
            _blocks = FlowUtil.Blocks(program);
            _analysisFilled = new List<RDLattice>();
            _analysisCircle = new List<RDLattice>();
            _dummyLattice = new RDLattice(program);
            
            InitalizeAnalysis();
            RunAnalysis();
        }
        
        private IStatement GetBlock(int label) => _blocks.First(x => x.Label == label);
        private IEnumerable<int> GetLabels() => _blocks.Select(b => b.Label);
        private RDLattice Iota() => new RDLattice(_program);
        private RDLattice Bottom() => _dummyLattice.Bottom();
        
        private RDLattice LeastUpperBound(int label)
        {
            var edges = _flow.Where(x => x.Dest == label);
            return edges.Aggregate(Bottom(), (prod, e) => prod.Join(TransferFunctions(e.Source)));
        }
        
        private RDLattice TransferFunctions(int label)
        {
            var block = GetBlock(label);
            var kill = Kill(block);
            var gen = Gen(block);
            var domain = _analysisFilled[label].Domain;
            var newDomain = domain.Except(kill).Union(gen).ToDomain();
            return new RDLattice(newDomain);
        }

        public RDDomain Kill(IStatement block) => block switch
        {
            // TODO: RecordDecl and AssignRecord (Needs work in parser to get ID's)
            IntDecl intDecl => GetLabels().Select(l => new RDDefinition(intDecl.Id, l)).Append(new RDDefinition(intDecl.Id)).ToDomain(),
            ArrayDecl arrayDecl => GetLabels().Select(l => new RDDefinition(arrayDecl.Id, l)).Append(new RDDefinition(arrayDecl.Id)).ToDomain(),
            // RecordDecl
            AssignStmt assignStmt => GetLabels().Select(l => new RDDefinition(assignStmt.Left.Left.Id, l))
                .Append(new RDDefinition(assignStmt.Left.Left.Id)).ToDomain(),
            // RecAssignStmt 
            _ => new RDDomain(),
        };

        public RDDomain Gen(IStatement block) => block switch
        {
            IntDecl intDecl => new RDDefinition(intDecl.Id, intDecl.Label).Singleton().ToDomain(),
            ArrayDecl arrayDecl => new RDDefinition(arrayDecl.Id, arrayDecl.Label).Singleton().ToDomain(),
            // TODO: RecordDecl and AssignRecord (Needs work in parser to get ID's)
            //RecordDecl arrayDecl => (RDDomain)(new RDDefinition(arrayDecl.Id, arrayDecl.Label)).Singleton(),
            AssignStmt assignStmt => new RDDefinition(assignStmt.Left.Left.Id, assignStmt.Label).Singleton().ToDomain(),
            _ => new RDDomain(),
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
            
            var labels = FlowUtil.Labels(_blocks);
            foreach (var lab in labels)
            {
                var edges = _flow.Where(x => x.Dest == lab);
                //_analysisCircle[lab] = edges.Aggregate(Bottom(), (prod, e) => prod.Join(TransferFunctions(e.Source)));
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
}