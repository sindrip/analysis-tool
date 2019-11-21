using System.Collections.Generic;
using System.Linq;
using Analysis.AST;

namespace Analysis.Analysis.IntervalAnalysis
{
    public class IADomain : Dictionary<Identifier, Interval>
    {
        public bool IsBottom() => this.All(x => x.Value.IsBottom);
    }
}