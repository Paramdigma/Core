using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    public class Vector2dTests
    {
        [Fact]
        public void Can_AddVector()
        {
            var pt = new Vector2d(0, 1);
            var v = ( Point2d ) pt;
            var pt2 = pt + v;
            var expected = new Vector2d(0, 2);
            Assert.True(pt2 == expected);
        }


        [Fact]
        public void CanBe_Added()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            var ptA = new Vector2d(a, b);
            var ptB = new Vector2d(b, c);
            var ptResult = new Vector2d(a + b, b + c);
            Assert.True(ptA + ptB == ptResult);
        }


        [Fact]
        public void CanBe_ConvertedTo()
        {
            var pt = new Vector2d(0, 1);
            var v = ( Point2d ) pt;
            Vector2d pt2 = v;
            Assert.True(pt == pt2);
        }


        [Fact]
        public void CanBe_ConvertedToString()
        {
            var result = "Vector3d [1, 0]";
            var v = new Vector2d(1, 0);
            var s = v.ToString();
            Assert.True(s == result);
        }


        [Fact]
        public void CanBe_Divided()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double m = 1.45;
            var ptA = new Vector2d(a, b);
            var ptResult = new Vector2d(a / m, b / m);
            Assert.True(ptA / m == ptResult);
        }


        [Fact]
        public void CanBe_Multiplied()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double m = 1.45;
            var ptA = new Vector2d(a, b);
            var ptResult = new Vector2d(a * m, b * m);
            Assert.True(ptA * m == ptResult);
            Assert.True(m * ptA == ptResult);
        }


        [Fact]
        public void CanBe_Negated()
        {
            const double a = 3.3;
            const double b = 2.2;
            var ptA = new Vector2d(a, b);
            var ptResult = new Vector2d(-a, -b);
            Assert.True(-ptA == ptResult);
        }


        [Fact]
        public void CanBe_Substracted()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            var ptA = new Vector2d(a, b);
            var ptB = new Vector2d(b, c);
            var ptResult = new Vector2d(a - b, b - c);
            Assert.True(ptA - ptB == ptResult);
        }


        [Fact]
        public void CanCompute_DotProduct()
        {
            var v = new Vector2d(1, 2);
            var v2 = new Vector2d(3.211, -2.22);
            v.DotProduct(v2);
            // TODO: Missing assertions.
        }


        [Fact]
        public void CanCompute_PerpendicularVector()
        {
            var v = new Vector2d(1, 2);
            var expected = new Vector2d(-2, 1);
            var perp = v.Perp();
            Assert.True(perp == expected);
        }


        [Fact]
        public void CanCompute_PerpProduct()
        {
            var v = new Vector2d(1, 2);
            var v2 = new Vector2d(3.211, -2.22);
            v.PerpProduct(v2);
            // TODO: Missing Assertions
        }


        [Fact]
        public void CanUnitize()
        {
            var v = new Vector2d(5.2333, 0);
            v.Unitize();
            var expected = new Vector2d(1, 0);
            Assert.True(v == expected);
        }


        [Fact]
        public void Create_FromPoint()
        {
            var expected = new Vector2d(2.4, 2.5);
            var copy = new Vector2d(expected);
            Assert.True(expected == copy);
        }


        [Fact]
        public void Create_Origin()
        {
            var empty = new Vector2d(0, 0);
            var expected = new Vector2d(0, 0);
            Assert.True(empty == expected);
        }


        [Fact]
        public void EqualsAndHashCode_HaveConsistentResults()
        {
            var pt = new Vector2d(1.000000009, -1);
            var pt2 = new Vector2d(1, -1);
            var b1 = pt == pt2;
            var b2 = pt.GetHashCode() == pt2.GetHashCode();
            var other = "S";
            Assert.True(b1 && b1 == b2);
            Assert.False(pt != pt2);
            Assert.False(pt.Equals(other));
        }
    }
}