using System;
using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    public class Ray3dTests
    {
        [Fact]
        public void CanCompute_PointAt()
        {
            var ray = new Ray(Point3d.WorldOrigin, Vector3d.UnitX);
            var expected = new Point3d(3, 0, 0);
            var actual = ray.PointAt(3);
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void CanCreate_AndThrowExceptions()
        {
            var ray = new Ray(Point3d.WorldOrigin, Vector3d.UnitX);
            Assert.Equal(Point3d.WorldOrigin, ray.Origin);
            Assert.Equal(Vector3d.UnitX, ray.Direction);

            Assert.Throws<ArgumentNullException>(() => new Ray(Point3d.WorldOrigin, null));
            Assert.Throws<ArgumentNullException>(() => new Ray(null, Vector3d.UnitY));
        }
    }
}