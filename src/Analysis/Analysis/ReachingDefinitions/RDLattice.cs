using System.Linq;
using Analysis.Analysis.AvailableExpressions;
using Analysis.AST;

namespace Analysis.Analysis.ReachingDefinitions
{
    public class RDLattice : ILattice<RDDomain>
    {
        public RDDomain Domain { get; set; }

        public RDLattice(RDDomain domain) => Domain = domain;

        public bool PartialOrder(ILattice<RDDomain> right) => Domain.IsSubsetOf(right.GetDomain());
        
        public ILattice<RDDomain> Join(ILattice<RDDomain> right) => new RDLattice(Domain.Union(right.GetDomain()).ToDomain());
        
        public RDDomain GetDomain() => Domain;

        public RDLattice Bottom() => new RDLattice();
        //public RDLattice Top(Program program) => new RDLattice(program);

        public RDLattice(Program program) => 
            Domain = AnalysisUtil.FreeVariables(program).Select(x => new RDDefinition(x.Id)).ToDomain();
        private RDLattice() => Domain = new RDDomain();
        
        public override string ToString() => $"{{ {string.Join(",", Domain.Select(x => x.ToString()))} }}";
    }
}