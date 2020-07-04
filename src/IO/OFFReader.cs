using System.Collections.Generic;
using System.IO;
using Paramdigma.Core.Geometry;

#pragma warning disable 1591

namespace Paramdigma.Core.IO
{
    /// <summary>OFF Reader class.</summary>
    public static class OffReader
    {
        public static OffResult ReadMeshFromFile(string filePath, out OffMeshData data)
        {
            var lines = File.ReadAllLines(filePath);
            data = new OffMeshData();

            // Check if first line states OFF format
            if (lines[0] != "OFF")
                return OffResult.IncorrectFormat;

            // Get second line and extract number of vertices and faces
            var initialData = lines[1].Split(' ');
            if (!int.TryParse(initialData[0], out var nVertex))
                return OffResult.IncorrectFormat;

            if (!int.TryParse(initialData[1], out var nFaces))
                return OffResult.IncorrectFormat;

            // Check if length of lines correct
            if (nVertex + nFaces + 2 != lines.Length)
                return OffResult.IncorrectFormat;
            // if (nVertex + nFaces + 2 != lines.Length)
            //     return OFFResult.IncorrectFormat;

            // Iterate through all the lines containing the mesh data
            const int start = 2;
            var vertices = new List<Point3d>();
            var faces = new List<List<int>>();

            for (var i = start; i < lines.Length; i++)
            {
                if (i < nVertex + start)
                {
                    // Extract vertices
                    var coords = new List<double>();

                    // Iterate over the string fragments and convert them to numbers
                    foreach (var ptStr in lines[i].Split(' '))
                    {
                        if (!double.TryParse(ptStr, out var ptCoord))
                            return OffResult.IncorrectVertex;
                        coords.Add(ptCoord);
                    }

                    vertices.Add(new Point3d(coords[0], coords[1], coords[2]));
                }
                else if (i < nVertex + nFaces + start)
                {
                    // Extract faces
                    // In OFF, faces come with a first number determining the number of vertices in that face
                    var vertexIndexes = new List<int>();

                    var faceStrings = lines[i].Split(' ');

                    // Get first int that represents vertex count of face
                    if (!int.TryParse(faceStrings[0], out var _))
                        return OffResult.IncorrectFace;

                    for (var f = 1; f < faceStrings.Length; f++)
                    {
                        if (!int.TryParse(faceStrings[f], out var vertIndex))
                            return OffResult.IncorrectFace;

                        vertexIndexes.Add(vertIndex);
                    }

                    faces.Add(vertexIndexes);
                }
            }

            // Set data output
            data.Vertices = vertices;
            data.Faces = faces;

            return OffResult.Ok;
        }
    }
}