using System;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    ///     Represents an infinite 2-dimensional ray.
    /// </summary>
    public class Ray2d
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Ray2d" /> class.
        /// </summary>
        /// <param name="origin">Origin point.</param>
        /// <param name="direction">Direction vector.</param>
        public Ray2d(Point2d origin, Vector2d direction)
        {
            this.Origin = origin ?? throw new ArgumentNullException(nameof(origin));
            this.Direction = direction ?? throw new ArgumentNullException(nameof(direction));
        }

        /// <summary>
        ///     Gets or sets the origin of the ray.
        /// </summary>
        /// <value>Origin point.</value>
        public Point2d Origin
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets or sets the direction of the ray as a unit vector.
        /// </summary>
        /// <value>Direction vector.</value>
        public Vector2d Direction
        {
            get;
            set;
        }
    }
}