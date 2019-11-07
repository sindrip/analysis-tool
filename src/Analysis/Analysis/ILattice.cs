using System.Collections.Generic;
using System.Linq;
using Analysis.AST;

namespace Analysis.Analysis
{
    public interface ILattice<T>
    {
        //T Lattice { get; set; }
        //bool PartialOrder(ILattice<T> right);
        //ILattice<T> Join(ILattice<T> right);
    }
    
    // Available Expressions

    // Reaching Definitions
    public class RDLattice : ILattice<Dictionary<Identifier, HashSet<int?>>>
    {
        public Dictionary<Identifier, HashSet<int?>> Lattice { get; set; }

        public RDLattice(Dictionary<Identifier, HashSet<int?>> lattice) => Lattice = lattice;

        public RDLattice(IEnumerable<Identifier> vars)
        {
            var bottom = new Dictionary<Identifier, HashSet<int?>>();
            foreach (var x in vars)
            {
                bottom.Add(x, new HashSet<int?>());
            }

            Lattice = bottom;
        }

        public bool PartialOrder(RDLattice right)
        {
            return Lattice.All(left => left.Value.IsSubsetOf(right.Lattice[left.Key]));
        }

        public RDLattice Join(RDLattice right)
        {
            var joinedLattice = new Dictionary<Identifier, HashSet<int?>>();
            foreach (var x in Lattice)
            {
                joinedLattice[x.Key] = x.Value.Union(right.Lattice[x.Key]).ToHashSet();
            }
            return new RDLattice(joinedLattice);
        }

        public RDLattice Bottom(IEnumerable<Identifier> vars)
        {
            var bottom = new Dictionary<Identifier, HashSet<int?>>();
            foreach (var x in vars)
            {
                bottom.Add(x, new HashSet<int?>());
            } 
            return new RDLattice(bottom);
        }

        public override string ToString()
        {
            return string.Join("\n", Lattice.Select(x => $"{x.Key}: {string.Join(",", x.Value.Select(x => x.ToString()))}"));
        }
    }
    
    public class RDLabel
    {
        private readonly int? _value;

        RDLabel()
        {
            int? x;
        }

        RDLabel(int value) => _value = value;
        
        public bool IsDefined() => _value == null;
    }
    
}

