using Paramdigma.Core.Collections;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    ///     Represents a 3D box.
    /// </summary>
    public class Box
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Box" /> class.
        /// </summary>
        /// <param name="plane">Base plane of the box.</param>
        /// <param name="domainX">Range of values in the X axis.</param>
        /// <param name="domainY">Range of values in the Y axis.</param>
        /// <param name="domainZ">Range of values in the Z axis.</param>
        public Box(Plane plane, Interval domainX, Interval domainY, Interval domainZ)
        {
            this.Plane = plane;
            this.DomainX = domainX;
            this.DomainY = domainY;
            this.DomainZ = domainZ;
        }


        /// <summary>
        ///     Initializes a new instance of the <see cref="Box" /> class from 2 corners. Both corners will
        ///     form the diagonal of
        ///     the box.
        /// </summary>
        /// <param name="lower">Lower left corner point.</param>
        /// <param name="upper">Upper right corner point.</param>
        public Box(Point3d lower, Point3d upper)
        {
            this.Plane = Plane.WorldXY;
            this.DomainX = new Interval(lower.X, upper.X);
            this.DomainY = new Interval(lower.Y, upper.Y);
            this.DomainZ = new Interval(lower.Z, upper.Z);
        }


        /// <summary>
        ///     Gets or sets the box's base plane.
        /// </summary>
        /// <value><see cref="Plane" />.</value>
        public Plane Plane { get; set; }

        /// <summary>
        ///     Gets or sets the box's X axis domain.
        /// </summary>
        /// <value><see cref="Interval" />.</value>
        public Interval DomainX { get; set; }

        /// <summary>
        ///     Gets or sets the box's Y axis domain.
        /// </summary>
        /// <value><see cref="Interval" />.</value>
        public Interval DomainY { get; set; }

        /// <summary>
        ///     Gets or sets the box's Z axis domain.
        /// </summary>
        /// <value><see cref="Interval" />.</value>
        public Interval DomainZ { get; set; }

        /// <summary>
        ///     Gets the corner point with lowest values.
        /// </summary>
        /// <returns><see cref="Point3d" />.</returns>
        public Point3d Min => new Point3d(
            this.DomainX.Start,
            this.DomainY.Start,
            this.DomainZ.Start);

        /// <summary>
        ///     Gets the corner point with highest values.
        /// </summary>
        /// <returns><see cref="Point3d" />.</returns>
        public Point3d Max => new Point3d(this.DomainX.End, this.DomainY.End, this.DomainZ.End);

        /// <summary>
        ///     Gets the center point of the box.
        /// </summary>
        /// <returns><see cref="Point3d" />.</returns>
        public Point3d Center => new Point3d(
            this.DomainX.RemapFromUnit(0.5),
            this.DomainY.RemapFromUnit(0.5),
            this.DomainZ.RemapFromUnit(0.5));
    }
}