using System;
using MathNet.Numerics.LinearAlgebra.Double;
using Paramdigma.Core.DiscreteDifferentialGeometry;
using Paramdigma.Core.HalfEdgeMesh;
using Paramdigma.Core.IO;
using Xunit;

namespace Paramdigma.Core.Tests.DiscreteDifferentialGeometry
{
    public class HeatMethodTests
    {
        [Theory]
        [InlineData("../../../../tests/Data/meshes/bunnyGood.off")]
        public void CanCompute_HeatMethod(string path)
        {
            OFFReader.ReadMeshFromFile(path, out var data);
            var mesh = new Mesh(data.Vertices,data.Faces);
            var heat = new HeatMethod(mesh);
            var init = DenseMatrix.Create(data.Vertices.Count, 1,0);
            init[5, 0] = 1;
            var result = heat.Compute(init);
            Console.WriteLine(result.ToArray());
        }
    }
}