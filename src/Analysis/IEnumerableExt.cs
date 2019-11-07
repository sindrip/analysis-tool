using System.Collections.Generic;
using System.Linq;
using Analysis.Analysis;
using Analysis.Analysis.AvailableExpressions;
using Analysis.Analysis.ReachingDefinitions;
using Analysis.AST;

namespace Analysis
{
    public static class IEnumerableExt
    {
        public static IEnumerable<T> Singleton<T>(this T element)
        {
            return Enumerable.Repeat<T>(element, 1);
        } 
    }

    public static class DomainExt
    {
        public static RDDomain ToDomain(this IEnumerable<RDDefinition> enumerable)
        {
            var rdDomain = new RDDomain();
            foreach (var rdDefinition in enumerable)
            {
                rdDomain.Add(rdDefinition);
            }

            return rdDomain;
        }

        public static AEDomain ToDomain(this IEnumerable<IExpression> enumerable)
        {
            var aeDomain = new AEDomain();
            foreach (var aeDefinition in enumerable)
            {
                aeDomain.Add(aeDefinition);
            }

            return aeDomain;
        }
    }
}