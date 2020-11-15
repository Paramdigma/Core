using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    public class Line2dTests
    {
        [Fact]
        public void CanBe_Created()
        {
            var ptA = new Point2d(0, 1);
            var ptB = new Point2d(1, 0);
            var v = new Vector2d(1, 0);
            var line = new Line2d(ptA, ptB);
            var lineB = new Line2d(ptA, v);
            var lineC = new Line2d(ptA, v, 3);

            Assert.NotNull(line);
            Assert.NotNull(lineB);
            Assert.NotNull(lineC);
        }
    }
}