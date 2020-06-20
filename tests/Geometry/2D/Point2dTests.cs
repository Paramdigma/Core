using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    public class Point2dTests
    {
        [Fact]
        public void Can_AddVector()
        {
            var pt = new Point2d(0, 1);
            Vector2d v = pt;
            var pt2 = pt + v;
            var expected = new Point2d(0, 2);
            Assert.True(pt2 == expected);
        }

        [Fact]
        public void CanBe_Added()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            var ptA = new Point2d(a, b);
            var ptB = new Point2d(b, c);
            var s = ptA - ptB;
            var ptResult = new Point2d(a + b, b + c);
            Assert.True(ptA + ptB == ptResult);
        }

        [Fact]
        public void CanBe_ConvertedToVector()
        {
            var pt = new Point2d(0, 1);
            Vector2d v = pt;
            var pt2 = (Point2d)v;
            Assert.True(pt == pt2);
        }

        [Fact]
        public void CanBe_Divided()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double m = 1.45;
            var ptA = new Point2d(a, b);
            var ptResult = new Point2d(a / m, b / m);
            Assert.True(ptA / m == ptResult);
        }

        [Fact]
        public void CanBe_Multiplied()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double m = 1.45;
            var ptA = new Point2d(a, b);
            var ptResult = new Point2d(a * m, b * m);
            Assert.True(ptA * m == ptResult);
            Assert.True(m * ptA == ptResult);
        }

        [Fact]
        public void CanBe_Negated()
        {
            const double a = 3.3;
            const double b = 2.2;
            var ptA = new Point2d(a, b);
            var ptResult = new Point2d(-a, -b);
            Assert.True(-ptA == ptResult);
        }

        [Fact]
        public void CanBe_Substracted()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            var ptA = new Point2d(a, b);
            var ptB = new Point2d(b, c);
            var ptResult = new Point2d(a - b, b - c);
            Assert.True(ptA - ptB == ptResult);
        }

        [Fact]
        public void Create_FromPoint()
        {
            var expected = new Point2d(2.4, 2.5);
            var copy = new Point2d(expected);
            Assert.True(expected == copy);
        }

        [Fact]
        public void Create_Origin()
        {
            var origin = Point2d.Origin;
            var empty = new Point2d();
            var expected = new Point2d(0, 0);

            Assert.True(origin == expected);
            Assert.True(empty == expected);
        }

        [Fact]
        public void EqualsAndHashCode_HaveConsistentResults()
        {
            var pt = new Point2d(1.00000009, -1);
            var pt2 = new Point2d(1, -1);
            var b1 = pt == pt2;
            var b2 = pt.GetHashCode() == pt2.GetHashCode();
            var other = "S";
            Assert.True(b1 && b1 == b2);
            Assert.False(pt != pt2);
            Assert.False(pt.Equals(other));
        }
    }
}