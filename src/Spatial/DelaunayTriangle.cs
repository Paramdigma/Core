using System;
using System.Collections.Generic;
using System.Linq;
using Paramdigma.Core.Geometry;

namespace Paramdigma.Core.Spatial
{
    /// <summary>
    ///     Represents a triangle in a delaunay triangulation operation.
    /// </summary>
    public class DelaunayTriangle
    {
        /// <summary>
        ///     Circumcenter of this triangle.
        /// </summary>
        public Point2d Circumcenter;

        /// <summary>
        ///     Squared radius of the triangle's circumcircle.
        /// </summary>
        public double RadiusSquared;

        /// <summary>
        ///     List of vertices of this triangle.
        /// </summary>
        public readonly List<DelaunayPoint> Vertices;


        /// <summary>
        ///     Initializes a new instance of the <see cref="DelaunayTriangle" /> class.
        /// </summary>
        /// <param name="point1">Point A.</param>
        /// <param name="point2">Point B.</param>
        /// <param name="point3">Point C.</param>
        public DelaunayTriangle(DelaunayPoint point1, DelaunayPoint point2, DelaunayPoint point3)
        {
            this.Vertices = new List<DelaunayPoint>();
            if (!IsCounterClockwise(point1, point2, point3))
            {
                this.Vertices.Add(point1);
                this.Vertices.Add(point3);
                this.Vertices.Add(point2);
            }
            else
            {
                this.Vertices.Add(point1);
                this.Vertices.Add(point2);
                this.Vertices.Add(point3);
            }

            this.Vertices[0].AdjacentTriangles.Add(this);
            this.Vertices[1].AdjacentTriangles.Add(this);
            this.Vertices[2].AdjacentTriangles.Add(this);
            this.UpdateCircumcircle();
        }


        /// <summary>
        ///     Returns a collection of neighbouring triangles.
        /// </summary>
        /// <returns>Collection of triangles.</returns>
        public IEnumerable<DelaunayTriangle> TrianglesWithSharedEdges()
        {
            var neighbours = new List<DelaunayTriangle>();
            foreach (var sharedTriangle in
                from vertex in this.Vertices
                from sharedTriangle in vertex.AdjacentTriangles
                where this.SharesEdgeWidth(sharedTriangle)
                   && neighbours.Contains(sharedTriangle) == false
                   && sharedTriangle != this
                select sharedTriangle)
                neighbours.Add(sharedTriangle);

            return neighbours;
        }


        /// <summary>
        ///     Checks if a point is inside this triangle's circumcircle.
        /// </summary>
        /// <param name="point">Point2d to check.</param>
        /// <returns>True if inside.</returns>
        public bool IsPointInsideCircumcircle(Point2d point)
        {
            var dSquared = (point.X - this.Circumcenter.X) * (point.X - this.Circumcenter.X)
                         + (point.Y - this.Circumcenter.Y) * (point.Y - this.Circumcenter.Y);
            return dSquared < this.RadiusSquared;
        }


        private static bool IsCounterClockwise(Point2d point1, Point2d point2, Point2d point3)
        {
            var result = (point2.X - point1.X) * (point3.Y - point1.Y)
                       - (point3.X - point1.X) * (point2.Y - point1.Y);
            return result > 0;
        }


        private void UpdateCircumcircle()
        {
            var p0 = this.Vertices[0];
            var p1 = this.Vertices[1];
            var p2 = this.Vertices[2];
            var dA = p0.X * p0.X + p0.Y * p0.Y;
            var dB = p1.X * p1.X + p1.Y * p1.Y;
            var dC = p2.X * p2.X + p2.Y * p2.Y;

            var aux1 = dA * (p2.Y - p1.Y)
                     + dB * (p0.Y - p2.Y)
                     + dC * (p1.Y - p0.Y);
            var aux2 = -(dA * (p2.X - p1.X)
                       + dB * (p0.X - p2.X)
                       + dC * (p1.X - p0.X));
            var div = 2 * (p0.X * (p2.Y - p1.Y)
                         + p1.X * (p0.Y - p2.Y)
                         + p2.X * (p1.Y - p0.Y));

            if (Math.Abs(div) < Settings.Tolerance)
                throw new Exception("Divisor too small");

            var center = new DelaunayPoint(aux1 / div, aux2 / div);
            this.Circumcenter = center;
            this.RadiusSquared = (center.X - p0.X) * (center.X - p0.X)
                               + (center.Y - p0.Y) * (center.Y - p0.Y);
        }


        private bool SharesEdgeWidth(DelaunayTriangle triangle)
        {
            var shared =
                from pt1 in this.Vertices
                from pt2 in triangle.Vertices
                where pt1 == pt2
                select pt1;
            return shared.Count() == 2;
        }
    }
}