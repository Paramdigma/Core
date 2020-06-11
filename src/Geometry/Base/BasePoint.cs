using System;

#pragma warning disable 1591

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    /// Abstract class representing a generic vector entity. All vector related entities must inherit from it.
    /// </summary>
    public abstract class BasePoint
    {
        /// <summary>
        /// Gets or sets x Coordinate.
        /// </summary>
        public double X
        {
            get
            {
                return this.x;
            }

            set
            {
                if (this.IsUnset)
                    this.IsUnset = false;
                this.x = value;
            }
        }

        /// <summary>
        /// Gets or sets y Coordinate.
        /// </summary>
        public double Y
        {
            get
            {
                return this.y;
            }

            set
            {
                if (this.IsUnset)
                    this.IsUnset = false;
                y = value;
            }
        }

        /// <summary>
        /// Gets or sets z Coordinate.
        /// </summary>
        public double Z
        {
            get
            {
                return this.z;
            }

            set
            {
                if (this.IsUnset)
                    this.IsUnset = false;
                this.z = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the current point is unset.
        /// </summary>
        /// <value>True if Unset.</value>
        public bool IsUnset
        {
            get; set;
        }

        private double x;
        private double y;
        private double z;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePoint"/> class.
        /// </summary>
        /// <returns>New origin point.</returns>
        protected BasePoint()
            : this(0, 0, 0)
        {
            this.IsUnset = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePoint"/> class.
        /// </summary>
        /// <param name="point">Point to copy coordinates from.</param>
        /// <returns></returns>
        protected BasePoint(BasePoint point)
            : this(point.X, point.Y, point.Z)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasePoint"/> class by cartesian coordinates.
        /// </summary>
        /// <param name="xCoord">X coordinate.</param>
        /// <param name="yCoord">Y coordinate.</param>
        /// <param name="zCoord">Z coordinate.</param>
        protected BasePoint(double xCoord, double yCoord, double zCoord)
        {
            this.x = xCoord;
            this.y = yCoord;
            this.z = zCoord;
            this.IsUnset = false;
        }

        /// <summary>
        /// Add a point to this point.
        /// </summary>
        /// <param name="point">Point to add.</param>
        public void Add(BasePoint point)
        {
            this.x += point.X;
            this.y += point.Y;
            this.z += point.Z;
            IsUnset = false;
        }

        /// <summary>
        /// Substract a point from this one.
        /// </summary>
        /// <param name="point">Point to substract.</param>
        public void Substract(BasePoint point)
        {
            this.x -= point.X;
            this.y -= point.Y;
            this.z -= point.Z;
            this.IsUnset = false;
        }

        /// <summary>
        /// Multiply this point by a number.
        /// </summary>
        /// <param name="scalar">Number to multiply by.</param>
        public void Multiply(double scalar)
        {
            this.x *= scalar;
            this.y *= scalar;
            this.z *= scalar;
        }

        /// <summary>
        /// Divide this point by a number.
        /// </summary>
        /// <param name="scalar">Number to divide by.</param>
        public void Divide(double scalar)
        {
            this.x /= scalar;
            this.y /= scalar;
            this.z /= scalar;
        }

        /// <summary>
        /// Negates this point.
        /// </summary>
        public void Negate()
        {
            this.x = this.x != 0 ? -this.x : 0;
            this.y = this.y != 0 ? -this.y : 0;
            this.z = this.z != 0 ? -this.z : 0;
        }

        /// <summary>
        /// Returns the string representation of this point.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "{ " + this.x + ", " + this.y + ", " + this.z + " }";

        /// <summary>
        /// Converts a point to an array of numbers.
        /// </summary>
        /// <returns>Array with cartesian coordinates of point.</returns>
        public double[] ToArray() => new[] { this.x, this.y, this.z };

        /// <summary>
        /// Performs a deep clone of the point.
        /// </summary>
        /// <param name="obj">Object to compare to.</param>
        /// <returns>Returns a copy of this BasePoint instance.</returns>
        
        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (!(obj is BasePoint))
            {
                return false;
            }

            var pt = (BasePoint)obj;
            return Math.Abs(this.X - pt.X) <= Settings.Tolerance
                && Math.Abs(this.Y - pt.Y) <= Settings.Tolerance
                && Math.Abs(this.Z - pt.Z) <= Settings.Tolerance;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                // Choose large primes to avoid hashing collisions
                const int hashingBase = (int)2166136261;
                const int hashingMultiplier = 16777619;
                var tol = Settings.Tolerance * 2;
                var tX = (int)(this.X * (1 / tol)) * tol;
                var tY = (int)(this.Y * (1 / tol)) * tol;
                var tZ = (int)(this.Z * (1 / tol)) * tol;

                int hash = hashingBase;
                hash = (hash * hashingMultiplier) ^ tX.GetHashCode();
                hash = (hash * hashingMultiplier) ^ tY.GetHashCode();
                hash = (hash * hashingMultiplier) ^ tZ.GetHashCode();
                return hash;
            }
        }
    }
}