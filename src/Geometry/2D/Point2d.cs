using System;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    ///     2-Dimensional point.
    /// </summary>
    public class Point2d
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Point2d" /> class.
        /// </summary>
        public Point2d()
            : this(0, 0) { }


        /// <summary>
        ///     Initializes a new instance of the <see cref="Point2d" /> class from x and y coordinates.
        /// </summary>
        /// <param name="x">X coordinate of the point.</param>
        /// <param name="y">Y coordinate of the point.</param>
        public Point2d(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }


        /// <summary>
        ///     Initializes a new instance of the <see cref="Point2d" /> class from an existing point.
        /// </summary>
        /// <param name="pt">A 2D point.</param>
        public Point2d(Point2d pt)
            : this(pt.X, pt.Y) { }


        /// <summary>
        ///     Gets or sets the X coordinate of the point.
        /// </summary>
        public double X { get; set; }

        /// <summary>
        ///     Gets or sets the Y coordinate of the point.
        /// </summary>
        public double Y { get; set; }

        /// <summary>
        ///     Gets a new 2d point with all coordinates =0.
        /// </summary>
        /// <returns>Origin 2d point.</returns>
        public static Point2d Origin => new Point2d(0, 0);

        // Overrided methods


        /// <summary>
        ///     String representation of a 2-dimensional point instance.
        /// </summary>
        /// <returns>Returns string representation of this Point2d instance.</returns>
        public override string ToString() => "Point2d{ " + this.X + "; " + this.Y + "}";


        /// <summary>
        ///     Compares a Point2d instance to the given objects.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>Returns true if object is equals, false if not.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Point2d))
                return false;
            var pt = ( Point2d ) obj;
            return Math.Abs(this.X - pt.X) <= Settings.Tolerance
                && Math.Abs(this.Y - pt.Y) <= Settings.Tolerance;
        }


        /// <summary>
        ///     Gets the hash code for the corresponding Point2d instance.
        /// </summary>
        /// <returns>Returns an int representing the Point2d hash code.</returns>
        public override int GetHashCode()
        {
            unchecked
            {
                // Choose large primes to avoid hashing collisions
                const int hashingBase = ( int ) 2166136261;
                const int hashingMultiplier = 16777619;
                var tol = Settings.Tolerance * 2;
                var tX = ( int ) (this.X * (1 / tol)) * tol;
                var tY = ( int ) (this.Y * (1 / tol)) * tol;

                var hash = hashingBase;
                hash = (hash * hashingMultiplier) ^ tX.GetHashCode();
                hash = (hash * hashingMultiplier) ^ tY.GetHashCode();
                return hash;
            }
        }


        /// <summary>
        ///     Add two points together.
        /// </summary>
        /// <param name="point">First point.</param>
        /// <param name="point2">Second point.</param>
        /// <returns>Addition result.</returns>
        public static Vector2d operator +(Point2d point, Point2d point2) =>
            new Vector2d(point.X + point2.X, point.Y + point2.Y);


        /// <summary>
        ///     Substracts one point from another.
        /// </summary>
        /// <param name="point">First point.</param>
        /// <param name="point2">Second point.</param>
        /// <returns>Substraction result.</returns>
        public static Vector2d operator -(Point2d point, Point2d point2) =>
            new Vector2d(point.X - point2.X, point.Y - point2.Y);


        /// <summary>
        ///     Negates a given point.
        /// </summary>
        /// <param name="point">Point to negate.</param>
        /// <returns>Negation result.</returns>
        public static Point2d operator -(Point2d point) => new Point2d(-point.X, -point.Y);


        /// <summary>
        ///     Multiplies a point by a number.
        /// </summary>
        /// <param name="point">Point to multiply.</param>
        /// <param name="scalar">Operand.</param>
        /// <returns>Multiplication result.</returns>
        public static Point2d operator *(Point2d point, double scalar) =>
            new Point2d(point.X * scalar, point.Y * scalar);


        /// <summary>
        ///     Multiplies a point by a number.
        /// </summary>
        /// <param name="scalar">Operand.</param>
        /// <param name="point">Point to multiply.</param>
        /// <returns>Multiplication result.</returns>
        public static Point2d operator *(double scalar, Point2d point) =>
            new Point2d(point.X * scalar, point.Y * scalar);


        /// <summary>
        ///     Divides a point by a number.
        /// </summary>
        /// <param name="point">Point.</param>
        /// <param name="scalar">Operand.</param>
        /// <returns>Division result.</returns>
        public static Point2d operator /(Point2d point, double scalar) =>
            new Point2d(point.X / scalar, point.Y / scalar);


        /// <summary>
        ///     Equality comparison between points.
        /// </summary>
        /// <param name="point">First point.</param>
        /// <param name="point2">Second point.</param>
        /// <returns>True if equal.</returns>
        public static bool operator ==(Point2d point, Point2d point2) => point.Equals(point2);


        /// <summary>
        ///     Inequality comparison between points.
        /// </summary>
        /// <param name="point">First point.</param>
        /// <param name="point2">Second point.</param>
        /// <returns>True if NOT equal.</returns>
        public static bool operator !=(Point2d point, Point2d point2) => !point.Equals(point2);


        /// <summary>
        ///     Divides a point by a number.
        /// </summary>
        /// <param name="point">Point.</param>
        /// <param name="v">Vector.</param>
        /// <returns>Division result.</returns>
        public static Point2d operator +(Point2d point, Vector2d v) =>
            new Point2d(point.X + v.X, point.Y + v.Y);


        // Implicit conversions


        /// <summary>
        ///     Explicit conversion from 2-dimensional point to vector.
        /// </summary>
        /// <param name="v">Vector to convert.</param>
        public static explicit operator Point2d(Vector2d v) => new Point2d(v.X, v.Y);


        /// <summary>
        ///     Implicit conversion from 2-dimensional point to vector.
        /// </summary>
        /// <param name="pt">Point to convert.</param>
        public static implicit operator Vector2d(Point2d pt) => new Vector2d(pt);
    }
}