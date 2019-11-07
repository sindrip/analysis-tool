using System;
using System.Collections.Generic;
using System.Linq;
using Analysis.AST;
using Analysis.AST.Statement;
using Analysis.CFG;

namespace Analysis.Analysis.AvailableExpressions
{
    public class AEAnalysis : Analysis<AEDomain>
    {
        private IEnumerable<int> _extremalLabels;
        private IEnumerable<FlowEdge> _flow;
        private IEnumerable<IStatement> _blocks;
        private IWorkList _workList;
        private List<AELattice> _analysisFilled;
        private List<AELattice> _analysisCircle;
        private Program _program;
        private AELattice _dummyLattice;

        public AEAnalysis(Program program) : base(program, AnalysisDirection.Forward)
        {
            _dummyLattice = new AELattice(program);
            
            InitalizeAnalysis();
            RunAnalysis();
        }

        //private ILattice<AEDomain> Iota() => _dummyLattice.Top(_program);
        protected override ILattice<AEDomain> Iota() => new AELattice();

        protected override ILattice<AEDomain> Bottom() => _dummyLattice.Bottom(_program);

        protected override ILattice<AEDomain> TransferFunctions(int label)
        {
            var block = GetBlock(label);
            var kill = Kill(block);
            var gen = Gen(block);
            var domain = _analysisFilled[label].Domain;
            var newDomain = domain.Except(kill).Union(gen).ToDomain();
            return new AELattice(newDomain);
        }
        
        public AEDomain Kill(IStatement block) => block switch
        {
            AssignStmt assignStmt => (AEDomain)AnalysisUtil.AvailableExpressions(_program)
                .Where(x => AnalysisUtil.FreeVariables(x).Contains(assignStmt.Left.Left)),
            IStatement iStmt => Iota().GetDomain(),
            _ => throw new ArgumentException("bla")
        };

        public AEDomain Gen(IStatement block) => block switch
        {
            AssignStmt assignStmt => (AEDomain)AnalysisUtil.AvailableExpressions(_program)
                .Where(x => !AnalysisUtil.FreeVariables(x).Contains(assignStmt.Left.Left)),
            IfStmt ifStmt => (AEDomain)AnalysisUtil.AvailableExpressions(ifStmt.Condition),
            IfElseStmt ifElseStmt => (AEDomain)AnalysisUtil.AvailableExpressions(ifElseStmt.Condition),
            WhileStmt whileStmt => (AEDomain)AnalysisUtil.AvailableExpressions(whileStmt.Condition),
            IStatement iStmt => Iota().GetDomain(),
            _ => throw new ArgumentException("bla")
        };
    }
}