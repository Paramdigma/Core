using System.Collections.Generic;

namespace Paramdigma.Core.SpatialSearch
{
    public interface ISpatialTree<T, Q>
    {
        T Boundary
        {
            get;
        }

        List<Q> Points
        {
            get;
        }

        bool ThresholdReached
        {
            get;
        }

        bool Insert(Q value);

        IEnumerable<Q> QueryRange(T range);
    }
}