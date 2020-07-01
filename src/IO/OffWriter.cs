using System.IO;
using System.Linq;
using Paramdigma.Core.HalfEdgeMesh;

#pragma warning disable 1591

namespace Paramdigma.Core.IO
{
    /// <summary>OFF format writer class.</summary>
    public static class OffWriter
    {
        /// <summary>
        ///     Write a Half-Edge mesh to a .OFF file.
        /// </summary>
        /// <param name="mesh">Half-edge mesh to export.</param>
        /// <param name="filePath">Path to save the file to.</param>
        /// <returns></returns>
        public static OffResult WriteMeshToFile(Mesh mesh, string filePath)
        {
            var offLines = new string[mesh.Vertices.Count + mesh.Faces.Count + 2];

            const string offHead = "OFF";
            offLines[0] = offHead;
            var offCount = mesh.Vertices.Count + " " + mesh.Faces.Count + " 0";
            offLines[1] = offCount;

            var count = 2;
            foreach (var vertex in mesh.Vertices)
            {
                var vText = vertex.X + " " + vertex.Y + " " + vertex.Z;
                offLines[count] = vText;
                count++;
            }

<<<<<<< develop:src/IO/OffWriter.cs
            foreach (var face in mesh.Faces)
            {
                if (!face.IsBoundaryLoop())
                {
                    var vertices = face.AdjacentVertices();
                    var faceString = vertices.Count.ToString();

                    foreach (var v in face.AdjacentVertices())
                        faceString = faceString + " " + v.Index;

                    offLines[count] = faceString;
                    count++;
                }
            }

            File.WriteAllLines(filePath, offLines);
            return OffResult.Ok;
=======
            foreach (string faceString in from face in mesh.Faces
                                          where !face.IsBoundaryLoop()
                                          let vertices = face.AdjacentVertices()
                                          let faceString = vertices.Count.ToString()
                                          select face.AdjacentVertices().Aggregate(faceString, (current, v) => current + " " + v.Index))
            {
                offLines[count] = faceString;
                count++;
            }

            File.WriteAllLines(filePath, offLines);
            return OFFResult.Ok;
>>>>>>> Big warning cleanup:src/IO/OFFWritter.cs
        }
    }
}