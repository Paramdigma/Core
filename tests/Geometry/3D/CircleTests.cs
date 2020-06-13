using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry._3D
{
    public class CircleTests
    {
        public Circle testCircle => new Circle(Plane.WorldXY, 1);
        
        [Fact]
        public void CanCompute_PointAt()
        {
            Assert.Equal(this.testCircle.PointAt(0),Vector3d.UnitX);
            Assert.Equal(this.testCircle.PointAt(0.25),Vector3d.UnitY);
            Assert.Equal(this.testCircle.PointAt(0.5),-Vector3d.UnitX);
            Assert.Equal(this.testCircle.PointAt(0.75),-Vector3d.UnitY);
        }
        [Fact]
        public void CanCompute_TangentAt()
        {
            Assert.Equal(this.testCircle.TangentAt(0),Vector3d.UnitY);
            Assert.Equal(this.testCircle.TangentAt(0.25),-Vector3d.UnitX);
            Assert.Equal(this.testCircle.TangentAt(0.5),-Vector3d.UnitY);
            Assert.Equal(this.testCircle.TangentAt(0.75),Vector3d.UnitX);
        }
        [Fact]
        public void CanCompute_NormalAt()
        {
            Assert.Equal(this.testCircle.NormalAt(0),-Vector3d.UnitX);
            Assert.Equal(this.testCircle.NormalAt(0.25),-Vector3d.UnitY);
            Assert.Equal(this.testCircle.NormalAt(0.5),Vector3d.UnitX);
            Assert.Equal(this.testCircle.NormalAt(0.75),Vector3d.UnitY);

        }
        [Fact]
        public void CanCompute_BinormalAt()
        {
            Assert.Equal(this.testCircle.BinormalAt(0),Vector3d.UnitZ);
            Assert.Equal(this.testCircle.BinormalAt(0.25),Vector3d.UnitZ);
            Assert.Equal(this.testCircle.BinormalAt(0.5),Vector3d.UnitZ);
            Assert.Equal(this.testCircle.BinormalAt(0.75),Vector3d.UnitZ);

        }
        [Fact]
        public void CanCompute_FrameAt()
        {
            Assert.Equal(this.testCircle.FrameAt(0),new Plane(testCircle.PointAt(0),-Vector3d.UnitX,Vector3d.UnitZ));

        }
    }
}