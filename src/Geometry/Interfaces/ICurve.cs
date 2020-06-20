namespace Paramdigma.Core.Geometry.Interfaces
{
    /// <summary>
    ///     Base interface that all curve entities must implement.
    /// </summary>
    public interface ICurve
    {
        /// <summary>
        ///     Computes the point on the curve at the specified parameter.
        /// </summary>
        /// <param name="t">Parameter.</param>
        /// <returns>Point on curve.</returns>
        Point3d PointAt(double t);

        /// <summary>
        ///     Computes the tangent vector on the curve at the specified parameter.
        /// </summary>
        /// <param name="t">Parameter.</param>
        /// <returns>Tangent on curve.</returns>
        Vector3d TangentAt(double t);

        /// <summary>
        ///     Computes the normal vector on the curve at the specified parameter.
        /// </summary>
        /// <param name="t">Parameter.</param>
        /// <returns>Normal on curve.</returns>
        Vector3d NormalAt(double t);

        /// <summary>
        ///     Computes the binormal vector on the curve at the specified parameter.
        /// </summary>
        /// <param name="t">Parameter.</param>
        /// <returns>Binormal vector on curve.</returns>
        Vector3d BinormalAt(double t);

        /// <summary>
        ///     Computes the perpendicular frame on the curve at the specified parameter.
        /// </summary>
        /// <param name="t">Parameter.</param>
        /// <returns>Perpendicular frame on curve.</returns>
        Plane FrameAt(double t);
    }
}