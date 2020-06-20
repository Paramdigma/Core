using System;
using Paramdigma.Core.Collections;
using Paramdigma.Core.Geometry.Interfaces;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    ///     Represents a spherical surface.
    /// </summary>
    public class Sphere : ISurface
    {
        public Sphere(Plane plane, double radius)
        {
            if (Math.Abs(radius) < Settings.Tolerance)
                throw new ArithmeticException("Can't create a sphere of 0 radius.");
            this.Plane = plane;
            this.Radius = radius;
            this.DomainU = Interval.Unit;
            this.DomainV = Interval.Unit;
        }

        public Sphere() : this(Plane.WorldXY, 1) { }

        /// <summary>
        ///     Gets or sets the base plane of the sphere.
        /// </summary>
        /// <value><see cref="Plane" />.</value>
        public Plane Plane
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets or sets the radius of the sphere.
        /// </summary>
        /// <value><see cref="double" />.</value>
        public double Radius
        {
            get;
            set;
        }

        /// <inheritdoc />
        public Interval DomainU
        {
            get;
            set;
        }

        /// <inheritdoc />
        public Interval DomainV
        {
            get;
            set;
        }

        /// <inheritdoc />
        public double DistanceTo(Point3d point) => this.Plane.Origin.DistanceTo(point) - this.Radius;

        /// <inheritdoc />
        public Point3d ClosestPointTo(Point3d point) => this.Plane.Origin + ((point - this.Plane.Origin).Unit() * this.Radius);

        /// <inheritdoc />
        public Plane FrameAt(double u, double v) => throw new NotImplementedException();

        /// <inheritdoc />
        public Vector3d NormalAt(double u, double v) => (this.PointAt(u, v) - this.Plane.Origin).Unit();


        /// <inheritdoc />
        public Point3d PointAt(double u, double v)
        {
            double x, y, z;
            var tau = new Interval(0, Math.PI).RemapFromUnit(v);
            var rho = new Interval(0, 2 * Math.PI).RemapFromUnit(u);
            x = this.Radius * Math.Sin(tau) * Math.Cos(rho);
            y = this.Radius * Math.Sin(tau) * Math.Sin(rho);
            z = this.Radius * Math.Cos(tau);
            return this.Plane.PointAt(x, y, z);
        }

        /// <summary>
        ///     Returns the closest point on the sphere as a 2D point containing it's UV coordinates.
        /// </summary>
        /// <param name="pt">Point to find closest to</param>
        /// <returns>UV Parameter of the closest point as a Point2d instance.</returns>
        public Point2d ClosestParam(Point3d pt)
        {
            var rho = Math.Atan(pt.Y / pt.X);
            var tau = Math.Atan(Math.Sqrt((pt.X * pt.X) + (pt.Y * pt.Y)) / pt.Z);
            var u = new Interval(0, 2 * Math.PI).RemapToUnit(rho);
            var v = new Interval(0, Math.PI).RemapToUnit(tau);
            return new Point2d(u, v);
        }

        /// <summary>
        ///     Computes the point at a specified parameter, provided as a <see cref="Point2d" /> instance.
        /// </summary>
        /// <param name="uvPoint"><see cref="Point2d" /> parameter coordinates.</param>
        /// <returns><see cref="Point3d" /> instance of the specified point.</returns>
        public Point3d PointAt(Point2d uvPoint) => this.PointAt(uvPoint.X, uvPoint.Y);
    }
}