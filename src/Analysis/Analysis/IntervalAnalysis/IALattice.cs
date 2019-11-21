using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using Analysis.AST;

namespace Analysis.Analysis.IntervalAnalysis
{
    public class IALattice : ILattice<IADomain>
    {
        public IADomain Domain { get; set; }

        public IALattice(IADomain domain) => Domain = domain;

        public static IALattice Bottom(Program program)
        {
            var domain = new IADomain();
            var idents = AnalysisUtil.FreeVariables(program);
            foreach (var ident in idents)
            {
                domain.Add(ident, Interval.Bottom());
            }
            return new IALattice(domain);
        }

        public static IALattice Top(Program program)
        {
            var domain = new IADomain();
            var idents = AnalysisUtil.FreeVariables(program);
            foreach (var ident in idents)
            {
                domain.Add(ident, Interval.Top());
            }
            return new IALattice(domain);
        }
        
        public bool PartialOrder(ILattice<IADomain> right) => 
            Domain.All(p => p.Value <= right.GetDomain()[p.Key]);

        public ILattice<IADomain> Join(ILattice<IADomain> right)
        {
            var rightDomain = right.GetDomain();
            var newDomain = new IADomain();
            foreach (var pair in Domain)
            {
                var newValue = pair.Value.Join(rightDomain[pair.Key]);
                newDomain.Add(pair.Key, newValue);
            }
            return new IALattice(newDomain);
        }

        public ILattice<IADomain> Meet(ILattice<IADomain> right)
        {
            var rightDomain = right.GetDomain();
            var newDomain = new IADomain();
            
            return new IALattice(newDomain);
        }
        
        public IADomain GetDomain() => Domain;

        public bool IsBottom() => Domain.All(x => x.Value.IsBottom);

        public override string ToString()
        {
            var ret = new StringBuilder();
            foreach (var pair in Domain)
            {
                ret.Append($"{pair.Key}: {pair.Value}");
            }

            return ret.ToString();
        }
    }
}