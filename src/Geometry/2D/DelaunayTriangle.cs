using System;
using System.Collections.Generic;

namespace Paramdigma.Core.Geometry
{
    public class DelaunayTriangle
    {

        public List<DelaunayPoint> Vertices = new List<DelaunayPoint>();

        public DelaunayPoint Circumcenter;
        public double RadiusSquared;

        public List<DelaunayTriangle> TrianglesWithSharedEdges()
        {
            List<DelaunayTriangle> neighbours = new List<DelaunayTriangle>();
            foreach (DelaunayPoint vertex in Vertices)
            {
                foreach (DelaunayTriangle sharedTriangle in vertex.AdjacentTriangles)
                {
                    if (this.SharesEdgeWidth(sharedTriangle) && neighbours.Contains(sharedTriangle) == false && sharedTriangle != this) neighbours.Add(sharedTriangle);

                }
            }
            return neighbours;
        }
        public DelaunayTriangle(DelaunayPoint point1, DelaunayPoint point2, DelaunayPoint point3)
        {

            Vertices = new List<DelaunayPoint>();
            if (!IsCounterClockwise(point1, point2, point3))
            {
                Vertices.Add(point1);
                Vertices.Add(point3);
                Vertices.Add(point2);
            }
            else
            {
                Vertices.Add(point1);
                Vertices.Add(point2);
                Vertices.Add(point3);
            }
            Vertices[0].AdjacentTriangles.Add(this);
            Vertices[1].AdjacentTriangles.Add(this);
            Vertices[2].AdjacentTriangles.Add(this);
            UpdateCircumcircle();

        }
        public void UpdateCircumcircle()
        {
            DelaunayPoint p0 = Vertices[0];
            DelaunayPoint p1 = Vertices[1];
            DelaunayPoint p2 = Vertices[2];
            double dA = (p0.X * p0.X) + (p0.Y * p0.Y);
            double dB = (p1.X * p1.X) + (p1.Y * p1.Y);
            double dC = (p2.X * p2.X) + (p2.Y * p2.Y);

            double aux1 = ((dA * (p2.Y - p1.Y)) + (dB * (p0.Y - p2.Y)) + (dC * (p1.Y - p0.Y)));
            double aux2 = -((dA * (p2.X - p1.X)) + (dB * (p0.X - p2.X)) + (dC * (p1.X - p0.X)));
            double div = (2 * ((p0.X * (p2.Y - p1.Y)) + (p1.X * (p0.Y - p2.Y)) + (p2.X * (p1.Y - p0.Y))));

            if (div == 0)
            {
                throw new System.Exception();
            }

            DelaunayPoint center = new DelaunayPoint(aux1 / div, aux2 / div);
            Circumcenter = center;
            RadiusSquared = ((center.X - p0.X) * (center.X - p0.X)) + ((center.Y - p0.Y) * (center.Y - p0.Y));
        }

        public bool IsCounterClockwise(DelaunayPoint point1, DelaunayPoint point2, DelaunayPoint point3)
        {
            double result = ((point2.X - point1.X) * (point3.Y - point1.Y)) -
                ((point3.X - point1.X) * (point2.Y - point1.Y));
            return result > 0;
        }

        public bool SharesEdgeWidth(DelaunayTriangle triangle)
        {
            int sharedCount = 0;
            foreach (DelaunayPoint vertex in this.Vertices)
            {
                foreach (DelaunayPoint vertex2 in triangle.Vertices)
                {
                    if (vertex == vertex2) sharedCount++;
                }
            }
            return sharedCount == 2;

            throw new NotImplementedException();
        }
        public bool IsPointInsideCircumcircle(DelaunayPoint point)
        {
            double d_squared = ((point.X - Circumcenter.X) * (point.X - Circumcenter.X)) +
                            ((point.Y - Circumcenter.Y) * (point.Y - Circumcenter.Y));
            return d_squared < RadiusSquared;
        }
    }
}
