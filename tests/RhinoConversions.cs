using Paramdigma.Core.Geometry;
using RG = Rhino.Geometry;

namespace Paramdigma.Core.Tests.Conversions
{
    public static class RhinoConversions
    {
        public static Point3d ToCore(this RG.Point3d point) =>
            new Point3d(point.X, point.Y, point.Z);


        public static RG.Point3d ToRhino(this Point3d point) =>
            new RG.Point3d(point.X, point.Y, point.Z);


        public static Vector3d ToCore(this RG.Vector3d vector) =>
            new Vector3d(vector.X, vector.Y, vector.Z);


        public static RG.Vector3d ToRhino(this Vector3d vector) =>
            new RG.Vector3d(vector.X, vector.Y, vector.Z);
    }
}