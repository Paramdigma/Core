using Paramdigma.Core.Geometry;

namespace Paramdigma.Core
{
    /// <summary>
    ///     Static class to handle unit and type conversions.
    /// </summary>
    public static class Convert
    {
        /// <summary>
        ///     Compute barycentric coordinates (u, v, w) for
        ///     point p with respect to triangle (a, b, c).
        /// </summary>
        /// <param name="p">Point to convert.</param>
        /// <param name="a">First point of triangle.</param>
        /// <param name="b">Second point of triangle.</param>
        /// <param name="c">Third point of triangle.</param>
        /// <returns></returns>
        public static double[] Point3dToBarycentric(Point3d p, Point3d a, Point3d b, Point3d c)
        {
            Vector3d v0 = b - a, v1 = c - a, v2 = p - a;

            var den = (v0.X * v1.Y) - (v1.X * v0.Y);

            var v = ((v2.X * v1.Y) - (v1.X * v2.Y)) / den;
            var w = ((v0.X * v2.Y) - (v2.X * v0.Y)) / den;
            var u = 1.0 - v - w;

            double[] result = {u, v, w};

            return result;
        }
    }
}