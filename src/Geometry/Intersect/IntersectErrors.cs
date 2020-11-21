using Paramdigma.Core.Geometry;

#pragma warning disable 1591

namespace Paramdigma.Core
{
    /// <summary>
    ///     Class contains all 3D related intersection methods.
    /// </summary>
    public static partial class Intersect3D
    {
        public enum LineLineIntersectionStatus
        {
            NoIntersection, Point, Error
        }

        public enum LinePlaneIntersectionStatus
        {
            NoIntersection, Point, OnPlane
        }

        public enum RayFacePerimeterIntersectionStatus
        {
            NoIntersection, Point, Error
        }

        public struct LineLineIntersectionResult
        {
            public double Distance { get; set; }

            public double ParamA { get; set; }

            public double ParamB { get; set; }

            public Point3d PointA { get; set; }

            public Point3d PointB { get; set; }
        }
    }
}