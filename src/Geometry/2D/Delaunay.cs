using System.Collections.Generic;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    /// Class holding all the delaunay and vornoi classes in 2 dimensions.
    /// </summary>
    public static class Delaunay
    {
        
        public static List<DelaunayTriangle> Compute(List<DelaunayPoint> points, List<DelaunayTriangle> border)
        {
            List<DelaunayTriangle> triangulation = new List<DelaunayTriangle>(border);
            points.Reverse();
            foreach (DelaunayPoint point in points)
            {
                List<DelaunayTriangle> badTriangles = FindBadTriangles(point, triangulation);
                List<DelaunayEdge> polygon = FindHoleBoundaries(badTriangles);
                foreach (DelaunayTriangle triangle in badTriangles)
                {
                    foreach (DelaunayPoint vertex in triangle.Vertices)
                    {
                        vertex.AdjacentTriangles.Remove(triangle);
                    }
                    if (triangulation.Contains(triangle)) triangulation.Remove(triangle);
                }
                foreach (DelaunayEdge edge in polygon)
                {
                    DelaunayTriangle triangle = new DelaunayTriangle(point, edge.StartPoint, edge.EndPoint);
                    triangulation.Add(triangle);
                }
            }
            return triangulation;
        }

        public static List<DelaunayEdge> Voronoi(List<DelaunayTriangle> triangulation)
        {
            List<DelaunayEdge> voronoiEdges = new List<DelaunayEdge>();
            foreach (DelaunayTriangle triangle in triangulation)
            {
                foreach (DelaunayTriangle neigbour in triangle.TrianglesWithSharedEdges())
                {
                    DelaunayEdge edge = new DelaunayEdge(triangle.Circumcenter, neigbour.Circumcenter);
                    voronoiEdges.Add(edge);

                }
            }
            return voronoiEdges;
        }

        private static List<DelaunayEdge> FindHoleBoundaries(List<DelaunayTriangle> badTriangles)
        {
            List<DelaunayEdge> boundaryEdges = new List<DelaunayEdge>();
            List<DelaunayEdge> duplicateEdges = new List<DelaunayEdge>();
            foreach (DelaunayTriangle triangle in badTriangles)
            {
                DelaunayEdge e = new DelaunayEdge(triangle.Vertices[0], triangle.Vertices[1]);
                if (!boundaryEdges.Contains(e))
                    boundaryEdges.Add(e);
                else
                    duplicateEdges.Add(e);
                DelaunayEdge f = new DelaunayEdge(triangle.Vertices[1], triangle.Vertices[2]);
                if (!boundaryEdges.Contains(f))
                    boundaryEdges.Add(f);
                else
                    duplicateEdges.Add(f);
                DelaunayEdge j = new DelaunayEdge(triangle.Vertices[2], triangle.Vertices[0]);
                if (!boundaryEdges.Contains(j))
                    boundaryEdges.Add(j);
                else
                    duplicateEdges.Add(j);
            }

            for (int i = boundaryEdges.Count - 1; i >= 0; i--)
            {
                DelaunayEdge e = boundaryEdges[i];
                if (duplicateEdges.Contains(e))
                    boundaryEdges.Remove(e);
            }
            return boundaryEdges;
        }

        private static List<DelaunayTriangle> FindBadTriangles(DelaunayPoint point, List<DelaunayTriangle> triangles)
        {
            List<DelaunayTriangle> badTriangles = new List<DelaunayTriangle>();
            foreach (DelaunayTriangle triangle in triangles)
            {
                if (triangle.IsPointInsideCircumcircle(point)) badTriangles.Add(triangle);
            }
            return badTriangles;
        }

    }
}
