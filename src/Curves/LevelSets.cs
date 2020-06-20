using System.Collections.Generic;
using Paramdigma.Core.Geometry;
using Paramdigma.Core.HalfEdgeMesh;

namespace Paramdigma.Core.Curves
{
    /// <summary>
    ///     Compute the level sets of a given function.
    /// </summary>
    public static class LevelSets
    {
        /// <summary>
        ///     Compute the level-sets for a mesh given a specified valueKey for the mesh vertex dictionary.
        /// </summary>
        /// <param name="valueKey">Key of the value to be computed per vertex.</param>
        /// <param name="levels">List of level values to be computed.</param>
        /// <param name="mesh">The mesh to compute the level-sets in.</param>
        /// <param name="levelSets">Resulting level sets.</param>
        public static void ComputeLevels(string valueKey, List<double> levels, Mesh mesh, out List<List<Line>> levelSets)
        {
            var resultLines = new List<List<Line>>();

            for (var i = 0; i < levels.Count; i++)
                resultLines.Add(new List<Line>());

            var iter = 0;
            foreach (var face in mesh.Faces)
            {
                var count = 0;
                foreach (var level in levels)
                {
                    if (GetFaceLevel(valueKey, level, face, out var l))
                        resultLines[count].Add(l);

                    count++;
                }

                iter++;
            }

            levelSets = resultLines;
        }

        /// <summary>
        ///     Compute the level on a specified face.
        /// </summary>
        /// <param name="valueKey">Key of the value to be computed per vertex.</param>
        /// <param name="level">Level value to be computed.</param>
        /// <param name="face">Face to compute the level in.</param>
        /// <param name="line">Resulting level line on the face.</param>
        /// <returns>True if successful, false if not.</returns>
        public static bool GetFaceLevel(string valueKey, double level, MeshFace face, out Line line)
        {
            var adj = face.AdjacentVertices();
            var vertexValues = new List<double> {adj[0].UserValues[valueKey], adj[1].UserValues[valueKey], adj[2].UserValues[valueKey]};

            var above = new List<int>();
            var below = new List<int>();

            for (var i = 0; i < vertexValues.Count; i++)
                if (vertexValues[i] < level)
                    below.Add(i);
                else
                    above.Add(i);

            if (above.Count == 3 || below.Count == 3)
            {
                // Triangle is above or below level
                line = new Line(new Point3d(), new Point3d());
                return false;
            }

            // Triangle intersects level
            var intersectionPoints = new List<Point3d>();

            foreach (var i in above)
            foreach (var j in below)
            {
                var diff = vertexValues[i] - vertexValues[j];
                var desiredDiff = level - vertexValues[j];
                var unitizedDistance = desiredDiff / diff;
                var edgeV = adj[i] - adj[j];
                var levelPoint = adj[j] + (edgeV * unitizedDistance);
                intersectionPoints.Add(levelPoint);
            }

            line = new Line(intersectionPoints[0], intersectionPoints[1]);
            return true;
        }

        /// <summary>
        ///     Compute the gradient on a given mesh given some per-vertex values.
        /// </summary>
        /// <param name="valueKey">Key of the values in the vertex.UserData dictionary.</param>
        /// <param name="mesh">Mesh to compute the gradient.</param>
        /// <returns>A list containing all the gradient vectors per-face.</returns>
        public static List<Vector3d> ComputeGradientField(string valueKey, Mesh mesh)
        {
            var gradientField = new List<Vector3d>();

            mesh.Faces.ForEach(face => gradientField.Add(ComputeFaceGradient(valueKey, face)));

            return gradientField;
        }

        /// <summary>
        ///     Compute the gradient on a given mesh face given some per-vertex values.
        /// </summary>
        /// <param name="valueKey">Key of the values in the vertex.UserData dictionary.</param>
        /// <param name="face">Face to compute thee gradient.</param>
        /// <returns>A vector representing the gradient on that mesh face.</returns>
        public static Vector3d ComputeFaceGradient(string valueKey, MeshFace face)
        {
            var adjacentVertices = face.AdjacentVertices();
            Point3d i = adjacentVertices[0];
            Point3d j = adjacentVertices[1];
            Point3d k = adjacentVertices[2];

            var gi = adjacentVertices[0].UserValues[valueKey];
            var gj = adjacentVertices[1].UserValues[valueKey];
            var gk = adjacentVertices[2].UserValues[valueKey];

            var faceNormal = face.Normal / (2 * face.Area);
            var rotatedGradient = ((gi * (k - j)) + (gj * (i - k)) + (gk * (j - i))) / (2 * face.Area);
            var gradient = rotatedGradient.Cross(faceNormal);

            return gradient;
        }
    }
}