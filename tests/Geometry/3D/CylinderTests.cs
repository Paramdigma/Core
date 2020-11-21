using System;
using Paramdigma.Core.Collections;
using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    public class CylinderTests
    {
        [Fact]
        private void CanCompute_PointAtCylinder()
        {
            var cyl = new Cylinder(Plane.WorldXY, 1, Interval.Unit);
            var actual = cyl.PointAt(0, 0);
            var expected = new Point3d(1, 0, 0);
            Assert.Equal(actual, expected);
            Assert.Throws<Exception>(() => cyl.PointAt(-1, 0));
            Assert.Throws<Exception>(() => cyl.PointAt(0, -1));
        }


        [Fact]
        public void CanCreate_FromDefaultConstructor()
        {
            var cyl = new Cylinder(Plane.WorldXY, 1, Interval.Unit);
            Assert.Equal(Plane.WorldXY, cyl.Plane);
            Assert.Equal(1, cyl.Radius);
            Assert.Equal(1, cyl.Height);
        }


        [Fact]
        public void CanCreate_FromPlaneHeightRadius()
        {
            var cyl = new Cylinder(Plane.WorldXY, 1, Interval.Unit);
            Assert.Equal(Plane.WorldXY, cyl.Plane);
            Assert.Equal(1, cyl.Radius);
            Assert.Equal(1, cyl.Height);
        }
        
        [Fact]
        public void CannotCreate_FromInvalidData()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new Cylinder(Plane.WorldXY, -1, Interval.Unit));
            Assert.Throws<ArgumentException>(() => new Cylinder(Plane.WorldXY, 1, new Interval(0, Settings.Tolerance/2)));
        }
    }
}