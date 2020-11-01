using System;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    ///     Represents a 2-dimensional vector.
    /// </summary>
    public class Vector2d
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Vector2d" /> class from another vector.
        /// </summary>
        /// <param name="vector">Vector to duplicate.</param>
        /// <returns>New vector with same values.</returns>
        public Vector2d(Vector2d vector)
            : this(vector.X, vector.Y) { }


        /// <summary>
        ///     Initializes a new instance of the <see cref="Vector2d" /> class from a point.
        /// </summary>
        /// <param name="point">Point to convert.</param>
        /// <returns>New vector with same values.</returns>
        public Vector2d(Point2d point)
            : this(point.X, point.Y) { }


        /// <summary>
        ///     Initializes a new instance of the <see cref="Vector2d" /> class.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Vector2d(double x, double y)
        {
            this.X = x;
            this.Y = y;
        }


        /// <summary>
        ///     Gets or sets the X Coordininate of the vector.
        /// </summary>
        /// <value>X coordinate.</value>
        public double X { get; set; }

        /// <summary>
        ///     Gets or sets the Y coordinate of the vector.
        /// </summary>
        /// <value>Y coordinate.</value>
        public double Y { get; set; }

        /// <summary>
        ///     Gets the squared length of the vector.
        /// </summary>
        public double LengthSquared => this.X * this.X + this.Y * this.Y;

        /// <summary>
        ///     Gets the length of the vector.
        /// </summary>
        public double Length => Math.Sqrt(this.LengthSquared);

        /// <summary>
        ///     Gets a vector in the World X direction {1;0}.
        /// </summary>
        public static Vector2d WorldX => new Vector2d(1, 0);

        /// <summary>
        ///     Gets a vector in the World Y direction {0;1}.
        /// </summary>
        public static Vector2d WorldY => new Vector2d(0, 1);


        /// <summary>
        ///     Returns a unit vector of this vector.
        /// </summary>
        /// <returns>New vector of unit lenght.</returns>
        public Vector2d Unit() => new Vector2d(this / this.Length);


        /// <summary>
        ///     Force this vector to be unit length.
        /// </summary>
        public void Unitize()
        {
            var length = this.Length;
            this.X /= length;
            this.Y /= length;
        }


        /// <summary>
        ///     Returns a CCW perpendicular vector to the current instance.
        /// </summary>
        /// <returns></returns>
        public Vector2d Perp() => new Vector2d(-this.Y, this.X);


        /// <summary>
        ///     Computes the dot product between this vector and the given one.
        /// </summary>
        /// <param name="vector">Vector to compute dot-product with.</param>
        /// <returns>Dot product result.</returns>
        public double DotProduct(Vector2d vector) => this.X * vector.X + this.Y * vector.Y;


        /// <summary>
        ///     Computes the perp product between this vector and the given one.
        /// </summary>
        /// <param name="vector">Vector to compute perp-product with.</param>
        /// <returns>Perp product result.</returns>
        public double PerpProduct(Vector2d vector) => this.X * vector.Y - this.Y * vector.X;


        /// <summary>
        ///     Sums two vectors together.
        /// </summary>
        /// <param name="v">Vector A.</param>
        /// <param name="v2">Vector B.</param>
        public static Vector2d operator +(Vector2d v, Vector2d v2) =>
            new Vector2d(v.X + v2.X, v.Y + v2.Y);


        /// <summary>
        ///     Substracts one vector from another.
        /// </summary>
        /// <param name="v">Vector A.</param>
        /// <param name="v2">Vector B.</param>
        public static Vector2d operator -(Vector2d v, Vector2d v2) =>
            new Vector2d(v.X - v2.X, v.Y - v2.Y);


        /// <summary>
        ///     Multiplies a vector with a number.
        /// </summary>
        /// <param name="v">Vector.</param>
        /// <param name="scalar">Number to multiply vector with.</param>
        public static Vector2d operator *(Vector2d v, double scalar) =>
            new Vector2d(v.X * scalar, v.Y * scalar);


        /// <summary>
        ///     Multiplies a vector with a number.
        /// </summary>
        /// <param name="scalar">Number to multiply vector with.</param>
        /// <param name="v">Vector.</param>
        public static Vector2d operator *(double scalar, Vector2d v) =>
            new Vector2d(v.X * scalar, v.Y * scalar);


        /// <summary>
        ///     Negates the values of the vector.
        /// </summary>
        /// <param name="v">Vector.</param>
        public static Vector2d operator -(Vector2d v) => new Vector2d(-v.X, -v.Y);


        /// <summary>
        ///     Divides a vector with a number.
        /// </summary>
        /// <param name="v">Vector.</param>
        /// <param name="scalar">Number to divide vector with.</param>
        public static Vector2d operator /(Vector2d v, double scalar) =>
            new Vector2d(v.X / scalar, v.Y / scalar);


        /// <summary>
        ///     Checks for equality between two vectors.
        /// </summary>
        /// <param name="v">Vector A.</param>
        /// <param name="w">Vector B.</param>
        public static bool operator ==(Vector2d v, Vector2d w) => v.Equals(w);


        /// <summary>
        ///     Checks for inequality between two vectors.
        /// </summary>
        /// <param name="v">Vector A.</param>
        /// <param name="w">Vector B.</param>
        public static bool operator !=(Vector2d v, Vector2d w) => !v.Equals(w);


        /// <summary>
        ///     Gets the string representation of the vector.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"Vector3d [{this.X}, {this.Y}]";


        /// <summary>
        ///     Checks if the vector is equal to an object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True if equal.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Vector2d))
                return false;

            var vect = obj as Vector2d;
            return Math.Abs(this.X - vect.X) <= Settings.Tolerance
                && Math.Abs(this.Y - vect.Y) <= Settings.Tolerance;
        }


        /// <summary>
        ///     Get the hashCode of the vector.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            unchecked
            {
                // Choose large primes to avoid hashing collisions
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
    }
}