using System.Collections.Generic;

namespace Paramdigma.Core.Geometry
{
    public class DelaunayPoint : Point2d
    {
        public List<DelaunayTriangle> AdjacentTriangles;

        public DelaunayPoint(double x, double y) : base(x, y)
        {
            AdjacentTriangles = new List<DelaunayTriangle>();
        }
    }
}
