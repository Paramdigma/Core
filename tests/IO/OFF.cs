using System;
using Xunit;
using Paramdigma.Core.IO;
namespace Paramdigma.Core.Tests.IO
{
    public class OFF
    {
        [Theory]
        [InlineData("../../../../tests/Data/meshes/cube.off")]
        [InlineData("../../../../tests/Data/meshes/sphere.off")]
        [InlineData("../../../../tests/Data/meshes/parabolicCyclide.off")]
        [InlineData("../../../../tests/Data/meshes/bunny.off")]
        public void CanRead_OffFile(string filePath)
        {
            OFFReader.ReadMeshFromFile(filePath, out var data);
            Console.WriteLine(data);
        }
    }
}