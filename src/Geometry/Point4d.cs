using System;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    ///     Represents a 4-dimensional point.
    /// </summary>
    public class Point4d : BasePoint
    {
        private double weight;


        /// <summary>
        ///     Initializes a new instance of the <see cref="Point4d" /> class.
        /// </summary>
        /// <returns>Point with all values to zero.</returns>
        public Point4d() => this.weight = 0;


        /// <summary>
        ///     Initializes a new instance of the <see cref="Point4d" /> class by cartesian coordinates and
        ///     weight.
        /// </summary>
        /// <param name="x">X Coordinate.</param>
        /// <param name="y">Y Coordinate.</param>
        /// <param name="z">Z Coordinate.</param>
        /// <param name="w">Weight.</param>
        /// <returns>New 4-dimensional point with the specified values.</returns>
        public Point4d(double x, double y, double z, double w)
            : base(x, y, z) =>
            this.weight = w;


        /// <summary>
        ///     Initializes a new instance of the <see cref="Point4d" /> class from a 3-dimensional point and a
        ///     weight.
        /// </summary>
        /// <param name="pt">Point.</param>
        /// <param name="w">Weight.</param>
        /// <returns>New 4-dimensional point with the specified values.</returns>
        public Point4d(Point3d pt, double w)
            : base(pt) =>
            this.weight = w;


        /// <summary>
        ///     Initializes a new instance of the <see cref="Point4d" /> class from a 3-dimensional point and a
        ///     weight.
        /// </summary>
        /// <param name="pt">Point.</param>
        /// <returns>New 4-dimensional point with the specified values.</returns>
        public Point4d(Point3d pt)
            : base(pt) =>
            this.weight = 1;


        /// <summary>
        ///     Gets or sets the weight of this point.
        /// </summary>
        public double Weight
        {
            get => this.weight;
            set
            {
                this.weight = value;
                if (this.IsUnset)
                    this.IsUnset = false;
            }
        }

        /// <summary>
        ///     Gets the raw position of the point4d (without taking into account weight).
        /// </summary>
        public Point3d Position => new Point3d(this.X, this.Y, this.Z);


        /// <summary>
        ///     Adds to points together.
        /// </summary>
        /// <param name="point">First point to add.</param>
        /// <param name="point2">Second point to add.</param>
        /// <returns>A new <see cref="Point4d" /> instance.</returns>
        public static Point4d operator +(Point4d point, Point4d point2) => new Point4d(
            point.X + point2.X,
            point.Y + point2.Y,
            point.Z + point2.Z,
            point.Weight + point2.Weight);


        /// <summary>
        ///     Subtracts one point from another.
        /// </summary>
        /// <param name="point">Point to subtract from.</param>
        /// <param name="point2">Point to be subtracted.</param>
        /// <returns>A new <see cref="Point4d" /> instance.</returns>
        public static Point4d operator -(Point4d point, Point4d point2) => new Point4d(
            point.X - point2.X,
            point.Y - point2.Y,
            point.Z - point2.Z,
            point.Weight - point2.Weight);


        /// <summary>
        ///     Negates a point.
        /// </summary>
        /// <param name="point">Point to be negated.</param>
        /// <returns>A new <see cref="Point4d" /> instance.</returns>
        public static Point4d operator -(Point4d point) => new Point4d(
            -point.X,
            -point.Y,
            -point.Z,
            point.Weight);


        /// <summary>
        ///     Multiply a point by a scalar.
        /// </summary>
        /// <param name="point">Point to be multiplied.</param>
        /// <param name="scalar">Number to multiply point with.</param>
        /// <returns>A new <see cref="Point4d" /> instance.</returns>
        public static Point4d operator *(Point4d point, double scalar) => new Point4d(
            point.X * scalar,
            point.Y * scalar,
            point.Z * scalar,
            point.Weight * scalar);


        /// <summary>
        ///     Multiply a point by a scalar.
        /// </summary>
        /// <param name="scalar">Number to multiply point with.</param>
        /// <param name="point">Point to be multiplied.</param>
        /// <returns>A new <see cref="Point4d" /> instance.</returns>
        public static Point4d operator *(double scalar, Point4d point) => new Point4d(
            point.X * scalar,
            point.Y * scalar,
            point.Z * scalar,
            point.Weight * scalar);


        /// <summary>
        ///     Divides a point by a scalar value.
        /// </summary>
        /// <param name="point">Point to divide.</param>
        /// <param name="scalar">Number to divide point with.</param>
        /// <returns>A new <see cref="Point4d" /> instance.</returns>
        public static Point4d operator /(Point4d point, double scalar) => new Point4d(
            point.X / scalar,
            point.Y / scalar,
            point.Z / scalar,
            point.Weight / scalar);


        /// <summary>
        ///     Equality check between two <see cref="Point4d" /> instances.
        /// </summary>
        /// <param name="point">Point to check equality from.</param>
        /// <param name="point2">Point to check equality to.</param>
        /// <returns>True if points are equal.</returns>
        public static bool operator ==(Point4d point, Point4d point2) =>
            point?.Equals(point2) ?? false;


        /// <summary>
        ///     Inequality check between two <see cref="Point4d" /> instances.
        /// </summary>
        /// <param name="point">Point to check inequality from.</param>
        /// <param name="point2">Point to check inequality to.</param>
        /// <returns>True if points are NOT equal.</returns>
        public static bool operator !=(Point4d point, Point4d point2) =>
            !point?.Equals(point2) ?? true;


        /// <summary>
        ///     Adds a vector and a point.
        /// </summary>
        /// <param name="point">A 4d point</param>
        /// <param name="v">A 3d vector</param>
        /// <returns>A new <see cref="Point4d" /> instance with the resulting coordinates.</returns>
        public static Point4d operator +(Point4d point, Vector3d v) => new Point4d(
            point.X + v.X,
            point.Y + v.Y,
            point.Z + v.Z,
            point.Weight);


        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is Point4d pt)
            {
                return base.Equals(obj)
                    && Math.Abs(this.Weight - pt.Weight) < Settings.Tolerance;
            }

            return false;
        }


        /// <inheritdoc />
        public override int GetHashCode() =>
            // TODO: Non consistent getHashCode implementation
            base.GetHashCode() ^ this.weight.GetHashCode();


        // TODO: Add hasWeightedCoordinates boolean and implement a weightCoordinates() method
    }
}