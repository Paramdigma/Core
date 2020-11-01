using System.Collections;
using System.Collections.Generic;
using Paramdigma.Core.Geometry;

namespace Paramdigma.Core.Optimization
{
    /// <summary>
    ///     Represents a vector cluster for the K-Means Clustering Algorithm.
    /// </summary>
    public class KMeansCluster : IList<VectorNd>
    {
        private readonly IList<VectorNd> list = new List<VectorNd>();

        /// <summary>
        ///     Gets or sets the vector at the given index.
        /// </summary>
        /// <param name="index">Index of the desired object.</param>
        public VectorNd this[int index]
        {
            get => this.list[index];
            set => this.list[index] = value;
        }

        /// <summary>
        ///     Gets the amount of clusters.
        /// </summary>
        public int Count => this.list.Count;

        /// <summary>
        ///     Gets a value indicating whether the cluster is readOnly.
        /// </summary>
        public bool IsReadOnly => this.list.IsReadOnly;


        /// <summary>
        ///     Add a new vector to the cluster.
        /// </summary>
        /// <param name="item">Vector to add.</param>
        public void Add(VectorNd item) => this.list.Add(item);


        /// <inheritdoc />
        public void Clear() => this.list.Clear();


        /// <inheritdoc />
        public bool Contains(VectorNd item) => this.list.Contains(item);


        /// <inheritdoc />
        public void CopyTo(VectorNd[] array, int arrayIndex) => this.list.CopyTo(array, arrayIndex);


        /// <inheritdoc />
        public IEnumerator<VectorNd> GetEnumerator() => this.list.GetEnumerator();


        /// <inheritdoc />
        public int IndexOf(VectorNd item) => this.list.IndexOf(item);


        /// <inheritdoc />
        public void Insert(int index, VectorNd item) => this.list.Insert(index, item);


        /// <inheritdoc />
        public bool Remove(VectorNd item) => this.list.Remove(item);


        /// <inheritdoc />
        public void RemoveAt(int index) => this.list.RemoveAt(index);


        IEnumerator IEnumerable.GetEnumerator() => this.list.GetEnumerator();


        /// <summary>
        ///     Computes the average of this cluster.
        /// </summary>
        /// <returns>Average vector of the current cluster.</returns>
        public VectorNd Average()
        {
            if (this.list.Count == 0)
                return new VectorNd(0);
            if (this.list.Count == 1)
                return this.list[0];

            var result = new VectorNd(this.list[0].Dimension);
            foreach (var vector in this.list)
                result += vector;

            result /= this.list.Count;
            return result;
        }


        /// <inheritdoc />
        public override string ToString() => "Paramdigma.Core.Cluster[" + this.Count + "]";
    }
}