using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Paramdigma.Core.Collections;
using Paramdigma.Core.Geometry;
using Paramdigma.Core.Optimization;
using Xunit;
using Xunit.Abstractions;

namespace Paramdigma.Core.Tests.Optimization
{
    public class KMeansClusteringTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public KMeansClusteringTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        public List<VectorNd> createClusterAround(Point3d pt, double radius, int count)
        {
            var cluster = new List<VectorNd>();
            var rnd = new Random();
            var range = new Interval(-radius, radius);
            for (int i = 0; i < count; i++)
            {
                var x = pt.X + rnd.NextDouble();
                var y = pt.Y + rnd.NextDouble();
                var z = pt.Z + rnd.NextDouble();
                cluster.Add(new VectorNd(x, y, z));
            }

            return cluster;
        }

        [Theory]
        [InlineData(4, 20 )]
        [InlineData(6, 18)]
        [InlineData(10, 15)]
        public void KMeans_MainTest(int expectedClusters, int expectedClusterCount)
        {
            //Generate random vectors
            var vectors = new List<VectorNd>();
            var cir = new Circle(Plane.WorldXY, 10);
            var pts = new List<Point3d>();

            for (int i = 0; i < expectedClusters; i++)
            {
                var pt = cir.PointAt((double)i / expectedClusters);
                pts.Add(pt);
                vectors.AddRange(createClusterAround(pt, 1, expectedClusterCount));
            }
            
            Assert.True(vectors.Count == expectedClusters * expectedClusterCount);
            bool eventCheck = false;
            // When
            var kMeans = new KMeansClustering(100, expectedClusters, vectors);
            kMeans.IterationCompleted += (sender, args) =>
            {
                Assert.True(args.iteration >= 0);
                Assert.True(args.Clusters.Count == expectedClusters);
                eventCheck = true;
                
            };
            kMeans.Run();
            //Assert the Iteration completed event has been raised
            Assert.True(eventCheck);
            // Then
            kMeans.Clusters.ForEach(cluster =>
            {
                Assert.NotEmpty(cluster);
                var first = new Point3d(cluster[0][0], cluster[0][1], cluster[0][2]);
                var closest = pts.First(pt => pt.DistanceTo(first) <= 2);
                foreach (var vector in cluster)
                {
                    var pt = new Point3d(vector[0], vector[1], vector[2]);
                    var dist = pt.DistanceTo(closest);
                    //testOutputHelper.WriteLine($"Distance: {dist}");
                    Assert.True(dist <= 2, $"Distance was bigger: {dist}");
                }
            });
            
            
        }
    }
}