using System.Collections.Generic;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    ///     Class holding all the delaunay and vornoi classes in 2 dimensions.
    /// </summary>
    public static class Delaunay
    {
        public static List<DelaunayTriangle> Compute(List<DelaunayPoint> points, List<DelaunayTriangle> border)
        {
            var triangulation = new List<DelaunayTriangle>(border);
            points.Reverse();
            foreach (var point in points)
            {
                var badTriangles = FindBadTriangles(point, triangulation);
                var polygon = FindHoleBoundaries(badTriangles);
                foreach (var triangle in badTriangles)
                {
                    foreach (var vertex in triangle.Vertices)
                        vertex.AdjacentTriangles.Remove(triangle);
                    if (triangulation.Contains(triangle)) triangulation.Remove(triangle);
                }

                foreach (var edge in polygon)
                {
                    var triangle = new DelaunayTriangle(point, edge.StartPoint, edge.EndPoint);
                    triangulation.Add(triangle);
                }
            }

            return triangulation;
        }

        public static List<DelaunayEdge> Voronoi(List<DelaunayTriangle> triangulation)
        {
            var voronoiEdges = new List<DelaunayEdge>();
            foreach (var triangle in triangulation)
            foreach (var neigbour in triangle.TrianglesWithSharedEdges())
            {
                var edge = new DelaunayEdge(triangle.Circumcenter, neigbour.Circumcenter);
                voronoiEdges.Add(edge);
            }

            return voronoiEdges;
        }

        private static List<DelaunayEdge> FindHoleBoundaries(List<DelaunayTriangle> badTriangles)
        {
            var boundaryEdges = new List<DelaunayEdge>();
            var duplicateEdges = new List<DelaunayEdge>();
            foreach (var triangle in badTriangles)
            {
                var e = new DelaunayEdge(triangle.Vertices[0], triangle.Vertices[1]);
                if (!boundaryEdges.Contains(e))
                    boundaryEdges.Add(e);
                else
                    duplicateEdges.Add(e);
                var f = new DelaunayEdge(triangle.Vertices[1], triangle.Vertices[2]);
                if (!boundaryEdges.Contains(f))
                    boundaryEdges.Add(f);
                else
                    duplicateEdges.Add(f);
                var j = new DelaunayEdge(triangle.Vertices[2], triangle.Vertices[0]);
                if (!boundaryEdges.Contains(j))
                    boundaryEdges.Add(j);
                else
                    duplicateEdges.Add(j);
            }

            for (var i = boundaryEdges.Count - 1; i >= 0; i--)
            {
                var e = boundaryEdges[i];
                if (duplicateEdges.Contains(e))
                    boundaryEdges.Remove(e);
            }

            return boundaryEdges;
        }

        private static List<DelaunayTriangle> FindBadTriangles(DelaunayPoint point, List<DelaunayTriangle> triangles)
        {
            var badTriangles = new List<DelaunayTriangle>();
            foreach (var triangle in triangles)
                if (triangle.IsPointInsideCircumcircle(point))
                    badTriangles.Add(triangle);
            return badTriangles;
        }
    }
}