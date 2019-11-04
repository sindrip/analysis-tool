using System.Linq;
using Analysis.AST;

namespace Analysis.Analysis.AvailableExpressions
{
    public class AELattice
    {
        public AEDomain Domain { get; set; }

        // TODO: what?
        public AELattice(AEDomain intersect) => Domain = new AEDomain();

        public bool PartialOrder(AELattice right) => Domain.IsSupersetOf(right.Domain);
        
        public AELattice Join(AELattice right) => new AELattice((AEDomain)Domain.Intersect(right.Domain));
        
        public AELattice Bottom(Program program) => new AELattice(program);
        public AELattice Top(Program program) => new AELattice();
        
        public AELattice(Program program) => Domain = (AEDomain)AnalysisUtil.AvailableExpressions(program);
        private AELattice() => Domain = new AEDomain();
        
        public override string ToString() => $"{{ {string.Join(",", Domain.Select(x => x.ToString()))} }}";
    }
}