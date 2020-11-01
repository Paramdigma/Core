using System.Collections.Generic;
using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    public class Point3dTests
    {
        public static IEnumerable<object[]> UnsetPointData => new List<object[]>
        {
            new object[] {new Point3d()},
            new object[] {Point3d.Unset},
            new object[] {new Point4d()}
        };


        [Theory]
        [ClassData(typeof(Point3dEqualDataset))]
        public void EqualsAndHashCode_HaveConsistentResults(Point3d pt, Point3d pt2)
        {
            var b1 = pt == pt2;
            var b2 = pt.GetHashCode() == pt2.GetHashCode();

            Assert.True(b1 && b1 == b2);
            Assert.False(pt != pt2);
        }


        [Theory]
        [MemberData(nameof(UnsetPointData))]
        public void CanToggleUnset_WithX(BasePoint pt)
        {
            Assert.True(pt.IsUnset);
            pt.X += 1;
            Assert.False(pt.IsUnset);
        }


        [Theory]
        [MemberData(nameof(UnsetPointData))]
        public void CanToggleUnset_WithY(BasePoint pt)
        {
            Assert.True(pt.IsUnset);
            pt.Y += 1;
            Assert.False(pt.IsUnset);
        }


        [Theory]
        [MemberData(nameof(UnsetPointData))]
        public void CanToggleUnset_WithZ(BasePoint pt)
        {
            Assert.True(pt.IsUnset);
            pt.Z += 1;
            Assert.False(pt.IsUnset);
        }


        [Fact]
        public void Can_OperateOnItself()
        {
            var pt = Point3d.WorldOrigin;
            var pt2 = new Point3d(1, 0, 0);
            pt.Add(pt2);
            Assert.True(pt == pt2);
            pt.Substract(pt2);
            Assert.True(pt == Point3d.WorldOrigin);
            var pt3 = pt2 * 2;
            pt2.Multiply(2);
            Assert.True(pt2 == pt3);
            var pt4 = pt2 / 2;
            pt2.Divide(2);
            Assert.True(pt2 == new Point3d(1, 0, 0));
            pt2.Negate();
            Assert.True(pt2 == new Point3d(-1, 0, 0));
        }


        [Fact]
        public void CanBe_Added()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            var ptA = new Point3d(a, b, c);
            var ptB = new Point3d(b, c, a);
            var s = ptA - ptB;
            var ptResult = new Point3d(a + b, b + c, c + a);
            Assert.True(ptA + ptB == ptResult);
        }


        [Fact]
        public void CanBe_Created()
        {
            var pt2 = new Point2d(1, 0);
            var pt3 = new Point3d(1, 0, 0);
            var pt4 = new Point4d(1, 0, 0, 1);
            var npt2 = new Point3d(pt2);
            var npt4 = new Point3d(pt4);
            var npt5 = ( Point3d ) pt4;
            Assert.True(pt3 == npt2);
            Assert.True(pt3 == npt4);
            Assert.True(pt3 == npt5);
        }


        [Fact]
        public void CanBe_Divided()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            const double m = 1.45;
            var ptA = new Point3d(a, b, c);
            var ptResult = new Point3d(a / m, b / m, c / m);
            Assert.True(ptA / m == ptResult);
        }


        [Fact]
        public void CanBe_Multiplied()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            const double m = 1.45;
            var ptA = new Point3d(a, b, c);
            var ptResult = new Point3d(a * m, b * m, c * m);
            Assert.True(ptA * m == ptResult);
        }


        [Fact]
        public void CanBe_Negated()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            var ptA = new Point3d(a, b, c);
            var ptResult = new Point3d(-a, -b, -c);
            Assert.True(-ptA == ptResult);
        }


        [Fact]
        public void CanBe_Substracted()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            var ptA = new Point3d(a, b, c);
            var v = new Vector3d(b, c, a);
            var ptB = ( Point3d ) v;
            var ptResult = new Point3d(a - b, b - c, c - a);
            Assert.True(ptA - ptB == ptResult);
            Assert.True(ptA - v == ptResult);
        }


        [Fact]
        public void CanConvert_ToArray()
        {
            var v = Vector3d.UnitX;
            var arr = v.ToArray();
            Assert.True(arr.Length == 3);
            Assert.True(arr[0] == 1 && arr[1] == 0 && arr[2] == 0);
        }
    }
}