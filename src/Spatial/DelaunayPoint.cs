using System.Collections.Generic;
using Paramdigma.Core.Geometry;

namespace Paramdigma.Core.Spatial
{
    /// <summary>
    ///     Represents a point in a delaunay triangulation with adjacency information.
    /// </summary>
    public class DelaunayPoint : Point2d
    {
        /// <summary>
        ///     List of adjacent triangles of this point.
        /// </summary>
        public readonly List<DelaunayTriangle> AdjacentTriangles;


        /// <summary>
        ///     Initializes a new instance of the <see cref="DelaunayPoint" /> class from it's coordinates.
        /// </summary>
        /// <param name="x">X Coordinate.</param>
        /// <param name="y">Y Coordinate.</param>
        public DelaunayPoint(double x, double y) : base(x, y) =>
            this.AdjacentTriangles = new List<DelaunayTriangle>();


        /// <summary>
        ///     Initializes a new instance of the <see cref="DelaunayPoint" /> class from a
        ///     <see cref="Point2d" /> instance.
        /// </summary>
        /// <param name="point">Point to create <see cref="DelaunayPoint" /> from.</param>
        public DelaunayPoint(Point2d point) : base(point.X, point.Y) { }

    }
}