using Paramdigma.Core.Collections;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    ///     Represents a 2D bounding box.
    /// </summary>
    public class BoundingBox2d
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BoundingBox2d" /> class  from 2 points.
        /// </summary>
        /// <param name="bottomLeftCorner">Bottom left corner.</param>
        /// <param name="topRightCorner">Top right corner.</param>
        public BoundingBox2d(Point2d bottomLeftCorner, Point2d topRightCorner)
        {
            this.XDomain = new Interval(bottomLeftCorner.X, topRightCorner.X);
            this.YDomain = new Interval(bottomLeftCorner.Y, topRightCorner.Y);
            if (this.XDomain.HasInvertedDirection)
                this.XDomain.FlipDirection();
            if (this.YDomain.HasInvertedDirection)
                this.YDomain.FlipDirection();
        }


        /// <summary>
        ///     Initializes a new instance of the <see cref="BoundingBox2d" /> class from a polyline.
        /// </summary>
        /// <param name="polyline">Polyline.</param>
        public BoundingBox2d(Polyline2d polyline)
        {
            var xMin = polyline.Vertices[0].X;
            var yMin = polyline.Vertices[0].Y;
            var xMax = polyline.Vertices[0].X;
            var yMax = polyline.Vertices[0].Y;

            polyline.Vertices.ForEach(
                vertex =>
                {
                    if (vertex.X < xMin)
                        xMin = vertex.X;
                    if (vertex.X > xMax)
                        xMax = vertex.X;

                    if (vertex.Y < yMin)
                        yMin = vertex.Y;
                    if (vertex.X > yMax)
                        yMax = vertex.Y;
                });

            this.XDomain = new Interval(xMin, xMax);
            this.YDomain = new Interval(yMin, yMax);
        }


        /// <summary>
        ///     Gets or sets the Domain in the X direction.
        /// </summary>
        /// <value></value>
        public Interval XDomain { get; set; }

        /// <summary>
        ///     Gets or sets the Domain in the Y direction.
        /// </summary>
        /// <value></value>
        public Interval YDomain { get; set; }

        /// <summary>
        ///     Gets the Bottom left corner of the BBox.
        /// </summary>
        /// <returns></returns>
        public Point2d BottomLeft => new Point2d(this.XDomain.Start, this.YDomain.Start);

        /// <summary>
        ///     Gets the Bottom right corner of the BBox.
        /// </summary>
        /// <returns></returns>
        public Point2d BottomRight => new Point2d(this.XDomain.End, this.YDomain.Start);

        /// <summary>
        ///     Gets the top left corner of the BBox.
        /// </summary>
        /// <returns></returns>
        public Point2d TopLeft => new Point2d(this.XDomain.Start, this.YDomain.End);

        /// <summary>
        ///     Gets the top right corner of the BBox.
        /// </summary>
        /// <returns></returns>
        public Point2d TopRight => new Point2d(this.XDomain.End, this.YDomain.End);

        /// <summary>
        /// Gets the midpoint at the left edge of the box.
        /// </summary>
        public Point2d MidLeft => new Point2d(this.XDomain.Start, this.YDomain.RemapFromUnit(0.5));

        /// <summary>
        /// Gets the midpoint at the right edge of the box.
        /// </summary>
        public Point2d MidRight => new Point2d(this.XDomain.End, this.YDomain.RemapFromUnit(0.5));

        /// <summary>
        /// Gets the midpoint at the bottom edge of the box.
        /// </summary>
        public Point2d MidBottom =>
            new Point2d(this.XDomain.RemapFromUnit(0.5), this.YDomain.Start);

        /// <summary>
        /// Gets the midpoint at the top edge of the box.
        /// </summary>
        public Point2d MidTop => new Point2d(this.XDomain.RemapFromUnit(0.5), this.YDomain.End);

        /// <summary>
        ///     Gets the center of the bounding BBox.
        /// </summary>
        public Point2d Center =>
            new Point2d(this.XDomain.RemapFromUnit(0.5), this.YDomain.RemapFromUnit(0.5));

        
        /// <summary>
        /// Checks if a point is contained inside the box.
        /// </summary>
        /// <param name="pt">Point to test containment.</param>
        /// <returns>True if point is contained inside the bounding box.</returns>
        public bool ContainsPoint(Point2d pt) =>
            this.XDomain.Contains(pt.X) && this.YDomain.Contains(pt.Y);

        /// <summary>
        /// Checks if a box intersects with this instance.
        /// </summary>
        /// <param name="box">Box to check intersection against.</param>
        /// <returns>True if intersection exists.</returns>
        public bool IntersectsBox(BoundingBox2d box)
        {
            var xCheck = this.XDomain.Contains(box.XDomain.Start)
                      || this.XDomain.Contains(box.XDomain.End);
            var yCheck = this.YDomain.Contains(box.YDomain.Start)
                      || this.YDomain.Contains(box.YDomain.End);
            return xCheck && yCheck;
        }


        /// <inheritdoc />
        public override string ToString() =>
            $"BBox2d [{this.XDomain.Start};{this.YDomain.Start}]-[{this.XDomain.End};{this.YDomain.End}]";
    }
}