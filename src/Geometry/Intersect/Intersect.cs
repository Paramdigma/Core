using System;
using Paramdigma.Core.Geometry;
using Paramdigma.Core.HalfEdgeMesh;

namespace Paramdigma.Core
{
    // INFO: Error enums for all classes are in file IntersectErrors.cs

    /// <summary>
    ///     Static class containing all 3-dimensional intersection methods.
    /// </summary>
    public static partial class Intersect3D
    {
        /// <summary>
        ///     Intersect a 3d line with a plane.
        /// </summary>
        /// <param name="line">The 3d line to intersect.</param>
        /// <param name="plane">The 3d plane to intersect.</param>
        /// <param name="intersectionPoint">The resulting intersection point, if it exists.</param>
        /// <returns>Intersection result.</returns>
        public static LinePlaneIntersectionStatus LinePlane(
            Line line,
            Plane plane,
            out Point3d intersectionPoint)
        {
            var u = line.EndPoint - line.StartPoint;
            var w = line.StartPoint - plane.Origin;

            var d = Vector3d.DotProduct(plane.ZAxis, u);
            var n = -Vector3d.DotProduct(plane.ZAxis, w);

            if (Math.Abs(d) <= Settings.Tolerance)
            {
                // Segment is parallel to plane
                if (Math.Abs(n) < Settings.Tolerance)
                {
                    // Segment lies in plane
                    intersectionPoint = null;
                    return LinePlaneIntersectionStatus.OnPlane;
                }

                intersectionPoint = null;
                return LinePlaneIntersectionStatus.NoIntersection;
            }

            // They are not parallel
            // Compute intersect param
            var sI = n / d;
            if (sI < 0 || sI > 1)
            {
                intersectionPoint = null;
                return LinePlaneIntersectionStatus.NoIntersection;
            }

            intersectionPoint = line.StartPoint + u * sI; // Compute segment intersection point
            return LinePlaneIntersectionStatus.Point;
        }


        /// <summary>
        ///     Compute the intersection between a mesh face perimeter and a ray tangent to the face.
        /// </summary>
        /// <param name="ray">The tangent ray.</param>
        /// <param name="face">The mesh face.</param>
        /// <param name="result">The resulting intersection point.</param>
        /// <param name="halfEdge">The half-edge on where the intersection lies.</param>
        /// <returns>Intersection result.</returns>
        public static RayFacePerimeterIntersectionStatus RayFacePerimeter(
            Ray ray,
            MeshFace face,
            out Point3d result,
            out MeshHalfEdge halfEdge)
        {
            var faceNormal = MeshGeometry.FaceNormal(face);
            var biNormal = Vector3d.CrossProduct(ray.Direction, faceNormal);

            var perpPlane = new Plane(ray.Origin, ray.Direction, faceNormal, biNormal);

            var vertices = face.AdjacentVertices();

            var temp = new Point3d();

            var line = new Line(vertices[0], vertices[1]);
            if (LinePlane(line, perpPlane, out temp) != LinePlaneIntersectionStatus.Point)
            {
                result = null;
                halfEdge = null;
                return RayFacePerimeterIntersectionStatus.Point;
            } // No intersection found

            if (temp != ray.Origin && temp != null)
            {
                result = temp;
                halfEdge = null;
                return RayFacePerimeterIntersectionStatus.Point;
            } // Intersection found

            line = new Line(vertices[1], vertices[2]);
            if (LinePlane(line, perpPlane, out temp) != LinePlaneIntersectionStatus.Point)
            {
                result = null;
                halfEdge = null;
                return RayFacePerimeterIntersectionStatus.NoIntersection;
            }

            if (temp != ray.Origin && temp != null)
            {
                result = temp;
                halfEdge = null;
                return RayFacePerimeterIntersectionStatus.Point;
            }

            line = new Line(vertices[2], vertices[0]);
            if (LinePlane(line, perpPlane, out temp) != LinePlaneIntersectionStatus.Point)
            {
                result = null;
                halfEdge = null;
                return RayFacePerimeterIntersectionStatus.NoIntersection;
            }

            if (temp != ray.Origin && temp != null)
            {
                result = temp;
                halfEdge = null;
                return RayFacePerimeterIntersectionStatus.Point;
            }

            result = null;
            halfEdge = null;
            return RayFacePerimeterIntersectionStatus.Error;
        }


        /// <summary>
        ///     Compute the intersection between two 3-dimensional lines.
        /// </summary>
        /// <param name="lineA">First line to intersect.</param>
        /// <param name="lineB">Second line to intersect.</param>
        /// <param name="result">Struct containing the intersection result.</param>
        /// <returns>Returns an enum containing the intersection status.</returns>
        public static LineLineIntersectionStatus LineLine(
            Line lineA,
            Line lineB,
            out LineLineIntersectionResult result)
        {
            var u = lineA.EndPoint - lineA.StartPoint;
            var v = lineB.EndPoint - lineB.StartPoint;
            var w = lineA.StartPoint - lineB.StartPoint;
            var a = u.Dot(u); // always >= 0
            var b = u.Dot(v);
            var c = v.Dot(v); // always >= 0
            var d = u.Dot(w);
            var e = v.Dot(w);
            var d2 = a * c - b * b; // always >= 0
            double sc, sN, sD = d2; // sc = sN / sD, default sD = D >= 0
            double tc, tN, tD = d2; // tc = tN / tD, default tD = D >= 0

            // compute the line parameters of the two closest points
            if (d2 < Settings.Tolerance)
            {
                // the lines are almost parallel
                sN = 0.0; // force using point P0 on segment S1
                sD = 1.0; // to prevent possible division by 0.0 later
                tN = e;
                tD = c;
            }
            else
            {
                // get the closest points on the infinite lines
                sN = b * e - c * d;
                tN = a * e - b * d;
                if (sN < 0.0)
                {
                    // sc < 0 => the s=0 edge is visible
                    sN = 0.0;
                    tN = e;
                    tD = c;
                }
                else if (sN > sD)
                {
                    // sc > 1  => the s=1 edge is visible
                    sN = sD;
                    tN = e + b;
                    tD = c;
                }
            }

            if (tN < 0.0)
            {
                // tc < 0 => the t=0 edge is visible
                tN = 0.0;

                // recompute sc for this edge
                if (-d < 0.0)
                    sN = 0.0;
                else if (-d > a)
                    sN = sD;
                else
                {
                    sN = -d;
                    sD = a;
                }
            }
            else if (tN > tD)
            {
                // tc > 1  => the t=1 edge is visible
                tN = tD;

                // recompute sc for this edge
                if (-d + b < 0.0)
                    sN = 0;
                else if (-d + b > a)
                    sN = sD;
                else
                {
                    sN = -d + b;
                    sD = a;
                }
            }

            // finally do the division to get sc and tc
            sc = Math.Abs(sN) < Settings.Tolerance ? 0.0 : sN / sD;
            tc = Math.Abs(tN) < Settings.Tolerance ? 0.0 : tN / tD;

            // get the difference of the two closest points
            var dP = w + (sc * u - tc * v); // =  S1(sc) - S2(tc)
            result = default;
            result.Distance = dP.Length; // return the closest distance
            result.ParamA = sc;
            result.ParamB = tc;
            result.PointA = lineA.PointAt(sc);
            result.PointB = lineB.PointAt(tc);

            if (result.Distance <= Settings.Tolerance)
                return LineLineIntersectionStatus.Point;
            if (result.Distance > Settings.Tolerance)
                return LineLineIntersectionStatus.NoIntersection;
            return LineLineIntersectionStatus.Error;
        }
    }
}