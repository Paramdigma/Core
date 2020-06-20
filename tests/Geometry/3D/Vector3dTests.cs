using System;
using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    public class Vector3dTests
    {
        [Fact]
        public void Can_ComputeAngle()
        {
            var v1 = Vector3d.UnitX;
            var v2 = Vector3d.UnitY;
            var v3 = v1 + v2;
            var q = 0.25 * Math.PI;
            var a = Vector3d.Angle(v1, v2);
            var a2 = Vector3d.Angle(v1, v3);
            Assert.True(a == 0.5 * Math.PI);
            Assert.True(Math.Abs(a2 - q) <= Settings.Tolerance);
        }

        [Fact]
        public void CanBe_Added()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            var vA = new Vector3d(a, b, c);
            var vB = new Vector3d(b, c, a);
            var s = vA - vB;
            var ptResult = new Vector3d(a + b, b + c, c + a);
            Assert.True(vA + vB == ptResult);
        }

        [Fact]
        public void CanBe_ConvertedToString()
        {
            var result = "Vector3d{ 1, 0, 0 }";
            var v = Vector3d.UnitX;
            var s = v.ToString();
            Assert.True(s == result);
        }

        [Fact]
        public void CanBe_Created()
        {
            var v = Vector3d.UnitY;
            var pt = new Point3d(0, 2, 2);
            var newV = new Vector3d(v);
            var newVp = new Vector3d(pt);

            Assert.True(v == newV);
            Assert.True(pt == newVp);
        }

        [Fact]
        public void CanBe_Divided()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            const double m = 1.45;
            var v = new Vector3d(a, b, c);
            var ptResult = new Vector3d(a / m, b / m, c / m);
            Assert.True(v / m == ptResult);
        }

        [Fact]
        public void CanBe_Multiplied()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            const double m = 1.45;
            var v = new Vector3d(a, b, c);
            var ptResult = new Vector3d(a * m, b * m, c * m);
            Assert.True(v * m == ptResult);
        }

        [Fact]
        public void CanBe_Negated()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            var v = new Vector3d(a, b, c);
            var ptResult = new Vector3d(-a, -b, -c);
            Assert.True(-v == ptResult);
        }

        [Fact]
        public void CanBe_Substracted()
        {
            const double a = 3.3;
            const double b = 2.2;
            const double c = 4.11;
            var vA = new Vector3d(a, b, c);
            var vB = new Vector3d(b, c, a);
            var ptResult = new Vector3d(a - b, b - c, c - a);
            Assert.True(vA - vB == ptResult);
        }

        [Fact]
        public void EqualsAndHashCode_HaveConsistentResults()
        {
            var v = new Vector3d(-1, 1, 1);
            var v2 = new Vector3d(-1.000000005, 1, 1);
            var b1 = v == v2;
            var b2 = v.GetHashCode() == v2.GetHashCode();

            Assert.True(b1 && b1 == b2);
        }
    }
}