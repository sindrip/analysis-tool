using System.Linq;
using Analysis.Analysis.LiveVariables;

namespace Analysis.Analysis.FaintVariables
{
    public class FVLattice : ILattice<FVDomain>
    {
        public FVDomain Domain { get; set; }

        public FVLattice(FVDomain domain) => Domain = domain;
        public FVLattice() => Domain = new FVDomain();
        public bool PartialOrder(ILattice<FVDomain> right) => Domain.IsSubsetOf(right.GetDomain());

        public ILattice<FVDomain> Join(ILattice<FVDomain> right) 
            => new FVLattice(Domain.Union(right.GetDomain()).ToFVDomain());

        public FVDomain GetDomain() => Domain;
        
        public FVLattice Bottom() => new FVLattice();
        
        public override string ToString() => $"{string.Join(",", Domain.Select(i => i.ToString()))}";
    }
}