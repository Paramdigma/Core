using System.Collections.Generic;
using Paramdigma.Core.Geometry;

#pragma warning disable 1591

namespace Paramdigma.Core.IO
{
    public struct OBJMeshData
    {
        public OBJMeshData(
            List<Point3d> vertices,
            List<List<int>> faces,
            List<List<int>> edges,
            List<List<double>> textureCoords,
            List<List<int>> faceTextureCoords,
            List<Vector3d> normals)
        {
            this.Vertices = vertices;
            this.Faces = faces;
            this.Edges = edges;
            this.TextureCoords = textureCoords;
            this.FaceTextureCoords = faceTextureCoords;
            this.Normals = normals;
        }


        public List<Point3d> Vertices { get; }

        public List<List<int>> Faces { get; }

        public List<List<int>> Edges { get; }

        public List<List<double>> TextureCoords { get; }

        public List<List<int>> FaceTextureCoords { get; }

        public List<Vector3d> Normals { get; }
    }
}