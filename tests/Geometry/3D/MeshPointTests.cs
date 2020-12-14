using System.Collections.Generic;
using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    
    
    public class MeshPointTests
    {
        
        public Mesh FlatTriangle
        {
            get
            {
                var ptA = new Point3d(0, 0, 0);
                var ptB = new Point3d(1, 0, 0);
                var ptC = new Point3d(1, 1, 0);
                var vertices = new List<Point3d> {ptA, ptB, ptC};
                var face = new List<int> {0, 1, 2};
                var mesh = new Mesh(vertices, new List<List<int>> {face});
                return mesh;
            }
        }
        
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
        public void CanConstruct_FromEntities()
        {
            
            var pt = new MeshPoint(FlatTriangle.Faces[0],new Point3d(0.4,0.5,0.6));
            Assert.Equal(0, pt.FaceIndex);
            Assert.NotNull(pt);
        }
        [Fact]
        public void CanConvert_ToString()
        {
            var pt = new MeshPoint(1, 0.4, 0.5, 0.6);
            Assert.NotNull(pt.ToString());
        }
    }
}