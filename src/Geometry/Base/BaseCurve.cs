using Paramdigma.Core.Collections;

#pragma warning disable 1591

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    ///     Represents a generic curve. This class is abstract and all curve classes should inherit from it.
    /// </summary>
    public abstract class BaseCurve
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseCurve" /> class.
        /// </summary>
        protected BaseCurve() => this.Domain = Interval.Unit;
        // Public properties

        /// <summary>
        ///     Gets or sets the curve's domain.
        /// </summary>
        public Interval Domain
        {
            get;
            set;
        }

        /// <summary>
        ///     Gets a value indicating whether the curve is valid.
        /// </summary>
        /// <returns>True if Valid.</returns>
        public bool IsValid => this.CheckValidity();

        public double Length => this.ComputeLength();

        /// <summary>
        ///     Compute a point along the curve at a specified parameter.
        /// </summary>
        /// <param name="t">Parameter.</param>
        /// <returns>Point at the parameter specified.</returns>
        public abstract Point3d PointAt(double t);

        /// <summary>
        ///     Compute the tangent vector along the curve at a specified parameter.
        /// </summary>
        /// <param name="t">Parameter.</param>
        /// <returns>Tangent vector at the parameter specified.</returns>
        public abstract Vector3d TangentAt(double t);

        /// <summary>
        ///     Compute normal vector along the curve at a specified parameter.
        /// </summary>
        /// <param name="t">Parameter.</param>
        /// <returns>Normal vector at the parameter specified.</returns>
        public abstract Vector3d NormalAt(double t);

        /// <summary>
        ///     Compute a binormal vector along the curve at a specified parameter.
        /// </summary>
        /// <param name="t">Parameter.</param>
        /// <returns>Binormal vector at the parameter specified.</returns>
        public abstract Vector3d BinormalAt(double t);

        /// <summary>
        ///     Compute the perpendicular frame along the curve at a specified parameter.
        /// </summary>
        /// <param name="t">Parameter.</param>
        /// <returns>Perpendicular plane at the parameter specified.</returns>
        public abstract Plane FrameAt(double t);

        /// <summary>
        ///     Checks the validity of the curve.
        /// </summary>
        /// <returns>True if valid.</returns>
        public abstract bool CheckValidity();

        /// <summary>
        ///     Computes the length of the curve.
        /// </summary>
        /// <returns>Length as number.</returns>
        protected abstract double ComputeLength();
    }
}