using System;
using System.Collections.Generic;
using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    public class DelaunayTests
    {
        private List<DelaunayPoint> GeneratePoints(int amout, double maxX, double maxY, out List<DelaunayTriangle> border)
        {
            var point0 = new DelaunayPoint(0, 0);
            var point1 = new DelaunayPoint(0, maxY);
            var point2 = new DelaunayPoint(maxX, maxY);
            var point3 = new DelaunayPoint(maxX, 0);
            var points = new List<DelaunayPoint> {point0, point1, point2, point3};
            var triangle1 = new DelaunayTriangle(point0, point1, point2);
            var triangle2 = new DelaunayTriangle(point0, point2, point3);
            border = new List<DelaunayTriangle> {triangle1, triangle2};
            var rnd = new Random();
            var points2 = new List<DelaunayPoint>();
            for (var i = 0; i < amout - 4; i++)
                points2.Add(RandomPoint(rnd, 0, maxX));
            return points2;
        }


        public static DelaunayPoint RandomPoint(Random RandGenerator, double MinValue, double MaxValue)
        {
            var range = MaxValue - MinValue;
            var randomPoint = new DelaunayPoint((RandGenerator.NextDouble() * range) + MinValue, (RandGenerator.NextDouble() * range) + MinValue);
            return randomPoint;
        }

        [Fact]
        public void CanComputeDelaunay()
        {
            var maxX = 100;
            var maxY = 100;
            var point0 = new DelaunayPoint(0, 0);
            var point1 = new DelaunayPoint(0, maxY);
            var point2 = new DelaunayPoint(maxX, maxY);
            var point3 = new DelaunayPoint(maxX, 0);
            var point4 = new DelaunayPoint(maxX / 2, maxY / 2);
            var points = new List<DelaunayPoint> {point0, point1, point2, point3};
            var triangle1 = new DelaunayTriangle(point0, point1, point2);
            var triangle2 = new DelaunayTriangle(point0, point2, point3);
            var border = new List<DelaunayTriangle> {triangle1, triangle2};

            var delaunay = Delaunay.Compute(
                new List<DelaunayPoint>
                {
                    point0,
                    point1,
                    point2,
                    point3,
                    point4
                },
                border
            );
            Assert.True(delaunay.Count == 4);

            var voronoi = Delaunay.Voronoi(delaunay);
            // TODO: We expect 8 because it currently outputs the lines repeated twice.
            Assert.True(voronoi.Count == 8);
        }
    }
}