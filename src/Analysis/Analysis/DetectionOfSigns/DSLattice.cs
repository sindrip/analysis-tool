using System.Collections.Generic;
using System.Linq;
using System.Text;
using Analysis.AST;

namespace Analysis.Analysis.DetectionOfSigns
{
    public class DSLattice : ILattice<DSDomain>
    {
        public DSDomain Domain { get; set; }

        public DSLattice() => Domain = new DSDomain();
        public DSLattice(DSDomain domain) => Domain = domain;

        public bool PartialOrder(ILattice<DSDomain> right) => 
            Domain.All(p => p.Value.IsSubsetOf(right.GetDomain()[p.Key]));

        public ILattice<DSDomain> Join(ILattice<DSDomain> right)
        {
            var rightDomain = right.GetDomain();
            var newDomain = new DSDomain();
            foreach (var pair in Domain)
            {
                var newValue = pair.Value.Union(rightDomain[pair.Key]).ToHashSet();
                newDomain.Add(pair.Key, newValue);
            }
            return new DSLattice(newDomain);
        }

        public static DSLattice Bottom(Program program)
        {
            var domain =  new DSDomain();
            var idents = AnalysisUtil.FreeVariables(program);
            foreach (var ident in idents)
            {
                domain.Add(ident, new HashSet<DSSign>());
            }
            return new DSLattice(domain);
        }

        public static DSLattice Top(Program program)
        {
            var domain = new DSDomain();
            var idents = AnalysisUtil.FreeVariables(program);
            foreach (var ident in idents)
            {
                domain.Add(ident, new HashSet<DSSign>() { DSSign.Negative, DSSign.Positive, DSSign.Zero});
            }
            return new DSLattice(domain);
        }

        public DSDomain GetDomain() => Domain;
        
        public override string ToString()
        {
            var ret = new StringBuilder();
            foreach (var pair in Domain)
            {
                var signs = string.Join(",", pair.Value.Select(s => SignToString(s)));
                ret.Append($"{pair.Key}: {{{signs}}}");
            }

            return ret.ToString();
        }

        private string SignToString(DSSign sign) => sign switch
        {
            DSSign.Negative => "-",
            DSSign.Positive => "+",
            DSSign.Zero => "0",
        };
    }
}