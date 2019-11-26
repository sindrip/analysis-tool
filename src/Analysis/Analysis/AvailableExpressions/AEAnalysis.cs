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
        private AELattice _dummyLattice;

        public AEAnalysis(Program program, string worklistName) : base(program, AnalysisDirection.Forward, worklistName)
        {
            _dummyLattice = new AELattice(program);
            
            InitializeAnalysis();
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
            var domain = _analysisCircle[label].GetDomain();
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