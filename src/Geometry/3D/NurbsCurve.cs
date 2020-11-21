using System;
using System.Collections.Generic;
using System.Linq;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    /// </summary>
    public class NurbsCurve : BaseCurve
    {
        /// <summary>
        ///     The control points of the nurbs curve.
        /// </summary>
        public List<Point4d> ControlPoints;

        /// <summary>
        ///     The degree of the curve.
        /// </summary>
        public int Degree;

        /// <summary>
        ///     The nurbs curve knot vector.
        /// </summary>
        public List<double> Knots;


        /// <summary>
        ///     Initializes a new instance of <see cref="NurbsCurve" /> by it's control points and degree.
        /// </summary>
        /// <param name="controlPoints">The control points to create the curve with.</param>
        /// <param name="degree">The desired degree of the curve. Degree cannot be > (ControlPoints - 1)</param>
        public NurbsCurve(List<Point4d> controlPoints, int degree)
        {
            this.ControlPoints = controlPoints;
            this.Knots = NurbsCalculator.CreateUniformKnotVector(controlPoints.Count, degree)
                                        .ToList();
            this.Degree = degree;
        }


        /// <summary>
        ///     Gets the count of the control points - 1.
        /// </summary>
        private int N => this.ControlPoints.Count - 1;

        /// <summary>
        ///     The start point of the curve.
        /// </summary>
        public Point3d StartPoint => this.PointAt(this.Domain.Start);

        /// <summary>
        ///     The end point of the curve.
        /// </summary>
        public Point3d EndPoint => this.PointAt(this.Domain.End);

        /// <summary>
        ///     The tangent vector at the start of the curve.
        /// </summary>
        public Vector3d StartTangent => this.TangentAt(this.Domain.Start);

        /// <summary>
        ///     The tangent vector at the end of the curve.
        /// </summary>
        public Vector3d EndTangent => this.TangentAt(this.Domain.End);


        /// <summary>
        ///     Computes the specific amount of derivatives on the specified parameter.
        /// </summary>
        /// <param name="t">Parameter to compute derivatives at.</param>
        /// <param name="count">Number of derivatives to compute.</param>
        /// <returns>Array containing the </returns>
        private IList<Vector3d> DerivativesAt(double t, int count) =>
            NurbsCalculator.NurbsCurveDerivs(
                this.N,
                this.Degree,
                this.Knots,
                this.ControlPoints,
                t,
                count
            );


        /// <inheritdoc />
        public override Point3d PointAt(double t) =>
            NurbsCalculator.CurvePoint(this.N, this.Degree, this.Knots, this.ControlPoints, t);


        /// <inheritdoc />
        public override Vector3d TangentAt(double t) => this.DerivativesAt(t, 1)[1].Unit();


        /// <inheritdoc />
        public override Vector3d NormalAt(double t) => this.DerivativesAt(t, 2)[2].Unit();


        /// <inheritdoc />
        public override Vector3d BinormalAt(double t) => this.DerivativesAt(t, 3)[3].Unit();


        /// <inheritdoc />
        public override Plane FrameAt(double t)
        {
            var ders = this.DerivativesAt(t, 3);
            return new Plane(( Point3d ) ders[0], ders[1], ders[2], ders[3]);
        }


        /// <inheritdoc />
        public override bool CheckValidity() => true;


        /// <inheritdoc />
        protected override double ComputeLength() => throw new NotImplementedException();
    }
}