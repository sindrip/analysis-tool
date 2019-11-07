using System.Linq;
using Analysis.AST;

namespace Analysis.Analysis.AvailableExpressions
{
    public class AELattice : ILattice<AEDomain>
    {
        public AEDomain Domain { get; set; }
        public AEDomain GetDomain() => Domain;

        // TODO: what?
        public AELattice(AEDomain intersect) => Domain = new AEDomain();

        public bool PartialOrder(ILattice<AEDomain> right) => Domain.IsSupersetOf(right.GetDomain());
        
        public ILattice<AEDomain> Join(ILattice<AEDomain> right) => new AELattice(Domain.Intersect(right.GetDomain()).ToDomain());
        
        public AELattice Bottom(Program program) => new AELattice(program);
        public AELattice Top(Program program) => new AELattice();
        
        public AELattice(Program program) => Domain = (AEDomain)AnalysisUtil.AvailableExpressions(program);
        public AELattice() => Domain = new AEDomain();
        
        public override string ToString() => $"{{ {string.Join(",", Domain.Select(x => x.ToString()))} }}";
    }
}