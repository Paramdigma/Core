using System.Collections;
using System.Collections.Generic;
using Paramdigma.Core.Geometry;

namespace Paramdigma.Core.Optimization
{
    /// <summary>
    /// Represents a vector cluster for the K-Means Clustering Algorithm.
    /// </summary>
    public class KMeansCluster : IList<VectorNd>
    {
        private readonly IList<VectorNd> list = new List<VectorNd>();

        /// <summary>
        /// Gets or sets the vector at the given index.
        /// </summary>
        /// <param name="index">Index of the desired object.</param>
        public VectorNd this[int index] { get => list[index]; set => list[index] = value; }

        /// <summary>
        /// Gets the amount of clusters.
        /// </summary>
        public int Count => list.Count;

        /// <summary>
        /// Gets a value indicating whether the cluster is readOnly.
        /// </summary>
        public bool IsReadOnly => list.IsReadOnly;

        /// <summary>
        /// Computes the average of this cluster.
        /// </summary>
        /// <returns>Average vector of the current cluster.</returns>
        public VectorNd Average()
        {
            if (list.Count == 0)
                return new VectorNd(0);
            else if (list.Count == 1)
                return list[0];

            var result = new VectorNd(list[0].Dimension);
            foreach (var vector in list)
            {
                result += vector;
            }

            result /= list.Count;
            return result;
        }

        /// <summary>
        /// Add a new vector to the cluster.
        /// </summary>
        /// <param name="item">Vector to add.</param>
        public void Add(VectorNd item)
        {
            list.Add(item);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            list.Clear();
        }

        /// <inheritdoc/>
        public bool Contains(VectorNd item)
        {
            return list.Contains(item);
        }

        /// <inheritdoc/>
        public void CopyTo(VectorNd[] array, int arrayIndex)
        {
            list.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc/>
        public IEnumerator<VectorNd> GetEnumerator()
        {
            return list.GetEnumerator();
        }

        /// <inheritdoc/>
        public int IndexOf(VectorNd item)
        {
            return list.IndexOf(item);
        }

        /// <inheritdoc/>
        public void Insert(int index, VectorNd item)
        {
            list.Insert(index, item);
        }

        /// <inheritdoc/>
        public bool Remove(VectorNd item)
        {
            return list.Remove(item);
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            list.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return list.GetEnumerator();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return "Paramdigma.Core.Cluster[" + this.Count + "]";
        }
    }
}
