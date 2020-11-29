using System.Collections.Generic;
using Paramdigma.Core.Geometry;
using Paramdigma.Core.HalfEdgeMesh;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    public class MeshTopologyTests
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
        public void CanCreate_MeshTopology()
        {
            // TODO: Improve this tests with better assertions.
            var topo = new MeshTopology(FlatSquare);
            
            Assert.Empty(topo.FaceFace);
        }
        
        [Fact]
        public void CanConvert_ToString()
        {
            // TODO: Improve this tests with better assertions.
            var topo = new MeshTopology(FlatSquare);
            
            Assert.NotNull(topo.TopologyDictToString(topo.FaceFace));
            Assert.NotNull(topo.TopologyDictToString(topo.VertexVertex));
            Assert.NotNull(topo.TopologyDictToString(topo.EdgeEdge));
        }
    }
}