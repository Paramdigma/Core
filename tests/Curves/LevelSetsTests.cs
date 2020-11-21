using System.Collections.Generic;
using Paramdigma.Core.Curves;
using Paramdigma.Core.Geometry;
using Paramdigma.Core.HalfEdgeMesh;
using Xunit;

namespace Paramdigma.Core.Tests.Curves
{
    public class LevelSetsTests
    {
        public Mesh Triangle
        {
            get
            {
                var key = "scalar-1";
                var ptA = new MeshVertex(0, 0, 0);
                ptA.UserValues[key] = 0;
                var ptB = new MeshVertex(1, 0, 0);
                ptB.UserValues[key] = 0;
                var ptC = new MeshVertex(0.5, 1, 1);
                ptC.UserValues[key] = 1;
                var vertices = new List<MeshVertex> {ptA, ptB, ptC};
                var face = new List<int> {0, 1, 2};
                var mesh = new Mesh(vertices, new List<List<int>> {face});
                return mesh;
            }
        }


        [Fact]
        public void CanCompute_GradientInFace_ReturnsValidVector()
        {
            var c = LevelSets.ComputeGradientField("scalar-1", this.Triangle);
            Assert.Equal(this.Triangle.Faces.Count, c.Count);
            Assert.Equal(new Vector3d(0, -1, -1).Unit(), c[0].Unit());
        }


        [Fact]
        public void CanCompute_LevelInFace_ReturnsValidLine()
        {
            LevelSets.ComputeLevels(
                "scalar-1",
                new List<double> {0.5},
                this.Triangle,
                out var levelSets);
            Assert.NotEmpty(levelSets);
            var v = ( Vector3d ) levelSets[0][0];
            Assert.Equal(new Vector3d(1, 0, 0), v.Unit());
        }
    }
}