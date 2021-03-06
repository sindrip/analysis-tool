using System;
using System.Collections.Generic;
using System.Linq;
using Analysis.Analysis;
using Analysis.Analysis.AvailableExpressions;
using Analysis.Analysis.FaintVariables;
using Analysis.Analysis.LiveVariables;
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
        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list, int? seed = 0)
        {
            var r = new Random(seed ?? 0);
            var shuffledList =
                list.
                    Select(x => new { Number = r.Next(), Item = x }).
                    OrderBy(x => x.Number).
                    Select(x => x.Item).
                    Take(list.Count()); // Assume first @size items is fine

            return shuffledList.ToList();
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

        public static LVDomain ToDomain(this IEnumerable<Identifier> enumerable)
        {
            var lvDomain = new LVDomain();
            foreach (var lvIdentifier in enumerable)
            {
                lvDomain.Add(lvIdentifier);
            }

            return lvDomain;
        }

        public static FVDomain ToFVDomain(this IEnumerable<Identifier> enumerable)
        {
            var fvDomain = new FVDomain();
            foreach (var fvIdentifier in enumerable )
            {
                fvDomain.Add(fvIdentifier);
            }

            return fvDomain;
        }
    }
}