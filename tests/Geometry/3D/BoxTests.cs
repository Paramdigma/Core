using Paramdigma.Core.Collections;
using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry._3D
{
    public class BoxTests
    {
        [Fact]
        public void CanCreate_FromCorners()
        {
            var box = new Box(
                Point3d.WorldOrigin,
                new Point3d(1, 1, 1)
            );
            Assert.Equal(Plane.WorldXY, box.Plane);
            Assert.Equal(Interval.Unit, box.DomainX);
            Assert.Equal(Interval.Unit, box.DomainY);
            Assert.Equal(Interval.Unit, box.DomainZ);
            Assert.Equal(Point3d.WorldOrigin, box.Min);
            Assert.Equal(new Point3d(1, 1, 1), box.Max);
            Assert.Equal(new Point3d(.5, .5, .5), box.Center);
        }


        [Fact]
        public void CanCreate_FromPlaneAndDimensions()
        {
            var box = new Box(Plane.WorldXY, Interval.Unit, Interval.Unit, Interval.Unit);
            Assert.Equal(Plane.WorldXY, box.Plane);
            Assert.Equal(Interval.Unit, box.DomainX);
            Assert.Equal(Interval.Unit, box.DomainY);
            Assert.Equal(Interval.Unit, box.DomainZ);
            Assert.Equal(Point3d.WorldOrigin, box.Min);
            Assert.Equal(new Point3d(1, 1, 1), box.Max);
            Assert.Equal(new Point3d(.5, .5, .5), box.Center);
        }
    }
}