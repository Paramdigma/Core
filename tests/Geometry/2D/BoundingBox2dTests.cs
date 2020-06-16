using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    public class BoundingBox2dTests
    {
        [Fact]
        public void CanCreate_FromTwoCorners()
        {
            Assert.NotNull(new BoundingBox2d( new Point2d(1,1),Point2d.Origin));
        }

        [Fact]
        public void CanGet_AllFourCorners()
        {
            var box = new BoundingBox2d(Point2d.Origin, new Point2d(1, 1));
            
            Assert.Equal(new Point2d(0,0),box.BottomLeft);
            Assert.Equal(new Point2d(1,0),box.BottomRight);
            Assert.Equal(new Point2d(0,1),box.TopLeft);
            Assert.Equal(new Point2d(1,1),box.TopRight);
        }
    }
}