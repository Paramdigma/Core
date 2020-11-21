using System.Collections.Generic;
using System.Linq;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    ///     Class holding all the delaunay and Voronoi classes in 2 dimensions.
    /// </summary>
    public static class Delaunay
    {
        /// <summary>
        ///     Compute the delaunay triangulation of a given list of points.
        /// </summary>
        /// <param name="points">Points to find delaunay tessellation.</param>
        /// <param name="border">Border to start from.</param>
        /// <returns>List of <see cref="DelaunayTriangle" />.</returns>
        public static IEnumerable<DelaunayTriangle> Compute(
            IEnumerable<DelaunayPoint> points,
            IEnumerable<DelaunayTriangle> border)
        {
            var triangulation = new List<DelaunayTriangle>(border);
            foreach (var point in points)
            {
                var badTriangles = FindBadTriangles(point, triangulation).ToList();
                var polygon = FindHoleBoundaries(badTriangles);
                foreach (var triangle in badTriangles)
                {
                    foreach (var vertex in triangle.Vertices)
                        vertex.AdjacentTriangles.Remove(triangle);

                    if (triangulation.Contains(triangle))
                        triangulation.Remove(triangle);
                }

                triangulation.AddRange(
                    polygon.Select(
                        edge => new DelaunayTriangle(point, edge.StartPoint, edge.EndPoint)));
            }

            return triangulation;
        }


        /// <summary>
        ///     Computes the Voronoi diagram of a given Delaunay triangulation as a list of
        ///     <see cref="DelaunayTriangle" /> instances.
        /// </summary>
        /// <param name="triangulation">Delaunay triangulation.</param>
        /// <returns>Collection of lines representing the Voronoi cells.</returns>
        public static IEnumerable<Line2d> Voronoi(IEnumerable<DelaunayTriangle> triangulation)
            =>
                from triangle in triangulation
                from neighbour in triangle.TrianglesWithSharedEdges()
                select new Line2d(triangle.Circumcenter, neighbour.Circumcenter);


        private static IEnumerable<DelaunayEdge> FindHoleBoundaries(
            IEnumerable<DelaunayTriangle> badTriangles)
        {
            var boundaryEdges = new List<DelaunayEdge>();
            var duplicateEdges = new List<DelaunayEdge>();
            foreach (var triangle in badTriangles)
            {
                for (var i = 0; i < triangle.Vertices.Count; i++)
                {
                    var e = new DelaunayEdge(
                        triangle.Vertices[i],
                        triangle.Vertices[(i + 1) % triangle.Vertices.Count]);
                    if (!boundaryEdges.Contains(e))
                        boundaryEdges.Add(e);
                    else
                        duplicateEdges.Add(e);
                }
            }

            for (var i = boundaryEdges.Count - 1; i >= 0; i--)
            {
                var e = boundaryEdges[i];
                if (duplicateEdges.Contains(e))
                    boundaryEdges.Remove(e);
            }

            return boundaryEdges;
        }


        private static IEnumerable<DelaunayTriangle> FindBadTriangles(
            Point2d point,
            IEnumerable<DelaunayTriangle> triangles)
            => triangles.Where(triangle => triangle.IsPointInsideCircumcircle(point));
    }
}