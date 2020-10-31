using System;
using System.Collections.Generic;
using System.Linq;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    /// </summary>
    public class NurbsCurve : BaseCurve
    {
        public List<Point3d> ControlPoints;
        public int Degree;
        public List<double> Knots;

        public NurbsCurve(List<Point3d> controlPoints, int degree)
        {
            this.ControlPoints = controlPoints;
            this.Knots = NurbsCalculator.CreateUnitKnotVector(controlPoints.Count, degree).ToList();
            this.Degree = degree;
        }

        /// <summary>
        ///     Gets the count of the control points - 1.
        /// </summary>
        private int n => this.ControlPoints.Count - 1;

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
            NurbsCalculator.CurveDerivsAlg1(
                this.n,
                this.Degree,
                this.Knots,
                this.ControlPoints,
                t,
                count
            );


        /// <inheritdoc />
        public override Point3d PointAt(double t) =>
            NurbsCalculator.CurvePoint(this.n, this.Degree, this.Knots, this.ControlPoints, t);

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
            return new Plane((Point3d)ders[0], ders[1], ders[2], ders[3]);
        }

        /// <inheritdoc />
        public override bool CheckValidity() => true;

        /// <inheritdoc />
        protected override double ComputeLength() => throw new NotImplementedException();
    }
}