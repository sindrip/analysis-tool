using System.Linq;
using Analysis.Analysis.AvailableExpressions;
using Analysis.AST;

namespace Analysis.Analysis.ReachingDefinitions
{
    public class RDLattice
    {
        public RDDomain Domain { get; set; }

        public RDLattice(RDDomain domain) => Domain = domain;

        public bool PartialOrder(RDLattice right) => Domain.IsSubsetOf(right.Domain);
        
        public RDLattice Join(RDLattice right) => new RDLattice(Domain.Union(right.Domain).ToDomain());
        
        public RDLattice Bottom() => new RDLattice();
        //public RDLattice Top(Program program) => new RDLattice(program);

        public RDLattice(Program program) => 
            Domain = AnalysisUtil.FreeVariables(program).Select(x => new RDDefinition(x.Id)).ToDomain();
        private RDLattice() => Domain = new RDDomain();
        
        public override string ToString() => $"{{ {string.Join(",", Domain.Select(x => x.ToString()))} }}";
    }
}