using System.Collections.Generic;
using Analysis.AST;

namespace Analysis.Analysis.DetectionOfSigns
{
    public class DSDomain : Dictionary<Identifier, HashSet<DSSign>>
    {
    }
}