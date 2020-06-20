using System.Collections.Generic;
using Paramdigma.Core.Geometry;
using Paramdigma.Core.HalfEdgeMesh;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry._3D
{
    public class MeshVertexTests
    {
        public Mesh FlatSquare
        {
            get
            {
                var ptA = new Point3d(0, 0, 0);
                var ptB = new Point3d(1, 0, 0);
                var ptC = new Point3d(1, 1, 0);
                var ptD = new Point3d(0, 1, 0);
                var vertices = new List<Point3d> {ptA, ptB, ptC, ptD};
                var face = new List<int> {0, 1, 2, 3};
                var mesh = new Mesh(vertices, new List<List<int>> {face});
                return mesh;
            }
        }

        [Fact]
        public void CanCompute_Adjacencies() =>
            this.FlatSquare.Vertices.ForEach(vertex =>
            {
                Assert.Single(vertex.AdjacentFaces());
                Assert.Single(vertex.AdjacentCorners());
                Assert.Equal(2, vertex.AdjacentVertices().Count);
                Assert.Equal(2, vertex.AdjacentEdges().Count);
                Assert.Equal(2, vertex.AdjacentHalfEdges().Count);
                Assert.Equal(2, vertex.Valence());
                Assert.True(vertex.OnBoundary());
            });

        [Fact]
        public void CanConvert_ToString() =>
            this.FlatSquare.Vertices.ForEach(vertex =>
            {
                Assert.NotNull(vertex.ToString());
            });

        [Fact]
        public void CanCreate()
        {
            Assert.NotNull(new MeshVertex());
            Assert.NotNull(new MeshVertex(Point3d.WorldOrigin));
        }
    }
}