using Paramdigma.Core.Extensions;
using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Extensions
{
    public class ListExtensionsTests
    {
        [Fact]
        public void CanCreate_Repeated()
        {
            for (var i = 1; i < 10; i++)
            {
                var list2 = Lists.Repeated(new Point3d(1, 1, 1), 4);
                list2.ForEach(pt => Assert.Equal(new Point3d(1, 1, 1), pt));
            }
        }

        [Fact]
        public void CanCreate_RepeatedDefault()
        {
            for (var i = 1; i < 10; i++)
            {
                var list2 = Lists.RepeatedDefault<Point3d>(i);
                Assert.True(list2.Count == i);
            }
        }
    }
}