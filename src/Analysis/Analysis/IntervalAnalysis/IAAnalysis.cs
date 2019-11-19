using Analysis.AST;

namespace Analysis.Analysis.IntervalAnalysis
{
    public class IAAnalysis : Analysis<IADomain>
    {
        public IAAnalysis(Program program) : base(program, AnalysisDirection.Forward)
        {
            InitializeAnalysis();
            RunAnalysis();
        }

        protected override ILattice<IADomain> TransferFunctions(int label)
        {
            throw new System.NotImplementedException();
        }

        protected override ILattice<IADomain> Iota()
        {
            throw new System.NotImplementedException();
        }

        protected override ILattice<IADomain> Bottom()
        {
            throw new System.NotImplementedException();
        }
    }
}