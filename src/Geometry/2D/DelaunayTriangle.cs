using System;
using System.Collections.Generic;

namespace Paramdigma.Core.Geometry
{
    public class DelaunayTriangle
    {
        public DelaunayPoint Circumcenter;
        public double RadiusSquared;

        public List<DelaunayPoint> Vertices = new List<DelaunayPoint>();

        public DelaunayTriangle(DelaunayPoint point1, DelaunayPoint point2, DelaunayPoint point3)
        {
            this.Vertices = new List<DelaunayPoint>();
            if (!this.IsCounterClockwise(point1, point2, point3))
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

        public List<DelaunayTriangle> TrianglesWithSharedEdges()
        {
            var neighbours = new List<DelaunayTriangle>();
            foreach (var vertex in this.Vertices)
            foreach (var sharedTriangle in vertex.AdjacentTriangles)
                if (this.SharesEdgeWidth(sharedTriangle) && neighbours.Contains(sharedTriangle) == false && sharedTriangle != this)
                    neighbours.Add(sharedTriangle);
            return neighbours;
        }

        public void UpdateCircumcircle()
        {
            var p0 = this.Vertices[0];
            var p1 = this.Vertices[1];
            var p2 = this.Vertices[2];
            var dA = (p0.X * p0.X) + (p0.Y * p0.Y);
            var dB = (p1.X * p1.X) + (p1.Y * p1.Y);
            var dC = (p2.X * p2.X) + (p2.Y * p2.Y);

            var aux1 = (dA * (p2.Y - p1.Y)) + (dB * (p0.Y - p2.Y)) + (dC * (p1.Y - p0.Y));
            var aux2 = -((dA * (p2.X - p1.X)) + (dB * (p0.X - p2.X)) + (dC * (p1.X - p0.X)));
            var div = 2 * ((p0.X * (p2.Y - p1.Y)) + (p1.X * (p0.Y - p2.Y)) + (p2.X * (p1.Y - p0.Y)));

            if (div == 0)
                throw new Exception();

            var center = new DelaunayPoint(aux1 / div, aux2 / div);
            this.Circumcenter = center;
            this.RadiusSquared = ((center.X - p0.X) * (center.X - p0.X)) + ((center.Y - p0.Y) * (center.Y - p0.Y));
        }

        public bool IsCounterClockwise(DelaunayPoint point1, DelaunayPoint point2, DelaunayPoint point3)
        {
            var result = ((point2.X - point1.X) * (point3.Y - point1.Y)) -
                         ((point3.X - point1.X) * (point2.Y - point1.Y));
            return result > 0;
        }

        public bool SharesEdgeWidth(DelaunayTriangle triangle)
        {
            var sharedCount = 0;
            foreach (var vertex in this.Vertices)
            foreach (var vertex2 in triangle.Vertices)
                if (vertex == vertex2)
                    sharedCount++;
            return sharedCount == 2;

            throw new NotImplementedException();
        }

        public bool IsPointInsideCircumcircle(DelaunayPoint point)
        {
            var d_squared = ((point.X - this.Circumcenter.X) * (point.X - this.Circumcenter.X)) +
                            ((point.Y - this.Circumcenter.Y) * (point.Y - this.Circumcenter.Y));
            return d_squared < this.RadiusSquared;
        }
    }
}