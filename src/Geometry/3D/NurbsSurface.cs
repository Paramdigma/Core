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
        public Matrix<Point4d> ControlPoints;

        public int DegreeU;
        
        public int DegreeV;

        public List<double> KnotsU;

        public List<double> KnotsV;


        public NurbsSurface(Matrix<Point4d> controlPoints, int degreeU, int degreeV)
        {
            this.ControlPoints = controlPoints;
            this.DegreeU = degreeU;
            this.DegreeV = degreeV;
            this.KnotsU = NurbsCalculator.CreateUnitKnotVector(this.ControlPoints.N, this.DegreeU)
                                         .ToList();
            this.KnotsV = NurbsCalculator.CreateUnitKnotVector(this.ControlPoints.M, this.DegreeV)
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
        public Vector3d NormalAt(double u, double v) => throw new System.NotImplementedException();


        /// <inheritdoc />
        public Plane FrameAt(double u, double v) => throw new System.NotImplementedException();


        /// <inheritdoc />
        public double DistanceTo(Point3d point) => throw new System.NotImplementedException();


        /// <inheritdoc />
        public Point3d ClosestPointTo(Point3d point) => throw new System.NotImplementedException();
    }
}