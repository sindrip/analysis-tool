using System.Collections.Generic;

namespace Analysis.Analysis
{
    public interface ILattice<T>
    {
        bool PartialOrder(ILattice<T> right);
        ILattice<T> Join(ILattice<T> right);

        T GetDomain();
    }
}