using System;
using Paramdigma.Core.Collections;
using Paramdigma.Core.Geometry.Interfaces;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    ///     Represents a toroidal surface.
    /// </summary>
    public class Torus : ISurface
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Torus" /> class from a plane and two radii.
        /// </summary>
        /// <param name="plane">The torus base plane.</param>
        /// <param name="majorRadius">The torus major radius.</param>
        /// <param name="minorRadius">The torus minor radius.</param>
        public Torus(Plane plane, double majorRadius, double minorRadius)
        {
            this.Plane = plane;
            this.MajorRadius = majorRadius;
            this.MinorRadius = minorRadius;
        }


        /// <summary>
        ///     Gets or sets the torus base plane.
        /// </summary>
        /// <value><see cref="Plane" />.</value>
        public Plane Plane { get; set; }

        /// <summary>
        ///     Gets or sets the torus major radius.
        /// </summary>
        /// <value><see cref="double" />.</value>
        public double MajorRadius { get; set; }

        /// <summary>
        ///     Gets or sets the torus minor radius.
        /// </summary>
        /// <value><see cref="double" />.</value>
        public double MinorRadius { get; set; }

        /// <inheritdoc />
        public Interval DomainU { get; set; }

        /// <inheritdoc />
        public Interval DomainV { get; set; }


        /// <inheritdoc />
        public Plane FrameAt(double u, double v) => throw new NotImplementedException();


        /// <inheritdoc />
        public double DistanceTo(Point3d point) => throw new NotImplementedException();


        /// <inheritdoc />
        public Point3d ClosestPointTo(Point3d point) => throw new NotImplementedException();


        /// <inheritdoc />
        public Vector3d NormalAt(double u, double v) => throw new NotImplementedException();


        /// <inheritdoc />
        public Point3d PointAt(double u, double v) => throw new NotImplementedException();
    }
}