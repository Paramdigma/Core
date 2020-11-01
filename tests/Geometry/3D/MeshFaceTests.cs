using System.Collections.Generic;
using Paramdigma.Core.Geometry;
using Paramdigma.Core.HalfEdgeMesh;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    public class MeshFaceTests
    {
        private static Mesh FlatSquare
        {
            get
            {
                var ptA = new Point3d(0, 0, 0);
                var ptB = new Point3d(1, 0, 0);
                var ptC = new Point3d(1, 1, 0);
                var ptD = new Point3d(0, 1, 0);
                var vertices = new List<Point3d> {ptA, ptB, ptC, ptD};
                var face1 = new List<int> {0, 1, 3};
                var face2 = new List<int> {3, 1, 2};
                var mesh = new Mesh(vertices, new List<List<int>> {face1, face2});
                return mesh;
            }
        }


        [Fact]
        public void CanCompute_FaceArea() =>
            FlatSquare.Faces.ForEach(face => { Assert.Equal(0.5, face.Area); });


        [Fact]
        public void CanCompute_FaceNormal() =>
            FlatSquare.Faces.ForEach(face => { Assert.Equal(Vector3d.UnitZ, face.Normal); });


        [Fact]
        public void CanCompute_FaceTopology() =>
            FlatSquare.Faces.ForEach(
                face =>
                {
                    Assert.Single(face.AdjacentFaces());
                    Assert.Equal(3, face.AdjacentEdges().Count);
                    Assert.Equal(3, face.AdjacentCorners().Count);
                    Assert.Equal(3, face.AdjacentHalfEdges().Count);
                    Assert.Equal(3, face.AdjacentVertices().Count);
                });


        [Fact]
        public void CanConvert_ToString() =>
            FlatSquare.Faces.ForEach(face => { Assert.NotNull(face.ToString()); });


        [Fact]
        public void CanGet_Index() =>
            FlatSquare.Faces.ForEach(face => { Assert.True(face.Index >= 0); });
    }
}