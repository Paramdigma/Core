using System;
using System.Collections.Generic;
using Paramdigma.Core.Geometry;
using Paramdigma.Core.HalfEdgeMesh;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    public class MeshGeometryTests
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
        private void CanCompute_CornerAngles()
        {
            FlatSquare.Corners.ForEach(corner =>
            {
                var angle = MeshGeometry.Angle(corner);
                Assert.True(Math.Abs(angle - (0.5 * Math.PI)) < Settings.Tolerance);
            });
        }

        [Fact]
        private void CanCompute_EdgeLengths()
        {
            FlatSquare.Edges.ForEach(edge =>
            {
                var length = MeshGeometry.Length(edge);
                Assert.True(Math.Abs(length - 1) < Settings.Tolerance);
            });
        }

        [Fact]
        private void CanCompute_FaceArea()
        {
            var area = MeshGeometry.Area(FlatTriangle.Faces[0]);
            Assert.True(Math.Abs(area - 0.5) < Settings.Tolerance);
        }

        [Fact]
        private void CanCompute_MeshArea()
        {
            var area = MeshGeometry.TotalArea(FlatTriangle);
            Assert.True(Math.Abs(area - 0.5) < Settings.Tolerance);
        }

        [Fact]
        private void CanCompute_FaceNormal()
        {
            var normal = MeshGeometry.FaceNormal(FlatTriangle.Faces[0]);
            Assert.Equal(Vector3d.UnitZ,normal);
        }

        [Fact]
        private void CanCompute_VertexNormal()
        {
            FlatTriangle.Vertices.ForEach(vertex =>
            {
                var normal = MeshGeometry.VertexNormalAngleWeighted(vertex);
                Assert.Equal(Vector3d.UnitZ, normal);
                normal = MeshGeometry.VertexNormalEquallyWeighted(vertex);
                Assert.Equal(Vector3d.UnitZ, normal);
                normal = MeshGeometry.VertexNormalAreaWeighted(vertex);
                Assert.Equal(Vector3d.UnitZ, normal);
            });
        }
    }
}