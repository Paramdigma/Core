using System;
using System.Collections.Generic;
using System.Linq;
using Paramdigma.Core.Collections;
using Paramdigma.Core.Geometry.Interfaces;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    ///     Represents a NURBS surface. Contains properties and methods for operating with NURBS surfaces.
    /// </summary>
    public class NurbsSurface : ISurface
    {
        /// <summary>
        ///     The surface's grid of control points.
        /// </summary>
        public readonly Matrix<Point4d> ControlPoints;

        /// <summary>
        ///     Surface's degree in the U direction
        /// </summary>
        public readonly int DegreeU;

        /// <summary>
        ///     Surface's degree in the V direction
        /// </summary>
        public readonly int DegreeV;

        /// <summary>
        ///     Knot vector in the U direction.
        /// </summary>
        public readonly List<double> KnotsU;

        /// <summary>
        ///     Knot vector in the V direction.
        /// </summary>
        public readonly List<double> KnotsV;


        /// <summary>
        ///     Initializes a new instance of <see cref="NurbsSurface" /> by it's control points and degrees.
        /// </summary>
        /// <param name="controlPoints">Grid of control points for the surface.</param>
        /// <param name="degreeU">Degree of the surface in the U direction.</param>
        /// <param name="degreeV">Degree of the surface in the V direction.</param>
        public NurbsSurface(Matrix<Point4d> controlPoints, int degreeU, int degreeV)
        {
            this.ControlPoints = controlPoints;

            this.DegreeU = degreeU;
            this.DegreeV = degreeV;

            this.KnotsU = NurbsCalculator
                         .CreateUniformKnotVector(this.ControlPoints.N, this.DegreeU)
                         .ToList();
            this.KnotsV = NurbsCalculator
                         .CreateUniformKnotVector(this.ControlPoints.M, this.DegreeV)
                         .ToList();
        }


        /// <inheritdoc />
        public Interval DomainU => new Interval(this.KnotsU[0], this.KnotsU[this.KnotsU.Count - 1]);

        /// <inheritdoc />
        public Interval DomainV => new Interval(this.KnotsV[0], this.KnotsV[this.KnotsV.Count - 1]);


        /// <inheritdoc />
        public Point3d PointAt(double u, double v) => NurbsCalculator.SurfacePoint(
            this.ControlPoints.N - 1,
            this.DegreeU,
            this.KnotsU,
            this.ControlPoints.M - 1,
            this.DegreeV,
            this.KnotsV,
            this.ControlPoints,
            u,
            v);


        /// <inheritdoc />
        public Vector3d NormalAt(double u, double v) => throw new NotImplementedException();


        /// <inheritdoc />
        public Plane FrameAt(double u, double v) => throw new NotImplementedException();


        /// <inheritdoc />
        public double DistanceTo(Point3d point) => throw new NotImplementedException();


        /// <inheritdoc />
        public Point3d ClosestPointTo(Point3d point) => throw new NotImplementedException();


        /// <summary>
        ///     Creates a square flat surface on the XY Plane.
        /// </summary>
        /// <param name="xDimension">Dimension interval on the X direction.</param>
        /// <param name="yDimension">Dimension interval on the Y direction.</param>
        /// <param name="xCount">Number of points on the X direction.</param>
        /// <param name="yCount">Number of points on the Y direction.</param>
        /// <returns>Flat nurbs surface.</returns>
        public static NurbsSurface CreateFlatSurface(
            Interval xDimension,
            Interval yDimension,
            int xCount,
            int yCount)
        {
            var m = new Matrix<Point4d>(xCount, yCount);
            for (var i = 0; i < xCount; i++)
            {
                for (var j = 0; j < yCount; j++)
                {
                    m[i, j] = new Point4d(
                        xDimension.RemapFromUnit(( double ) i / xCount),
                        yDimension.RemapFromUnit(( double ) j / yCount),
                        0,
                        1);
                }
            }

            var degreeU = xCount <= 3 ? xCount - 1 : 3;
            var degreeV = yCount <= 3 ? yCount - 1 : 3;
            return new NurbsSurface(m, degreeU, degreeV);
        }


        /// <summary>
        ///     Computes the derivatives at at S(u,v).
        /// </summary>
        /// <param name="u">U parameter to compute at.</param>
        /// <param name="v">V parameter to compute at.</param>
        /// <param name="count">Number of derivatives to compute.</param>
        /// <returns>Computed derivatives.</returns>
        public Matrix<Vector3d> DerivativesAt(double u, double v, int count) =>
            NurbsCalculator.NurbsSurfaceDerivs(
                this.ControlPoints.M - 1,
                this.DegreeU,
                this.KnotsU,
                this.ControlPoints.M - 1,
                this.DegreeV,
                this.KnotsV,
                this.ControlPoints,
                u,
                v,
                count);
    }
}