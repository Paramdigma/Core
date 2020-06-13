using Paramdigma.Core.HalfEdgeMesh;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    public class MeshPointTests
    {
        [Fact]
        public void CanConstruct_FromNumbers()
        {
            var pt = new MeshPoint(1, 0.4, 0.5, 0.6);
            Assert.Equal(1, pt.FaceIndex);
            Assert.Equal(0.4, pt.U);
            Assert.Equal(0.5, pt.V);
            Assert.Equal(0.6, pt.W);
        }

        [Fact]
        public void CanConvert_ToString()
        {
            var pt = new MeshPoint(1, 0.4, 0.5, 0.6);
            Assert.NotNull(pt.ToString());
        }
    }
}