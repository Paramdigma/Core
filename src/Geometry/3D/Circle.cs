using System;
using Paramdigma.Core.Geometry.Interfaces;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    /// Represents a planar circle curve.
    /// </summary>
    public class Circle : ICurve
    {
        /// <summary>
        /// The base plane for the circle.
        /// </summary>
        public Plane Plane;
        
        /// <summary>
        /// The radius of the circle.
        /// </summary>
        public double Radius;

        /// <summary>
        /// Initializes a new instance of <see cref="Circle"/> by it's plane and radius.
        /// </summary>
        /// <param name="plane">The plane to draw the circle at.</param>
        /// <param name="radius">The desired radius of the circle.</param>
        public Circle(Plane plane, double radius)
        {
            this.Plane = plane;
            this.Radius = radius;
        }


        /// <inheritdoc />
        public Point3d PointAt(double t)
        {
            var radians = t * 2 * Math.PI;
            var x = this.Radius * Math.Cos(radians);
            var y = this.Radius * Math.Sin(radians);
            return this.Plane.PointAt(x, y, 0);
        }


        /// <inheritdoc />
        public Vector3d TangentAt(double t) => this.NormalAt(t).Cross(this.Plane.ZAxis);


        /// <inheritdoc />
        public Vector3d NormalAt(double t) => (this.Plane.Origin - this.PointAt(t)).Unit();


        /// <inheritdoc />
        public Vector3d BinormalAt(double t) => this.TangentAt(t).Cross(this.NormalAt(t));


        /// <inheritdoc />
        public Plane FrameAt(double t) => new Plane(
            this.PointAt(t),
            this.NormalAt(t),
            this.BinormalAt(t),
            this.TangentAt(t));
    }
}