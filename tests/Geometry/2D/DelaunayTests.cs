using System;
using System.Collections.Generic;
using System.Linq;
using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    public class DelaunayTests
    {
        [Fact]
        public void CanComputeDelaunay()
        {
            var maxX = 100;
            var maxY = 100;
            var point0 = new DelaunayPoint(0, 0);
            var point1 = new DelaunayPoint(0, maxY);
            var point2 = new DelaunayPoint(maxX, maxY);
            var point3 = new DelaunayPoint(maxX, 0);
            var point4 = new DelaunayPoint(maxX / 2.0, maxY / 2.0);

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
                                    )
                                   .ToList();
            Assert.True(delaunay.Count == 4);

            var voronoi = Delaunay.Voronoi(delaunay);
            // TODO: We expect 8 because it currently outputs the lines repeated twice.
            Assert.True(voronoi.ToList().Count == 8);
        }


        private List<DelaunayPoint> GeneratePoints(
            int ammount,
            double maxX,
            double maxY,
            out List<DelaunayTriangle> border)
        {
            var point0 = new DelaunayPoint(0, 0);
            var point1 = new DelaunayPoint(0, maxY);
            var point2 = new DelaunayPoint(maxX, maxY);
            var point3 = new DelaunayPoint(maxX, 0);
            var triangle1 = new DelaunayTriangle(point0, point1, point2);
            var triangle2 = new DelaunayTriangle(point0, point2, point3);
            border = new List<DelaunayTriangle> {triangle1, triangle2};
            var rnd = new Random();
            var points = new List<DelaunayPoint>();
            for (var i = 0; i < ammount - 4; i++)
                points.Add(RandomPoint(rnd, 0, maxX));
            return points;
        }


        private static DelaunayPoint RandomPoint(
            Random randGenerator,
            double minValue,
            double maxValue)
        {
            var range = maxValue - minValue;
            var randomPoint = new DelaunayPoint(
                randGenerator.NextDouble() * range + minValue,
                randGenerator.NextDouble() * range + minValue);
            return randomPoint;
        }


        [Fact]
        public void CanCompare_DelaunayEdges()
        {
            var edgeA = new DelaunayEdge(new DelaunayPoint(0, 0), new DelaunayPoint(1, 0));
            var edgeB = new DelaunayEdge(new DelaunayPoint(0, 0), new DelaunayPoint(1, 0));
            Assert.Equal(edgeA, edgeB);
            Assert.Equal(edgeA.GetHashCode(), edgeB.GetHashCode());
            Assert.NotNull(edgeA);
        }


        [Fact]
        public void CanCreate_DelaunayPoint_FromPoint2d()
        {
            var pt = new Point2d(.5, .5);
            var dpt = new DelaunayPoint(pt);
            Assert.Equal(pt.X, dpt.X);
            Assert.Equal(pt.Y, dpt.Y);
        }
    }
}