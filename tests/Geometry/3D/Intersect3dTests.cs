using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    public class Intersect3dTests
    {
        [Fact]
        public void CanIntersect_Line_Line()
        {
            var lineA = new Line(Point3d.WorldOrigin, new Point3d(1, 1, 1));
            var lineB = new Line(new Point3d(1, 0, 0), new Point3d(0, 1, 1));

            var status = Intersect3D.LineLine(lineA, lineB, out var result);
            Assert.Equal(Intersect3D.ISLineLine.Point, status);
            Assert.Equal(new Point3d(0.5, 0.5, 0.5), result.PointA);
            Assert.Equal(new Point3d(0.5, 0.5, 0.5), result.PointB);
            Assert.Equal(0.5, result.TA);
            Assert.Equal(0.5, result.TB);
        }


        [Fact]
        public void CanIntersect_Line_Plane()
        {
            var expected = new Point3d(.4, .23, 0);
            var a = new Point3d(.4, .23, 1);
            var b = new Point3d(.4, .23, -1);
            var lineA = new Line(a, b);
            var plane = Plane.WorldXY;

            var status = Intersect3D.LinePlane(lineA, plane, out var actual);
            Assert.Equal(Intersect3D.ISLinePlane.Point, status);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CannotIntersect_Line_Plane_DoNotTouch()
        {
            var a = new Point3d(.4, .23, 1);
            var b = new Point3d(.4, .23, .2);
            var lineA = new Line(a, b);
            var plane = Plane.WorldXY;

            var status = Intersect3D.LinePlane(lineA, plane, out var actual);
            Assert.Equal(Intersect3D.ISLinePlane.NoIntersection, status);
            Assert.Null(actual);
        }

        [Fact]
        public void CannotIntersect_Line_Plane_DoNotTouchAndAreParallel()
        {
            var a = new Point3d(.4, .23, 1);
            var b = new Point3d(.9, .23, 1);
            var lineA = new Line(a, b);
            var plane = Plane.WorldXY;

            var status = Intersect3D.LinePlane(lineA, plane, out var actual);
            Assert.Equal(Intersect3D.ISLinePlane.NoIntersection, status);
            Assert.Null(actual);
        }

        [Fact]
        public void CannotIntersect_Line_Plane_Parallel()
        {
            var a = new Point3d(.6, .6, 0);
            var b = new Point3d(.4, .2, 0);
            var lineA = new Line(a, b);
            var plane = Plane.WorldXY;

            var status = Intersect3D.LinePlane(lineA, plane, out var actual);
            Assert.Equal(Intersect3D.ISLinePlane.OnPlane, status);
            Assert.Null(actual);
        }
    }
}