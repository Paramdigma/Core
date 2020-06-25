using System.Linq;
using Paramdigma.Core.Geometry;
using Paramdigma.Core.SpatialSearch;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry.SpatialStructures
{
    public class QuadTreeTests
    {
        [Fact]
        public void CanCreate_QuadTree()
        {
            var range = new Box2d(
                Point2d.Origin,
                new Point2d(1, 1)
            );

            var tree = new QuadTree(range, .26);
            var pt = new Point2d(0.35, 0.35);
            var low = new Point2d(0.3, 0.3);
            var high = new Point2d(0.4, 0.4);
            var check = tree.Insert(pt);
            Assert.True(check);
            var expected = tree.QueryRange(
                new Box2d(low, high)
            );
            Assert.Equal(pt, expected.ToList()[0]);
        }
    }
}