using System;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    /// Represents a 3D Vector entity.
    /// </summary>
    public class Vector3d : BasePoint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3d"/> class.
        /// </summary>
        public Vector3d()
            : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3d"/> class with the same values as the provided vector.
        /// </summary>
        /// <param name="vector">Vector to copy values from.</param>
        public Vector3d(Vector3d vector)
            : base(vector) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3d"/> class from a v.
        /// </summary>
        /// <param name="point">Point to copy values from.</param>
        /// <returns>New vector with the same coordinate values as the given v.</returns>
        public Vector3d(Point3d point)
            : base(point) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="Vector3d"/> class given it's 3 coordinates.
        /// </summary>
        /// <param name="xCoord">X coordinate.</param>
        /// <param name="yCoord">Y coordinate.</param>
        /// <param name="zCoord">Z coordinate.</param>
        /// <returns>Vector entity with the specified coordinate values.</returns>
        public Vector3d(double xCoord, double yCoord, double zCoord)
            : base(xCoord, yCoord, zCoord) { }

        /// <summary>
        /// Gets the Euclidean length squared of this vector.
        /// </summary>
        /// <returns>Squared Length of the vector.</returns>
        public double LengthSquared => DotProduct(this, this);

        /// <summary>
        /// Gets the Euclidean length of this vector.
        /// </summary>
        /// <returns>Length of the vector.</returns>
        public double Length => Math.Sqrt(LengthSquared);

        /// <summary>
        /// Divides this vector by it's euclidean length.
        /// </summary>
        public void Unitize()
        {
            double length = Length;
            X /= length;
            Y /= length;
            Z /= length;
        }

        /// <summary>
        /// Returns a normalized copy of this vector.
        /// </summary>
        /// <returns>A copy of this vector unitized.</returns>
        public Vector3d Unit()
        {
            double length = Length;
            double x = X / length;
            double y = Y / length;
            double z = Z / length;
            return new Vector3d(x, y, z);
        }

        /// <summary>
        /// Computes the dot product of this vector and v.
        /// </summary>
        /// <param name="v">Vector.</param>
        /// <returns>Dot product.</returns>
        public double Dot(Vector3d v) => DotProduct(this, v);

        /// <summary>
        /// Returns the cross product of this vector and v.
        /// </summary>
        /// <param name="v">Vector.</param>
        /// <returns>Cross product vector.</returns>
        public Vector3d Cross(Vector3d v) => CrossProduct(this, v);

        /// <summary>
        /// Gets the scalar product (dot product) of two given vectors
        /// Dot product = u1*v1 + u2*v2 + u3*v3.
        /// </summary>
        /// <param name="u">First vector.</param>
        /// <param name="v">Second vector.</param>
        /// <returns>Numerical value of the dot product.</returns>
        public static double DotProduct(Vector3d u, Vector3d v) => (u.X * v.X) + (u.Y * v.Y) + (u.Z * v.Z);

        /// <summary>
        /// Computes the vector product (cross product) of two given vectors
        /// Cross product = { u2 * v3 - u3 * v2; u3 * v1 - u1 * v3; u1 * v2 - u2 * v1 }.
        /// </summary>
        /// <param name="u">First vector.</param>
        /// <param name="v">Second vector.</param>
        /// <returns>Vector result of the cross product.</returns>
        public static Vector3d CrossProduct(Vector3d u, Vector3d v)
        {
            double x = (u.Y * v.Z) - (u.Z * v.Y);
            double y = (u.Z * v.X) - (u.X * v.Z);
            double z = (u.X * v.Y) - (u.Y * v.X);

            return new Vector3d(x, y, z);
        }

        /// <summary>
        /// Computes the angle in Radians between two given vectors
        /// Angle = Arcosine of the CrossProduct of UxV divided with their multiplied lengths.
        /// </summary>
        /// <param name="u">First vector.</param>
        /// <param name="v">Second vector.</param>
        /// <returns>Angle formed between u and v.</returns>
        public static double Angle(Vector3d u, Vector3d v)
        {
            return Math.Acos(DotProduct(u, v) / (u.Length * v.Length));
        }

        /// <summary>
        /// Gets a vector of unit length in the X direction.
        /// </summary>
        /// <returns>Vector {1,0,0}.</returns>
        public static Vector3d UnitX => new Vector3d(1, 0, 0);

        /// <summary>
        /// Gets a vector of unit length in the Y direction.
        /// </summary>
        /// <returns>Vector {0,1,0}.</returns>
        public static Vector3d UnitY => new Vector3d(0, 1, 0);

        /// <summary>
        /// Gets a vector of unit length in the Z direction.
        /// </summary>
        /// <returns>Vector {0,1,0}.</returns>
        public static Vector3d UnitZ => new Vector3d(0, 0, 1);

        /// <summary>
        /// Adds one vector to another.
        /// </summary>
        /// <param name="v">First vector to add.</param>
        /// <param name="v2">Second vector to add.</param>
        /// <returns>New vector entity with the result of the addition.</returns>
        public static Vector3d operator +(Vector3d v, Vector3d v2) => new Vector3d(v.X + v2.X, v.Y + v2.Y, v.Z + v2.Z);

        /// <summary>
        /// Substracts one vector from another.
        /// </summary>
        /// <param name="v">Vector to substract from.</param>
        /// <param name="v2">Vector to be substracted.</param>
        /// <returns>New vector entity with the result of the substraction.</returns>
        public static Vector3d operator -(Vector3d v, Vector3d v2) => new Vector3d(v.X - v2.X, v.Y - v2.Y, v.Z - v2.Z);

        /// <summary>
        /// Multiply one vector by a number.
        /// </summary>
        /// <param name="v">Vector to multiply.</param>
        /// <param name="scalar">Number to multiply with.</param>
        /// <returns>New vector entity with the result of the multiplication.</returns>
        public static Vector3d operator *(Vector3d v, double scalar) => new Vector3d(v.X * scalar, v.Y * scalar, v.Z * scalar);

        /// <summary>
        /// Multiply one vector by a number.
        /// </summary>
        /// <param name="scalar">Number to multiply with.</param>
        /// <param name="v">Vector to multiply.</param>
        /// <returns>New vector entity with the result of the multiplication.</returns>
        public static Vector3d operator *(double scalar, Vector3d v) => new Vector3d(v.X * scalar, v.Y * scalar, v.Z * scalar);

        /// <summary>
        /// Negate a vector.
        /// </summary>
        /// <param name="v">Vector to negate.</param>
        /// <returns>New vector entity with all the values negated.</returns>
        public static Vector3d operator -(Vector3d v)
            => new Vector3d(
                v.X != 0 ? -v.X : 0,
                v.Y != 0 ? -v.Y : 0,
                v.Z != 0 ? -v.Z : 0);

        /// <summary>
        /// Divide a vector by a number.
        /// </summary>
        /// <param name="v">Vector to be divided.</param>
        /// <param name="scalar">Number to be divided by.</param>
        /// <returns>New vector entity with the result of the division.</returns>
        public static Vector3d operator /(Vector3d v, double scalar) => new Vector3d(v.X / scalar, v.Y / scalar, v.Z / scalar);

        /// <summary>
        /// Checks if two vectors are equal.
        /// </summary>
        /// <param name="v">First vector.</param>
        /// <param name="w">Second vector.</param>
        /// <returns>Result of the comparison between v and w.</returns>
        public static bool operator ==(Vector3d v, Vector3d w) => v.Equals(w);

        /// <summary>
        /// Checks if two vectors are not equal.
        /// </summary>
        /// <param name="v">First vector.</param>
        /// <param name="w">Second vector.</param>
        /// <returns>Result of the comparison between v and w.</returns>
        public static bool operator !=(Vector3d v, Vector3d w) => !v.Equals(w);

        /// <inheritdoc/>
        public override bool Equals(object obj) => base.Equals(obj);

        /// <summary>
        /// Converts a vector into a string.
        /// </summary>
        /// <returns>Returns a string representation of this vector.</returns>
        public override string ToString() => "Vector3d" + base.ToString();

        /// <inheritdoc/>
        public override int GetHashCode() => base.GetHashCode();
    }
}