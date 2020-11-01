using System.Collections;
using System.Collections.Generic;
using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    public class Polyline2dUnitSquareAndSegments : IEnumerable<object[]>
    {
        private readonly Point2d pt1 = new Point2d(0, 0);

        private readonly Point2d pt2 = new Point2d(1, 0);

        private readonly Point2d pt3 = new Point2d(1, 1);

        private readonly Point2d pt4 = new Point2d(0, 1);


        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new Polyline2d(
                    new List<Point2d> {this.pt1, this.pt2, this.pt3, this.pt4},
                    false),
                3
            };
            yield return new object[]
            {
                new Polyline2d(
                    new List<Point2d> {this.pt1, this.pt2, this.pt3, this.pt4},
                    true),
                4
            };
        }


        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    public class Polyline2dDataSet : IEnumerable<object[]>
    {
        private readonly Point2d pt1 = new Point2d(0, 0);

        private readonly Point2d pt2 = new Point2d(1, 0);

        private readonly Point2d pt3 = new Point2d(1, 1);

        private readonly Point2d pt4 = new Point2d(0, 1);


        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new Polyline2d(
                    new List<Point2d> {this.pt1, this.pt2, this.pt3, this.pt4},
                    false)
            };
        }


        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }

    public class Polyline2dTests
    {
        [Theory]
        [ClassData(typeof(Polyline2dUnitSquareAndSegments))]
        public void Constructor_ClosedOption_AddsVertexAndSegment(
            Polyline2d polyline,
            int expectedSegments)
        {
            Assert.True(polyline.Vertices.Count == expectedSegments + 1);
            Assert.True(polyline.Segments.Count == expectedSegments);
        }


        [Theory]
        [ClassData(typeof(Polyline2dUnitSquareAndSegments))]
        public void DefaultDomain_IsParametrizedByArcLength(
            Polyline2d polyline,
            double expectedLength) => Assert.True(polyline.Domain.End == expectedLength);


        [Theory]
        [ClassData(typeof(Polyline2dDataSet))]
        public void Reparametrize_SetsAllSegmentsDomain(Polyline2d polyline)
        {
            polyline.Reparametrize();

            Assert.True(polyline.Domain.End == polyline.Segments[^1].Domain.End);
            Assert.True(polyline.Domain.End == 1);
        }


        [Theory]
        [ClassData(typeof(Polyline2dDataSet))]
        public void Check_IsClockwise(Polyline2d polyline)
        {
            polyline.IsClosed = true;
            var cond = polyline.IsClockwise();
            Assert.False(cond);
        }


        [Theory]
        [ClassData(typeof(Polyline2dDataSet))]
        public void CanCompute_Area(Polyline2d polyline)
        {
            polyline.IsClosed = true;
            var area = polyline.Area();
            Assert.True(area == 1.0);
            polyline.IsClosed = false;
            area = polyline.Area();
            Assert.True(area == 0.0);
        }
    }
}