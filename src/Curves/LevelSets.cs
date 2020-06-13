using System.Collections.Generic;
using Paramdigma.Core.Geometry;
using Paramdigma.Core.HalfEdgeMesh;

namespace Paramdigma.Core.Curves
{
    /// <summary>
    /// Compute the level sets of a given function.
    /// </summary>
    public static class LevelSets
    {
        /// <summary>
        /// Compute the level-sets for a mesh given a specified valueKey for the mesh vertex dictionary.
        /// </summary>
        /// <param name="valueKey">Key of the value to be computed per vertex.</param>
        /// <param name="levels">List of level values to be computed.</param>
        /// <param name="mesh">The mesh to compute the level-sets in.</param>
        /// <param name="levelSets">Resulting level sets.</param>
        public static void ComputeLevels(string valueKey, List<double> levels, Mesh mesh, out List<List<Line>> levelSets)
        {
            List<List<Line>> resultLines = new List<List<Line>>();

            for (int i = 0; i < levels.Count; i++)
            {
                resultLines.Add(new List<Line>());
            }

            int iter = 0;
            foreach (MeshFace face in mesh.Faces)
            {
                int count = 0;
                foreach (double level in levels)
                {
                    if (GetFaceLevel(valueKey, level, face, out var l))
                    {
                        resultLines[count].Add(l);
                    }

                    count++;
                }

                iter++;
            }

            levelSets = resultLines;
        }

        /// <summary>
        /// Compute the level on a specified face.
        /// </summary>
        /// <param name="valueKey">Key of the value to be computed per vertex.</param>
        /// <param name="level">Level value to be computed.</param>
        /// <param name="face">Face to compute the level in.</param>
        /// <param name="line">Resulting level line on the face.</param>
        /// <returns>True if successful, false if not.</returns>
        public static bool GetFaceLevel(string valueKey, double level, MeshFace face, out Line line)
        {
            List<MeshVertex> adj = face.AdjacentVertices();
            List<double> vertexValues = new List<double> { adj[0].UserValues[valueKey], adj[1].UserValues[valueKey], adj[2].UserValues[valueKey] };

            List<int> above = new List<int>();
            List<int> below = new List<int>();

            for (int i = 0; i < vertexValues.Count; i++)
            {
                if (vertexValues[i] < level)
                    below.Add(i);
                else
                    above.Add(i);
            }

            if (above.Count == 3 || below.Count == 3)
            {
                // Triangle is above or below level
                line = new Line(new Point3d(), new Point3d());
                return false;
            }

            // Triangle intersects level
            List<Point3d> intersectionPoints = new List<Point3d>();

            foreach (int i in above)
            {
                foreach (int j in below)
                {
                    double diff = vertexValues[i] - vertexValues[j];
                    double desiredDiff = level - vertexValues[j];
                    double unitizedDistance = desiredDiff / diff;
                    Vector3d edgeV = adj[i] - adj[j];
                    Point3d levelPoint = adj[j] + (edgeV * unitizedDistance);
                    intersectionPoints.Add(levelPoint);
                }
            }

            line = new Line(intersectionPoints[0], intersectionPoints[1]);
            return true;
        }

        /// <summary>
        /// Compute the gradient on a given mesh given some per-vertex values.
        /// </summary>
        /// <param name="valueKey">Key of the values in the vertex.UserData dictionary.</param>
        /// <param name="mesh">Mesh to compute the gradient.</param>
        /// <returns>A list containing all the gradient vectors per-face.</returns>
        public static List<Vector3d> ComputeGradientField(string valueKey, Mesh mesh)
        {
            List<Vector3d> gradientField = new List<Vector3d>();

            mesh.Faces.ForEach(face => gradientField.Add(ComputeFaceGradient(valueKey, face)));

            return gradientField;
        }

        /// <summary>
        /// Compute the gradient on a given mesh face given some per-vertex values.
        /// </summary>
        /// <param name="valueKey">Key of the values in the vertex.UserData dictionary.</param>
        /// <param name="face">Face to compute thee gradient.</param>
        /// <returns>A vector representing the gradient on that mesh face.</returns>
        public static Vector3d ComputeFaceGradient(string valueKey, MeshFace face)
        {
            List<MeshVertex> adjacentVertices = face.AdjacentVertices();
            Point3d i = adjacentVertices[0];
            Point3d j = adjacentVertices[1];
            Point3d k = adjacentVertices[2];

            double gi = adjacentVertices[0].UserValues[valueKey];
            double gj = adjacentVertices[1].UserValues[valueKey];
            double gk = adjacentVertices[2].UserValues[valueKey];

            Vector3d faceNormal = face.Normal / (2 * face.Area);
            Vector3d rotatedGradient = ((gi * (k - j)) + (gj * (i - k)) + (gk * (j - i))) / (2 * face.Area);
            Vector3d gradient = rotatedGradient.Cross(faceNormal);

            return gradient;
        }
    }
}