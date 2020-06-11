using System;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    /// Represents a 3-dimensional point in cartesian coordinates.
    /// </summary>
    public class Point3d : BasePoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Point3d"/> class.
        /// </summary>
        /// <returns><see cref="Point3d"/>.</returns>
        public Point3d()
            : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point3d"/> class by cartesian coordinates.
        /// </summary>
        /// <param name="xCoord">X coordinate.</param>
        /// <param name="yCoord">Y coordinate.</param>
        /// <param name="zCoord">Z coordinate.</param>
        /// <returns></returns>
        public Point3d(double xCoord, double yCoord, double zCoord)
            : base(xCoord, yCoord, zCoord) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point3d"/> class from a 2-dimensional point.
        /// </summary>
        /// <param name="point">2d point to convert.</param>
        /// <returns><see cref="Point3d"/>.</returns>
        public Point3d(Point2d point)
            : base(point.X, point.Y, 0) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point3d"/> class.
        /// </summary>
        /// <param name="point">3d point to copy.</param>
        /// <returns><see cref="Point3d"/>.</returns>
        public Point3d(Point3d point)
            : base(point) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Point3d"/> class from a 4-dimensional point by dividing the cartesian coordinates by the weight.
        /// </summary>
        /// <param name="point">4d point to convert.</param>
        /// <returns><see cref="Point3d"/>.</returns>
        public Point3d(Point4d point)
            : this(point.X / point.Weight, point.Y / point.Weight, point.Z / point.Weight) { }

        /// <summary>
        /// Gets a new Unset point.
        /// </summary>
        /// <returns><see cref="Point3d"/>.</returns>
        public static Point3d Unset => new Point3d();

        /// <summary>
        /// Gets a point with X,Y,Z = 0, and Unset = False.
        /// </summary>
        /// <returns></returns>
        public static Point3d WorldOrigin => new Point3d(0, 0, 0);

        /// <summary>
        /// Performs a deep clone of the point.
        /// </summary>
        /// <returns>Returns a copy of this point instance.</returns>
        public Point3d Clone() => new Point3d(this.X, this.Y, this.Z);

        /// <summary>
        /// Gets the euclidean distance between this point and the provided one.
        /// </summary>
        /// <param name="point">Point.</param>
        /// <returns></returns>
        public double DistanceTo(Point3d point) => Math.Sqrt(Math.Pow(point.X - X, 2) + Math.Pow(point.Y - Y, 2) + Math.Pow(point.Z - Z, 2));

        /// <inheritdoc/>
        public override bool Equals(object obj) => base.Equals(obj);

        /// <inheritdoc/>
        public override int GetHashCode() => base.GetHashCode();

        /// <inheritdoc/>
        public override string ToString() => "Point3d" + base.ToString();

        /// <summary>
        /// Adds a vector to a point.
        /// </summary>
        /// <param name="point">Point.</param>
        /// <param name="v">Vector.</param>
        /// <returns><see cref="Point3d"/>.</returns>
        public static Point3d operator +(Point3d point, Vector3d v) => new Point3d(point.X + v.X, point.Y + v.Y, point.Z + v.Z);

        /// <summary>
        /// Substracts a point from another point.
        /// </summary>
        /// <param name="point">Point A.</param>
        /// <param name="point2">Point B.</param>
        /// <returns><see cref="Point3d"/>.</returns>
        public static Vector3d operator -(Point3d point, Point3d point2) => new Vector3d(point.X - point2.X, point.Y - point2.Y, point.Z - point2.Z);

        /// <summary>
        /// Substracts a vector from a point.
        /// </summary>
        /// <param name="point">Point.</param>
        /// <param name="vector">Vector.</param>
        /// <returns><see cref="Point3d"/>.</returns>
        public static Vector3d operator -(Point3d point, Vector3d vector) => new Vector3d(point.X - vector.X, point.Y - vector.Y, point.Z - vector.Z);

        /// <summary>
        /// Negates a point.
        /// </summary>
        /// <param name="point">Point.</param>
        /// <returns><see cref="Point3d"/>.</returns>
        public static Point3d operator -(Point3d point)
            => new Point3d(
                point.X != 0 ? -point.X : 0,
                point.Y != 0 ? -point.Y : 0,
                point.Z != 0 ? -point.Z : 0);

        /// <summary>
        /// Multiplies a point with a number.
        /// </summary>
        /// <param name="point">Point.</param>
        /// <param name="scalar">Number.</param>
        /// <returns><see cref="Point3d"/>.</returns>
        public static Point3d operator *(Point3d point, double scalar) => new Point3d(point.X * scalar, point.Y * scalar, point.Z * scalar);

        /// <summary>
        /// Multiplies a point with a number.
        /// </summary>
        /// <param name="scalar">Number.</param>
        /// <param name="point">Point.</param>
        /// <returns><see cref="Point3d"/>.</returns>
        public static Point3d operator *(double scalar, Point3d point) => new Point3d(point.X * scalar, point.Y * scalar, point.Z * scalar);

        /// <summary>
        /// Divides a point with a number.
        /// </summary>
        /// <param name="point">Point.</param>
        /// <param name="scalar">Number.</param>
        /// <returns><see cref="Point3d"/>.</returns>
        public static Point3d operator /(Point3d point, double scalar) => new Point3d(point.X / scalar, point.Y / scalar, point.Z / scalar);

        /// <summary>
        /// Checks equality between two points.
        /// </summary>
        /// <param name="point">Point A.</param>
        /// <param name="point2">Point B.</param>
        /// <returns><see cref="Point3d"/>.</returns>
        public static bool operator ==(Point3d point, Point3d point2) => point != null && point.Equals(point2);

        /// <summary>
        /// Checks inequality between two points.
        /// </summary>
        /// <param name="point">Point A.</param>
        /// <param name="point2">Point B.</param>
        /// <returns><see cref="Point3d"/>.</returns>
        public static bool operator !=(Point3d point, Point3d point2) => !point.Equals(point2);

        // Implicit conversions

        /// <summary>
        /// Explicit conversion from vector to point.
        /// </summary>
        /// <param name="v">3d Vector to convert.</param>
        public static explicit operator Point3d(Vector3d v) => new Point3d(v.X, v.Y, v.Z);

        /// <summary>
        /// Implicit conversion from point to vector.
        /// </summary>
        /// <param name="pt">3d Point to convert.</param>
        public static implicit operator Vector3d(Point3d pt) => new Vector3d(pt.X, pt.Y, pt.Z);

        /// <summary>
        /// Explicit conversion from 4-dimensional point to 3-dimensional point. (X/W,Y/W,Z/W).
        /// </summary>
        /// <param name="point">3d Point to convert.</param>
        public static explicit operator Point3d(Point4d point) => new Point3d(point);
    }
}