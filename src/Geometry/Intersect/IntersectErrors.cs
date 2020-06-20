using Paramdigma.Core.Geometry;

#pragma warning disable 1591

namespace Paramdigma.Core
{
    /// <summary>
    ///     Class contains all 3D related intersection methods.
    /// </summary>
    public static partial class Intersect3D
    {
        public enum ISLineLine
        {
            NoIntersection, Point, Error
        }

        // INFO: IS prefix stands for Intersection Status
        // INFO: IR prefix stands for Intersection Result
        public enum ISLinePlane
        {
            NoIntersection, Point, OnPlane
        }

        public enum ISRayFacePerimeter
        {
            NoIntersection, Point, Error
        }

        public struct IRLineLine
        {
            public double Distance
            {
                get;
                set;
            }

            public double TA
            {
                get;
                set;
            }

            public double TB
            {
                get;
                set;
            }

            public Point3d PointA
            {
                get;
                set;
            }

            public Point3d PointB
            {
                get;
                set;
            }
        }
    }
}