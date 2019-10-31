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
                    break;
                }
                case ArrayDecl arrayDecl:
                {
                    return hs;
                    break;
                }
                case RecordDecl recordDecl:
                {
                    return hs;
                    break;
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
                    break;
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