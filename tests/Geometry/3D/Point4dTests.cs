using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    public class Point4dTests
    {
        [Fact]
        public void CanBe_Added()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            const double d = 1.344;

            var ptA = new Point4d(a, b, c, d);
            var ptB = new Point4d(b, c, a, d);
            var ptResult = new Point4d(a + b, b + c, c + a, d + d);
            Assert.True(ptA + ptB == ptResult);
        }

        [Fact]
        public void CanBe_Added_WithVector()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            const double d = 1.344;

            var ptA = new Point4d(a, b, c, d);
            var v = new Vector3d(b, c, a);
            var ptResult = new Point4d(a + b, b + c, c + a, d);
            Assert.True(ptA + v == ptResult);
        }

        [Fact]
        public void CanBe_Created()
        {
            var pt3 = new Point3d(1, 0, 0);
            var pt4 = new Point4d(1, 0, 0, 1);
            var pt42 = new Point4d(pt3);

            Assert.Equal(pt42, pt4);
        }

        [Fact]
        public void CanBe_Divided()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            const double d = 1.344;
            const double m = 1.45;
            var ptA = new Point4d(a, b, c, d);
            var ptResult = new Point4d(a / m, b / m, c / m, d / m);
            Assert.True(ptA / m == ptResult);
        }

        [Fact]
        public void CanBe_Multiplied()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            const double d = 1.344;
            const double m = 1.45;
            var ptA = new Point4d(a, b, c, d);
            var ptResult = new Point4d(a * m, b * m, c * m, d * m);
            Assert.True(ptA * m == ptResult);
            Assert.True(m * ptA == ptResult);
        }

        [Fact]
        public void CanBe_Negated()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            const double d = 1.344;

            var ptA = new Point4d(a, b, c, d);
            var ptResult = new Point4d(-a, -b, -c, d);
            Assert.True(-ptA == ptResult);
        }

        [Fact]
        public void CanBe_Subtracted()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            const double d = 1.344;

            var ptA = new Point4d(a, b, c, d);
            var ptB = new Point4d(b, c, a, d);
            var expected = new Point4d(a - b, b - c, c - a, d - d);
            var actual = ptA - ptB;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void CanCheck_Equality()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            const double d = 1.344;

            var ptA = new Point4d(a, b, c, d);
            var expectedEqual = new Point4d(a + (Settings.Tolerance / 2), b, c, d);
            var expectedNotEqual = new Point4d(a + (Settings.Tolerance * 2), b, c, d);
            Assert.True(ptA == expectedEqual);
            Assert.Equal(ptA.GetHashCode(), expectedEqual.GetHashCode());
            Assert.True(ptA != expectedNotEqual);
            Assert.NotEqual(ptA.GetHashCode(), expectedNotEqual.GetHashCode());
            Assert.False(ptA == null);
        }

        [Fact]
        public void CanCreate_FromPointAndWeight()
        {
            var pt = new Point3d(1, 0, 1);
            const int weight = 1;
            var pt4 = new Point4d(pt, weight);
            Assert.Equal(pt, pt4.Position);
            Assert.Equal(weight, pt4.Weight);
        }

        [Fact]
        public void CanToggle_IsUnset_OnWeightChange()
        {
            var pt = new Point4d();
            Assert.True(pt.IsUnset);
            pt.Weight += 1;
            Assert.False(pt.IsUnset);
        }
    }
}