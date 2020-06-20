using System.Collections.Generic;
using Paramdigma.Core.Geometry;

#pragma warning disable 1591

namespace Paramdigma.Core.IO
{
    public struct OBJMeshData
    {
        public List<Point3d> Vertices
        {
            get;
        }

        public List<List<int>> Faces
        {
            get;
        }

        public List<List<int>> Edges
        {
            get;
        }

        public List<List<double>> TextureCoords
        {
            get;
        }

        public List<List<int>> FaceTextureCoords
        {
            get;
        }

        public List<Vector3d> Normals
        {
            get;
        }
    }
}