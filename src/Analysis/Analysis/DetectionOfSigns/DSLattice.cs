namespace Analysis.Analysis.DetectionOfSigns
{
    public class DSLattice : ILattice<DSDomain>
    {
        public DSDomain Domain { get; set; }

        public bool PartialOrder(ILattice<DSDomain> right)
        {
            throw new System.NotImplementedException();
        }

        public ILattice<DSDomain> Join(ILattice<DSDomain> right)
        {
            throw new System.NotImplementedException();
        }

        public DSDomain GetDomain()
        {
            throw new System.NotImplementedException();
        }
    }
}