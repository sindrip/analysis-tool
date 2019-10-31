using System.Linq;
using Analysis.Analysis.AvailableExpressions;
using Analysis.AST;

namespace Analysis.Analysis.ReachingDefinitions
{
    public class RDLattice
    {
        public RDDomain Domain { get; set; }

        public RDLattice(RDDomain intersect) => Domain = new RDDomain();

        public bool PartialOrder(RDLattice right) => Domain.IsSupersetOf(right.Domain);
        
        public RDLattice Join(RDLattice right) => new RDLattice((RDDomain)Domain.Intersect(right.Domain));
        
        public RDLattice Bottom(Program program) => new RDLattice(program);
        public RDLattice Top(Program program) => new RDLattice();
        
        public RDLattice(Program program) => Domain = (RDDomain)AnalysisUtil.FreeVariables(program);
        private RDLattice() => Domain = new RDDomain();
        
        public override string ToString() => $"{{ {string.Join(",", Domain.Select(x => x.ToString()))} }}";
    }
}