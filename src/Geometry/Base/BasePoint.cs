﻿using System;

namespace AR_Lib.Geometry
{
    public abstract class BasePoint
    {
        // Public properties
        public double X
        {
            get { return x; }
            set { if (isUnset) isUnset = false; x = Math.Round(value, Settings.MaxDecimals); }
        }
        public double Y { get { return y; } set { if (isUnset) isUnset = false; y = Math.Round(value, Settings.MaxDecimals); } }
        public double Z { get { return z; } set { if (isUnset) isUnset = false; z = Math.Round(value, Settings.MaxDecimals); } }
        public bool IsUnset { get => isUnset; }

        // Private parameters
        protected double x;
        protected double y;
        protected double z;
        protected bool isUnset;

        //Constructors

        protected BasePoint() : this(0, 0, 0) { isUnset = true; }

        protected BasePoint(BasePoint point) : this(point.X, point.Y, point.Z)
        {
        }

        protected BasePoint(double xCoord, double yCoord, double zCoord)
        {
            X = xCoord;
            Y = yCoord;
            Z = zCoord;
            isUnset = false;
        }


        // Mathematical operations

        public void Add(BasePoint point)
        {
            x += point.X;
            y += point.Y;
            z += point.Z;
            isUnset = false;
        }

        public void Substract(BasePoint point)
        {
            x -= point.X;
            y -= point.Y;
            z -= point.Z;
            isUnset = false;
        }

        public void Multiply(double scalar)
        {
            x *= scalar;
            y *= scalar;
            z *= scalar;
        }

        public void Divide(double scalar)
        {
            x /= scalar;
            y /= scalar;
            z /= scalar;
        }

        public void Negate()
        {
            x = -x;
            y = -y;
            z = -z;
        }

        public override string ToString() => "{ " + x + ", " + y + ", " + z + " }";
        public double[] ToArray() => new double[] { x, y, z };

        // Override Methods
        public override bool Equals(object obj)
        {
            if (!(obj is BasePoint))
                return false;
            var pt = (BasePoint)obj;
            return Math.Abs(X - pt.X) <= Settings.Tolerance
                && Math.Abs(Y - pt.Y) <= Settings.Tolerance
                && Math.Abs(Z - pt.Z) <= Settings.Tolerance;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                // Choose large primes to avoid hashing collisions
                const int hashingBase = (int)2166136261;
                const int hashingMultiplier = 16777619;
                double tol = Settings.Tolerance * 2;
                double tX = (int)(X * (1 / tol)) * tol;
                double tY = (int)(Y * (1 / tol)) * tol;
                double tZ = (int)(Z * (1 / tol)) * tol;

                int hash = hashingBase;
                hash = (hash * hashingMultiplier) ^ tX.GetHashCode();
                hash = (hash * hashingMultiplier) ^ tY.GetHashCode();
                hash = (hash * hashingMultiplier) ^ tZ.GetHashCode();
                return hash;
            }
        }

    }

}