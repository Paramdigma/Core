namespace Paramdigma.Core.Geometry
{
    /// <summary>
    ///     Represents a connection between two points in a Delaunay triangulation.
    /// </summary>
    public class DelaunayEdge
    {
        /// <summary>
        ///     The edge's end point.
        /// </summary>
        public DelaunayPoint EndPoint;

        /// <summary>
        ///     The edge's start point.
        /// </summary>
        public DelaunayPoint StartPoint;


        /// <summary>
        ///     Initializes a new instance of the <see cref="DelaunayEdge" /> class.
        /// </summary>
        /// <param name="startPoint">Start point.</param>
        /// <param name="endPoint">End point.</param>
        public DelaunayEdge(DelaunayPoint startPoint, DelaunayPoint endPoint)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
        }


        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (!(obj is DelaunayEdge edge))
                return false;
            var samePoints = this.StartPoint == edge.StartPoint && this.EndPoint == edge.EndPoint;
            var samePointsReversed =
                this.StartPoint == edge.EndPoint && this.EndPoint == edge.StartPoint;
            return samePoints || samePointsReversed;
        }


        /// <inheritdoc />
        public override int GetHashCode()
        {
            var hCode = ( int ) this.StartPoint.X
                      ^ ( int ) this.StartPoint.Y
                      ^ ( int ) this.EndPoint.X
                      ^ ( int ) this.EndPoint.Y;
            return hCode.GetHashCode();
        }
    }
}