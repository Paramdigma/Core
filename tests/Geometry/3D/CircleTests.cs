using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    public class CircleTests
    {
        private static Circle TestCircle => new Circle(Plane.WorldXY, 1);


        [Fact]
        public void CanCompute_BinormalAt()
        {
            Assert.Equal(TestCircle.BinormalAt(0), Vector3d.UnitZ);
            Assert.Equal(TestCircle.BinormalAt(0.25), Vector3d.UnitZ);
            Assert.Equal(TestCircle.BinormalAt(0.5), Vector3d.UnitZ);
            Assert.Equal(TestCircle.BinormalAt(0.75), Vector3d.UnitZ);
        }


        [Fact]
        public void CanCompute_FrameAt() => Assert.Equal(
            TestCircle.FrameAt(0),
            new Plane(TestCircle.PointAt(0), -Vector3d.UnitX, Vector3d.UnitZ));


        [Fact]
        public void CanCompute_NormalAt()
        {
            Assert.Equal(TestCircle.NormalAt(0), -Vector3d.UnitX);
            Assert.Equal(TestCircle.NormalAt(0.25), -Vector3d.UnitY);
            Assert.Equal(TestCircle.NormalAt(0.5), Vector3d.UnitX);
            Assert.Equal(TestCircle.NormalAt(0.75), Vector3d.UnitY);
        }


        [Fact]
        public void CanCompute_PointAt()
        {
            Assert.Equal(TestCircle.PointAt(0), Vector3d.UnitX);
            Assert.Equal(TestCircle.PointAt(0.25), Vector3d.UnitY);
            Assert.Equal(TestCircle.PointAt(0.5), -Vector3d.UnitX);
            Assert.Equal(TestCircle.PointAt(0.75), -Vector3d.UnitY);
        }


        [Fact]
        public void CanCompute_TangentAt()
        {
            Assert.Equal(TestCircle.TangentAt(0), Vector3d.UnitY);
            Assert.Equal(TestCircle.TangentAt(0.25), -Vector3d.UnitX);
            Assert.Equal(TestCircle.TangentAt(0.5), -Vector3d.UnitY);
            Assert.Equal(TestCircle.TangentAt(0.75), Vector3d.UnitX);
        }
    }
}