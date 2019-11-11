using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Analysis.AST;
using Analysis.AST.AExpr;
using Analysis.AST.Statement;
using Analysis.CFG;

namespace Analysis.Analysis
{
    public interface IAnalysis<L>
    {
        IEnumerable<int> ExtremalLabels { get; set; }
        IEnumerable<FlowEdge> Flow { get; set; }
        L Iota();
        //void TransferFunctions(int label);
    }

    public class AEAnalysis : IAnalysis<AELattice>
    {
        public IEnumerable<int> ExtremalLabels { get; set; }
        public IEnumerable<FlowEdge> Flow { get; set; }
        private IEnumerable<IStatement> _blocks { get; set; }
        private IWorkList _workListChaotic { get; set; }
        private IWorkList _workListFIFO { get; set; }
        private IWorkList _workListLIFO { get; set; }

        private IWorkList _workListRoundRobin { get; set; }
        private List<AELattice> _analysisFilled { get; set; }
        private List<AELattice> _analysisCircle { get; set; }
        private Program _program { get; set; }

        public AEAnalysis(Program program)
        {
            _program = program;
            Flow = FlowUtil.Flow(program);
            ExtremalLabels = FlowUtil.Init(program).Singleton();
            _blocks = FlowUtil.Blocks(program);
            _analysisFilled = new List<AELattice>();
            _analysisCircle = new List<AELattice>();
            
            var orderedBlocks = _blocks.OrderBy(x => x.Label);
            foreach (var b in orderedBlocks)
            {
                if (ExtremalLabels.Contains(b.Label))
                {
                    _analysisFilled.Add(Iota());
                }
                else
                {
                    _analysisFilled.Add(AELattice.Bottom(program));
                }

                _analysisCircle.Add(AELattice.Bottom(program));
            }

            _workListChaotic = new ChaoticIteration(Flow.Shuffle(1));
            _workListFIFO = new FIFOWorklist(Flow.Shuffle(3));
            _workListLIFO = new LIFOWorklist(Flow.Shuffle());

            worklistAlgorithm(_workListChaotic);
            worklistAlgorithm(_workListFIFO);
            worklistAlgorithm(_workListLIFO);

            // testing Depth First Spanning Tree
            DepthFirstSpanningTree dfst = new DepthFirstSpanningTree(new FlowGraph(_program));

            _workListRoundRobin = new RoundRobin(Flow, dfst.GetRP());

            worklistAlgorithm(_workListRoundRobin);

            var labels = FlowUtil.Labels(_blocks);
            foreach (var lab in labels)
            {
                var edges = Flow.Where(x => x.Dest == lab);
                _analysisCircle[lab] = edges.Aggregate(AELattice.Bottom(program),
                    (prod, e) => prod.Join(TransferFunctions(e.Source)));
            }
        }

        public AELattice TransferFunctions(int label)
        {
            var block = getBlock(label);
            // (AEExit(l) \ Kill_AE(B^l)) union Gen_AE(B^l)
            var kill = Kill(block);
            var gen = Gen(block);
            var lattice = _analysisFilled[label].Lattice;
            var nl = lattice.Except(kill).Union(gen).ToHashSet();
            return new AELattice(nl);
        }

        public HashSet<IExpression> Kill(IStatement block) => block switch
        {
            AssignStmt assignStmt => AnalysisUtil.AvailableExpressions(_program)
                .Where(x => AnalysisUtil.FreeVariables(x).Contains(assignStmt.Left.Left))
                .ToHashSet(),
            IStatement iStmt => new HashSet<IExpression>(),
            _ => throw new ArgumentException("bla")
        };

        public HashSet<IExpression> Gen(IStatement block) => block switch
        {
            AssignStmt assignStmt => AnalysisUtil.AvailableExpressions(_program)
                .Where(x => !AnalysisUtil.FreeVariables(x).Contains(assignStmt.Left.Left))
                .ToHashSet(),
            IfStmt ifStmt => AnalysisUtil.AvailableExpressions(ifStmt.Condition),
            IfElseStmt ifElseStmt => AnalysisUtil.AvailableExpressions(ifElseStmt.Condition),
            WhileStmt whileStmt => AnalysisUtil.AvailableExpressions(whileStmt.Condition),
            IStatement iStmt => new HashSet<IExpression>(),
            _ => throw new ArgumentException("bla")
        };

        public AELattice Iota() => AELattice.Top();
        private IStatement getBlock(int label) => _blocks.First(x => x.Label == label);

