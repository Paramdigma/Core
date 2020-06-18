using System.Collections.Generic;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    /// Represents a point in a delaunay triangulation with adjacency information.
    /// </summary>
    public class DelaunayPoint : Point2d
    {
        /// <summary>
        /// List of adjacent triangles of this point.
        /// </summary>
        public List<DelaunayTriangle> AdjacentTriangles;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelaunayPoint"/> class from it's coordinates.
        /// </summary>
        /// <param name="x">X Coordinate.</param>
        /// <param name="y">Y Coordinate.</param>
        public DelaunayPoint(double x, double y) : base(x, y)
        {
            AdjacentTriangles = new List<DelaunayTriangle>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DelaunayPoint"/> class from a <see cref="Point2d"/> instance.
        /// </summary>
        /// <param name="point">Point to create <see cref="DelaunayPoint"/> from.</param>
        public DelaunayPoint(Point2d point) : this(point.X, point.Y) { }
    }
}