using System.Collections.Generic;
using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    
    
    public class MeshCornerTests
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
        public void HasPropertiesAssigned()
        {
            FlatTriangle.Corners.ForEach(
                corner =>
                {
                    Assert.NotNull(corner.Vertex);
                    Assert.NotNull(corner.Face);
                    Assert.NotNull(corner.Next);
                    Assert.NotNull(corner.Prev);
                    Assert.NotEqual(corner.Index, -1);
                });
        }
    }
}