        private void worklistAlgorithm(IWorkList workListToWorkThrough)
        {
            int numberOfOpertations = 0;
            while (!workListToWorkThrough.Empty())
            {
                var edge = workListToWorkThrough.Extract();
                numberOfOpertations++;
                var sourceTransfer = TransferFunctions(edge.Source);
                var target = _analysisFilled[edge.Dest];
                if (!sourceTransfer.PartialOrder(target))
                {
                    _analysisFilled[edge.Dest] = target.Join(sourceTransfer);
                    var edgesToAdd = Flow.Where(x => x.Source == edge.Dest);
                    foreach (var e in edgesToAdd)
                    {
                        workListToWorkThrough.Insert(e);
                        numberOfOpertations++;
                    }
                }
            }
            Console.WriteLine("Worklist operations: " + numberOfOpertations);
        }

        public override string ToString()
        {
            var circle = string.Join("\n", _analysisCircle.Select(x => x.ToString()));
            var filled = string.Join("\n", _analysisFilled.Select(x => x.ToString()));
            return $"circle: {circle} \n filled: {filled}";
        }
}
    
    public class RDAnalysis : IAnalysis<RDLattice>
    {
        public IEnumerable<int> ExtremalLabels { get; set; }
        public IEnumerable<FlowEdge> Flow { get; set; }
        private IEnumerable<Identifier> _variables { get; set; }
        private List<RDLattice> _analysisCircle { get; set; }
        private IEnumerable<IStatement> _blocks { get; set; }
        private IWorkList _workList { get; set; }

        public RDAnalysis(Program program)
        {
            Flow = FlowUtil.Flow(program);
            ExtremalLabels = FlowUtil.Init(program).Singleton();
            _variables = AnalysisUtil.FreeVariables(program);
            _blocks = FlowUtil.Blocks(program);
            
            var orderedBlocks = _blocks.OrderBy(x => x.Label);
            foreach (var b in orderedBlocks)
            {
                if (ExtremalLabels.Contains(b.Label))
                {
                    _analysisCircle.Add(Iota());
                }
                else
                {
                    // The dumb way of creating bottom, but it do
                    _analysisCircle.Add(new RDLattice(_variables));
                }
            }

            _workList = new ChaoticIteration(Flow);

            while (!_workList.Empty())
            {
                var edge = _workList.Extract();
                var sourceTransfer = TransferFunctions(edge.Source);
                var target = _analysisCircle[edge.Dest];
                if (!sourceTransfer.PartialOrder(target))
                {
                    _analysisCircle[edge.Dest] = target.Join(sourceTransfer);
                    var edgesToAdd = Flow.Where(x => x.Source == edge.Dest);
                    foreach (var e in edgesToAdd)
                    {
                        _workList.Insert(e);
                    }
                }
                
            }
        }

        public RDLattice Iota()
        {
            var iota = new Dictionary<Identifier, HashSet<int?>>();
            foreach (var v in _variables)
            {
                var hs = new HashSet<int?>();
                hs.Add(null);
               iota.Add(v, hs); 
            }
            return new RDLattice(iota);
        }
        
        public RDLattice TransferFunctions(int label)
        {
            var block = getBlock(label);
            // (LVExit(l) \ Kill_LV(B^l)) union gen_LV(B^l)
            var kill = Kill(block);
            var gen = Gen(block);
            var lattice = _analysisCircle[label].Lattice;
            var newLattice = new Dictionary<Identifier, HashSet<int>>();
            foreach (var x in lattice)
            {
                //newla
            } 
            //var nl = lattice.Except(kill).Union(gen);
            //return newLattice;
            throw new System.NotImplementedException();
        }

        private HashSet<int?> Kill(IStatement block)
        {
            var labels = _blocks.Select(x => x.Label);
            var hs = new HashSet<int?>();
            hs.Add(null);
            foreach (var lab in labels)
            {
                hs.Add(lab);
            }
            switch (block)
            {
                case IntDecl intDecl:
                {
                    return hs;
                }
                case ArrayDecl arrayDecl:
                {
                    return hs;
                }
                case RecordDecl recordDecl:
                {
                    return hs;
                }
                case AssignStmt assignStmt:
                {
                    if (assignStmt.Left is ArrayAccess)
                    {
                        return new HashSet<int?>();
                    }
                    else
                    {
                        return hs;
                    }
                }
                case RecAssignStmt recAssignStmt:
                {
                    break;
                }
            }
            return new HashSet<int?>();
        }

        private HashSet<int?> Gen(IStatement block)
        {
           throw new NotImplementedException(); 
        }

        private IStatement getBlock(int label) => _blocks.First(x => x.Label == label);
    }


}