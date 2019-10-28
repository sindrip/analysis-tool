using System.Collections.Generic;
using System.Linq;
using Analysis.AST;

namespace Analysis.Analysis
{
    public interface ILattice<T>
    {
        T Lattice { get; set; }
        bool PartialOrder(ILattice<T> right);
        ILattice<T> Join(ILattice<T> right);
    }

    public class RDLattice : ILattice<Dictionary<Identifier, HashSet<int>>>
    {
        public Dictionary<Identifier, HashSet<int>> Lattice { get; set; }

        public RDLattice(Dictionary<Identifier, HashSet<int>> lattice) => Lattice = lattice;

        public bool PartialOrder(ILattice<Dictionary<Identifier, HashSet<int>>> right)
        {
            return Lattice.All(left => left.Value.IsSubsetOf(right.Lattice[left.Key]));
        }

        public ILattice<Dictionary<Identifier, HashSet<int>>> Join(ILattice<Dictionary<Identifier, HashSet<int>>> right)
        {
            var joinedLattice = new Dictionary<Identifier, HashSet<int>>();
            foreach (var x in Lattice)
            {
                joinedLattice[x.Key] = x.Value.Union(right.Lattice[x.Key]).ToHashSet();
            }
            return new RDLattice(joinedLattice);
        }

        public override string ToString()
        {
            return string.Join("\n", Lattice.Select(x => $"{x.Key}: {string.Join(",", x.Value.Select(x => x.ToString()))}"));
        }
    }
}

