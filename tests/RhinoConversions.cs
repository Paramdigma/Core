using System;
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


        public static RG.NurbsCurve ToRhino(this NurbsCurve curve)
        {
            throw new NotImplementedException();
        }


        public static NurbsCurve ToCore(this RG.NurbsCurve curve)
        {
            throw new NotImplementedException();
        }


        public static RG.NurbsSurface ToRhino(this NurbsSurface surface)
        {
            // Create surface
            var surf = RG.NurbsSurface.Create(
                3,
                false,
                surface.DegreeU + 1, // order is degree+1
                surface.DegreeV + 1,
                surface.ControlPoints.N,
                surface.ControlPoints.M);

            // Assign control points
            for (var i = 0; i < surface.ControlPoints.N; i++)
            {
                for (var j = 0; j < surface.ControlPoints.M; j++)
                {
                    var pt = surface.ControlPoints[i, j];
                    surf.Points.SetPoint(i, j, pt.X, pt.Y, pt.Z, pt.Weight);
                }
            }

            // TODO: Add switch for periodic knots.

            // Create uniform knots.
            surf.KnotsU.CreateUniformKnots(1);
            surf.KnotsV.CreateUniformKnots(1);

            // Update surface interval.
            surf.SetDomain(0, new RG.Interval(0, 1));
            surf.SetDomain(1, new RG.Interval(0, 1));

            return surf;
        }


        public static NurbsSurface ToCore(this RG.NurbsSurface surface)
        {
            throw new NotImplementedException();
        }
    }
}