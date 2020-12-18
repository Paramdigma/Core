using Paramdigma.Core.Collections;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    ///     Represents a 2-dimensional line.
    /// </summary>
    public class Line2d
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Line2d" /> class.
        /// </summary>
        /// <param name="startPoint">Start point of the line.</param>
        /// <param name="endPoint">End point of the line.</param>
        public Line2d(Point2d startPoint, Point2d endPoint)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
            this.Domain = new Interval(0, this.Length);
        }


        /// <summary>
        ///     Initializes a new instance of the <see cref="Line2d" /> class.
        /// </summary>
        /// <param name="startPoint">The start point of the line.</param>
        /// <param name="direction">Direction. The length of the vector will determine the end point.</param>
        /// <returns>New 2d line instance.</returns>
        public Line2d(Point2d startPoint, Vector2d direction)
            : this(startPoint, startPoint + direction) { }


        /// <summary>
        ///     Initializes a new instance of the <see cref="Line2d" /> class.
        /// </summary>
        /// <param name="startPoint">Start point.</param>
        /// <param name="direction">Direction (length of vector will be disregarded).</param>
        /// <param name="length">Desired length of the line.</param>
        /// <returns>New 2d line instance.</returns>
        public Line2d(Point2d startPoint, Vector2d direction, double length)
            : this(startPoint, direction.Unit() * length) { }


        /// <summary>
        ///     Gets or sets the start point of the line.
        /// </summary>
        /// <value>3D Point.</value>
        public Point2d StartPoint { get; set; }

        /// <summary>
        ///     Gets or sets the end point of the line.
        /// </summary>
        /// <value>3D Point.</value>
        public Point2d EndPoint { get; set; }

        /// <summary>
        ///     Gets or sets the line's domain.
        /// </summary>
        /// <value>Interval.</value>
        public Interval Domain { get; set; }

        /// <summary>
        ///     Gets the vector representation of the line.
        /// </summary>
        public Vector2d Vector =>
            this; // Implicit line to vector conversion (this property exists just for convenience and readability)

        /// <summary>
        ///     Gets the length of the line.
        /// </summary>
        public double Length => this.Vector.Length;


        /// <summary>
        ///     Implicit conversion from line to vector.
        /// </summary>
        /// <param name="line">Line to be transformed into vector.</param>
        public static implicit operator Vector2d(Line2d line) => line.EndPoint - line.StartPoint;


        /// <summary>
        ///     Computes if a given point is at the left, right or on the current line.
        /// </summary>
        /// <param name="point">Point to test.</param>
        /// <returns>
        ///     >0 for point left of the line
        ///     =0 for point on the line
        ///     bigger 0 for point right of the line.
        /// </returns>
        public double IsLeft(Point2d point)
        {
            Vector2d v1 = this;
            var v2 = point - this.StartPoint;
            return v1.PerpProduct(v2);
        }
    }
}