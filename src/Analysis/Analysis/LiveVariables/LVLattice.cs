using System.Linq;

namespace Analysis.Analysis.LiveVariables
{
    public class LVLattice : ILattice<LVDomain>
    {
        public LVDomain Domain { get; set; }

        public LVLattice(LVDomain domain) => Domain = domain;
        public LVLattice() => Domain = new LVDomain();
            
        public bool PartialOrder(ILattice<LVDomain> right) => Domain.IsSubsetOf(right.GetDomain());

        public ILattice<LVDomain> Join(ILattice<LVDomain> right) =>
            new LVLattice(Domain.Union(right.GetDomain()).ToDomain());

        public LVDomain GetDomain() => Domain;
        
        public  LVLattice Bottom() => new LVLattice();

        public override string ToString() => $"{string.Join(",", Domain.Select(i => i.ToString()))}";
    }
}