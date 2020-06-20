using System;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    ///     Infinite 3d ray starting at a point.
    /// </summary>
    public class Ray
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Ray" /> class with origin and direction.
        /// </summary>
        /// <param name="origin">Point representing the origin of the ray.</param>
        /// <param name="direction">Vector representing the direction of the ray.</param>
        public Ray(Point3d origin, Vector3d direction)
        {
            this.Origin = origin ?? throw new ArgumentNullException(nameof(origin));
            this.Direction = direction ?? throw new ArgumentNullException(nameof(direction));
        }

        /// <summary>
        ///     Gets or sets the origin point of the ray.
        /// </summary>
        public Point3d Origin
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets or sets the direction vector of the ray.
        /// </summary>
        public Vector3d Direction
        {
            get;
            set;
        }

        /// <summary>
        ///     Computes a point in the ray at the given parameter.
        /// </summary>
        /// <param name="t">Parameter to obtain point.</param>
        /// <returns>Returns a point at the specified parameter of the Ray.</returns>
        public Point3d PointAt(double t) => this.Origin + (t * this.Direction);
    }
}