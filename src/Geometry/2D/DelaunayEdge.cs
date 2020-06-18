namespace Paramdigma.Core.Geometry
{
    /// <summary>
    /// Represents a connection between two points in a Delaunay triangulation.
    /// </summary>
    public class DelaunayEdge
    {
        /// <summary>
        /// The edge's start point.
        /// </summary>
        public DelaunayPoint StartPoint;

        /// <summary>
        /// The edge's end point.
        /// </summary>
        public DelaunayPoint EndPoint;

        /// <summary>
        /// Initializes a new instance of the <see cref="DelaunayEdge"/> class.
        /// </summary>
        /// <param name="startPoint">Start point.</param>
        /// <param name="endPoint">End point.</param>
        public DelaunayEdge(DelaunayPoint startPoint, DelaunayPoint endPoint)
        {
            StartPoint = startPoint;
            EndPoint = endPoint;
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is DelaunayEdge edge))
                return false;
            var samePoints = StartPoint == edge.StartPoint && EndPoint == edge.EndPoint;
            var samePointsReversed = StartPoint == edge.EndPoint && EndPoint == edge.StartPoint;
            return samePoints || samePointsReversed;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            int hCode = (int)StartPoint.X ^ (int)StartPoint.Y ^ (int)EndPoint.X ^ (int)EndPoint.Y;
            return hCode.GetHashCode();
        }
    }
}