using System;
using MathNet.Numerics.LinearAlgebra.Double;
using Paramdigma.Core.DiscreteDifferentialGeometry;
using Paramdigma.Core.Geometry;
using Paramdigma.Core.HalfEdgeMesh;
using Paramdigma.Core.IO;
using Xunit;
using Xunit.Abstractions;

namespace Paramdigma.Core.Tests.DiscreteDifferentialGeometry
{
    public class HeatMethodTests
    {
        private readonly ITestOutputHelper testOutputHelper;

        public HeatMethodTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        [Theory]
        [InlineData("../../../../tests/Data/meshes/sphere.off")]
        public void CanCompute_HeatMethod(string path)
        {

            
            var indices = new int[] {2, 5, 9};
            
            OFFReader.ReadMeshFromFile(path, out var data);
            var mesh = new Mesh(data.Vertices,data.Faces);
            MeshGeometry.Normalize(mesh,true);
            var heat = new HeatMethod(mesh);
            var init = DenseMatrix.Create(data.Vertices.Count, 1,0);
            foreach (var index in indices)
            {
                init[index, 0] = 1;
            }
            var result = heat.Compute(init);
            this.testOutputHelper.WriteLine(result.ToString());
        }
    }
}