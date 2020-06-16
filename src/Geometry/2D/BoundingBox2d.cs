using Paramdigma.Core.Collections;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    /// Represents a 2D bounding box.
    /// </summary>
    public class BoundingBox2d
    {
        /// <summary>
        /// Gets or sets the Domain in the X direction.
        /// </summary>
        /// <value></value>
        public Interval XDomain
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the Domain in the Y direction.
        /// </summary>
        /// <value></value>
        public Interval YDomain
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the Bottom left corner of the BBox.
        /// </summary>
        /// <returns></returns>
        public Point2d BottomLeft => new Point2d(this.XDomain.Start, this.YDomain.Start);

        /// <summary>
        /// Gets the Bottom right corner of the BBox.
        /// </summary>
        /// <returns></returns>
        public Point2d BottomRight => new Point2d(this.XDomain.End, this.YDomain.Start);

        /// <summary>
        /// Gets the top left corner of the BBox.
        /// </summary>
        /// <returns></returns>
        public Point2d TopLeft => new Point2d(this.XDomain.Start, this.YDomain.End);

        /// <summary>
        /// Gets the top right corner of the BBox.
        /// </summary>
        /// <returns></returns>
        public Point2d TopRight => new Point2d(this.XDomain.End, this.YDomain.End);

        public Point2d MidLeft => new Point2d(this.XDomain.Start, this.YDomain.RemapFromUnit(0.5));

        public Point2d MidRight => new Point2d(this.XDomain.End, this.YDomain.RemapFromUnit(0.5));

        public Point2d MidBottom => new Point2d(this.XDomain.RemapFromUnit(0.5), this.YDomain.Start);

        public Point2d MidTop => new Point2d(this.XDomain.RemapFromUnit(0.5), this.YDomain.End);

        /// <summary>
        /// Gets the center of the bounding BBox.
        /// </summary>
        public Point2d Center => new Point2d(XDomain.RemapFromUnit(0.5), YDomain.RemapFromUnit(0.5));

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundingBox2d"/> class  from 2 points.
        /// </summary>
        /// <param name="bottomLeftCorner">Bottom left corner.</param>
        /// <param name="topRightCorner">Top right corner.</param>
        public BoundingBox2d(Point2d bottomLeftCorner, Point2d topRightCorner)
        {
            this.XDomain = new Interval(bottomLeftCorner.X, topRightCorner.X);
            this.YDomain = new Interval(bottomLeftCorner.Y, topRightCorner.Y);
            if (this.XDomain.HasInvertedDirection)
                this.XDomain.FlipDirection();
            if (YDomain.HasInvertedDirection)
                this.YDomain.FlipDirection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundingBox2d"/> class from a polyline.
        /// </summary>
        /// <param name="polyline">Polyline.</param>
        public BoundingBox2d(Polyline2d polyline)
        {
            double xMin = polyline.Vertices[0].X;
            double yMin = polyline.Vertices[0].Y;
            double xMax = polyline.Vertices[0].X;
            double yMax = polyline.Vertices[0].Y;

            polyline.Vertices.ForEach(vertex =>
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

        public bool ContainsPoint(Point2d pt) => XDomain.Contains(pt.X) && YDomain.Contains(pt.Y);

        public bool IntersectsBox(BoundingBox2d box)
        {
            var xCheck = XDomain.Contains(box.XDomain.Start) || XDomain.Contains(box.XDomain.End);
            var yCheck = YDomain.Contains(box.YDomain.Start) || YDomain.Contains(box.YDomain.End);
            return xCheck && yCheck;
        }
    }
}