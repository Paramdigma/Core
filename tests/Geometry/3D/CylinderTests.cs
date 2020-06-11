using Paramdigma.Core.Collections;
using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry._3D
{
    public class CylinderTests
    {
        [Fact]
        public void CanCreate_FromPlaneHeightRadius()
        {
            var cyl = new Cylinder(Plane.WorldXY,1,Interval.Unit);
            Assert.Equal(Plane.WorldXY,cyl.Plane);
            Assert.Equal(1,cyl.Radius);
            Assert.Equal(1,cyl.Height);
                
        }
    }
}