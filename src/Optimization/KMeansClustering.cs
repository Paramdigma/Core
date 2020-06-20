using System;
using System.Collections.Generic;
using System.Linq;
using Paramdigma.Core.Geometry;

namespace Paramdigma.Core.Optimization
{
    /// <summary>
    ///     Generic K-Means Clustering Algorithm for N dimensional vectors.
    /// </summary>
    public class KMeansClustering
    {
        private readonly int clusterCount;
        private readonly int maxIterations;
        private int currentIterations;

        /// <summary>
        ///     Initializes a new instance of the <see cref="KMeansClustering" /> class.
        /// </summary>
        /// <param name="maxIterations">Maximum iterations allowed.</param>
        /// <param name="clusterCount">Desired amount of clusters.</param>
        /// <param name="data">List of N dimensional vectors to cluster.</param>
        public KMeansClustering(int maxIterations, int clusterCount, List<VectorNd> data)
        {
            this.maxIterations = maxIterations;
            this.clusterCount = clusterCount;
            this.InitializeClusters(data);
        }

        /// <summary>
        ///     Gets or sets the list of clusters.
        /// </summary>
        public List<KMeansCluster> Clusters
        {
            get;
            set;
        }

        private void InitializeClusters(List<VectorNd> data)
        {
            this.Clusters = new List<KMeansCluster>(this.clusterCount);
            for (var i = 0; i < this.clusterCount; i++)
                this.Clusters.Add(new KMeansCluster());

            data.ForEach(vector => this.Clusters[new Random().Next() % this.clusterCount].Add(vector));
        }

        /// <summary>
        ///     Run the algorithm until it reaches the maximum amount of iterations.
        /// </summary>
        public void Run() => this.Run(this.maxIterations, false);

        /// <summary>
        ///     Run the k-means clustering algorithm for a specified amount of iterations.
        /// </summary>
        /// <param name="iterations">Iterations to run.</param>
        public void Run(int iterations, bool allowEmptyClusters)
        {
            var rnd = new Random();
            bool hasChanged;
            var iteration = 0;
            do
            {
                hasChanged = false;

                // Compute cluster averages
                var averages = new List<VectorNd>(this.Clusters.Count);
                this.Clusters.ForEach(cluster => averages.Add(cluster.Average()));

                // Create placeholder clusters for next iteration
                var newClusters = new List<KMeansCluster>(this.Clusters.Count);
                for (var i = 0; i < this.Clusters.Count; i++)
                    newClusters.Add(new KMeansCluster());

                // Find the closest average for each vector in each cluster
                this.Clusters.ForEach(cluster =>
                {
                    var ind = this.Clusters.IndexOf(cluster);
                    for (var i = 0; i < cluster.Count; i++)
                    {
                        var vector = cluster[i];
                        var simIndex = this.FindIndexOfSimilar(averages, vector);
                        newClusters[simIndex].Add(vector);
                        if (simIndex != ind)
                            hasChanged = true;
                    }
                });

                // Check for empty clusters
                if (!allowEmptyClusters)
                    newClusters.ForEach(cluster =>
                    {
                        if (cluster.Count == 0)
                        {
                            Console.WriteLine("Cluster has no mass");
                            var biggest = newClusters.OrderByDescending(x => x.Count)
                                                     .First();

                            var randomVector = biggest[rnd.Next(biggest.Count)];

                            biggest.Remove(randomVector);
                            cluster.Add(randomVector);
                        }
                    });

                // Update clusters and increase iteration
                this.Clusters = newClusters;
                var iterArgs = new IterationCompletedEventArgs {iteration = iteration, Clusters = newClusters};
                this.OnIterationCompleted(iterArgs);
                iteration++;
                this.currentIterations++;
            } while (hasChanged
                  && iteration < iterations
                  && this.currentIterations < this.maxIterations);
        }

        /// <summary>
        ///     Find the index of the most similar vector to a given one.
        /// </summary>
        /// <param name="pool">List of vectors to compare with.</param>
        /// <param name="vector">Reference vector.</param>
        /// <returns>Index of the most similar vector in the pool.</returns>
        public int FindIndexOfSimilar(List<VectorNd> pool, VectorNd vector)
        {
            var min = double.MaxValue;
            var minIndex = -1;

            for (var i = 0; i < pool.Count; i++)
            {
                var v = pool[i];
                var sim = VectorNd.CosineSimilarity(v, vector);
                if (sim < min)
                {
                    min = sim;
                    minIndex = i;
                }
            }

            return minIndex;
        }

        /// <summary>
        ///     Raised when an iteration is completed.
        /// </summary>
        public event EventHandler<IterationCompletedEventArgs> IterationCompleted;

        /// <summary>
        ///     Method to call when an iteration is completed.
        /// </summary>
        /// <param name="iterArgs">Data for the current iteration.</param>
        protected virtual void OnIterationCompleted(IterationCompletedEventArgs iterArgs) => this.IterationCompleted?.Invoke(this, iterArgs);

        /// <summary>
        ///     Data for the current iteration event.
        /// </summary>
        public class IterationCompletedEventArgs : EventArgs
        {
            public int iteration
            {
                get;
                set;
            }

            public List<KMeansCluster> Clusters
            {
                get;
                set;
            }
        }
    }
}