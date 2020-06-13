using System;
using System.Collections.Generic;
using Paramdigma.Core.Exceptions;
using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    public class Polyline3dTests : CurveBaseTests<Polyline>
    {
        private Polyline GetTestPolyline(int count)
        {
            var knots = new List<Point3d>();
            for (int i = 0; i <= count; i++)
            {
                knots.Add(new Point3d(i, 0, 0));
            }

            return new Polyline(knots);
        }

        [Fact]
        public override void CanGet_Length()
        {
            for (int i = 3; i < 4; i++)
            {
                var poly = this.GetTestPolyline(i);
                var length = poly.Length;
                Assert.True(length == i, $"Length {length} is not {i}");
            }
        }

        [Fact]
        public void CanEnumerate_Knots()
        {
            var poly = this.GetTestPolyline(3);
            foreach (var knot in poly)
            {
                Assert.NotNull(knot);
            }
        }

        [Fact]
        public override void CanGet_PointAt()
        {
            var poly = this.GetTestPolyline(3);
            var pt = poly.PointAt(1);
            var expected = new Point3d(1, 0, 0);
            Assert.Equal(expected, pt);
        }

        [Fact]
        public void CanAdd_Knot()
        {
            var poly = this.GetTestPolyline(3);
            var segCount = poly.Segments.Count;
            var knotCount = poly.Knots.Count;
            poly.AddKnot(poly[0]);
            Assert.Equal(segCount + 1, poly.Segments.Count);
            Assert.Equal(knotCount + 1, poly.Knots.Count);
        }

        [Fact]
        public void CanInsert_Knot()
        {
            var poly = this.GetTestPolyline(3);
            var segCount = poly.Segments.Count;
            var knotCount = poly.Knots.Count;
            poly.InsertKnot(poly[0],2);
            Assert.Equal(segCount + 1, poly.Segments.Count);
            Assert.Equal(knotCount + 1, poly.Knots.Count);
            Assert.Equal(poly[0],poly[2]);
        }

        [Fact]
        public void CanRemove_KnotAtIndex()
        {
            var poly = this.GetTestPolyline(3);
            var segCount = poly.Segments.Count;
            var knotCount = poly.Knots.Count;
            poly.RemoveKnotAt(0);

            Assert.Equal(segCount - 1, poly.Segments.Count);
            Assert.Equal(knotCount - 1, poly.Knots.Count);
        }

        [Fact]
        public void CanRemove_Knot()
        {
            var poly = this.GetTestPolyline(3);
            var segCount = poly.Segments.Count;
            var knotCount = poly.Knots.Count;
            var unexpected = new Point3d(poly[0]);
            poly.RemoveKnot(poly[0]);

            Assert.Equal(segCount - 1, poly.Segments.Count);
            Assert.Equal(knotCount - 1, poly.Knots.Count);
            Assert.NotEqual(unexpected,poly[0]);
        }

        [Fact]
        public void CanRemove_Knot_ThrowsException()
        {
            var poly = this.GetTestPolyline(3);
            var poly2 = new Polyline();
            Assert.Throws<IndexOutOfRangeException>(() => poly.RemoveKnotAt(17));
            Assert.Throws<UnsetGeometryException>(() => poly2.RemoveKnotAt(0));
        }

        [Fact]
        public void CanToggle_IsClosed()
        {
            var poly = this.GetTestPolyline(3);
            Assert.False(poly.IsClosed);
            poly.AddKnot(poly[0]);
            Assert.True(poly.IsClosed);
            poly.RemoveKnotAt(4);
            Assert.False(poly.IsClosed);
        }

        [Fact]
        public override void CanCheck_Validity()
        {
            var poly = this.GetTestPolyline(3);
            Assert.True(poly.IsValid);
            poly = new Polyline();
            Assert.False(poly.IsValid);

            var pt = new Point3d(0, 0, 0);
            poly = new Polyline(new List<Point3d> {pt, pt});
            Assert.False(poly.IsValid);
        }

        [Fact]
        public void CanAccess_UpdatedSegmentList()
        {
            var poly = this.GetTestPolyline(3);
            Assert.Equal(3,poly.Segments.Count);
            poly.AddKnot(poly[0]);
            Assert.Equal(4,poly.Segments.Count);

        }

        [Fact]
        public override void CanGet_Tangent()
        {
            //TODO: Improve test
            var poly = this.GetTestPolyline(3);
            Assert.NotNull(poly.TangentAt(1));
        }

        [Fact]
        public override void CanGet_Normal()
        {
            //TODO: Improve test
            var poly = this.GetTestPolyline(3);
            Assert.NotNull(poly.NormalAt(1));
        }

        [Fact]
        public override void CanGet_BiNormal()
        {
            //TODO: Improve test
            var poly = this.GetTestPolyline(3);
            Assert.NotNull(poly.BinormalAt(1));
        }

        [Fact]
        public override void CanGet_PerpFrame()
        {
            //TODO: Improve test
            var poly = this.GetTestPolyline(3);
            Assert.NotNull(poly.FrameAt(1));
        }
    }
}