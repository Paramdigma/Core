using System;
using System.Collections.Generic;
using Paramdigma.Core.Collections;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    /// Contains all methods related to 'The Nurbs Book 2nd Edition' implementation of NURBS curves and surfaces.
    /// </summary>
    public static class NurbsCalculator
    {
        /// <summary>
        /// Constructs a Unit knot vector given a point count and degree.
        /// </summary>
        /// <param name="controlPointCount">Ammount of control points in the curve.</param>
        /// <param name="degree">Degree of the curve.</param>
        /// <returns></returns>
        public static double[] CreateUnitKnotVector(int controlPointCount, int degree)
        {
            if (degree > controlPointCount)
                throw new Exception("Degree cannot be bigger than 'ControlPoints - 1'");
            var knotVector = new double[controlPointCount + degree + 2];
            for (int i = 0; i <= degree; i++)
                knotVector[i] = 0.0;
            for (int i = degree + 1; i < controlPointCount + 1; i++)
                knotVector[i] = ((double)i - degree) / (controlPointCount - degree + 1);
            for (int i = controlPointCount + 1; i < controlPointCount + degree + 2; i++)
                knotVector[i] = 1.0;
            return knotVector;
        }

        /// <summary>
        /// Compute a point on a power basis curve.
        /// </summary>
        /// <param name="points">Curve points.</param>
        /// <param name="degree">Curve degree.</param>
        /// <param name="t">Parameter.</param>
        /// <returns>Computed point on curve.</returns>
        public static Point3d Horner1(Point3d[] points, int degree, double t)
        {
            var c = points[degree];
            for (int i = degree - 1; i >= 0; i--)
                c = (c * t) + points[i];
            return c;
        }

        /// <summary>
        /// Compute the value of a Bernstein polynomial.
        /// </summary>
        /// <param name="index">Index of point to compute polynomial of.</param>
        /// <param name="degree">Degree of curve.</param>
        /// <param name="t">Parameter.</param>
        /// <returns></returns>
        public static double Bernstein(int index, int degree, double t)
        {
            var temp = new List<double>();
            for (int j = 0; j <= degree; j++)
            {
                temp.Add(0.0);
            }

            temp[degree - index] = 1.0;

            var u1 = 1.0 - t;
            for (var k = 1; k <= degree; k++)
            {
                for (var j = degree; j >= k; j--)
                {
                    temp[j] = (u1 * temp[j]) + (t * temp[j - 1]);
                }
            }

            return temp[degree];
        }

        /// <summary>
        /// Compute point on a power basis surface.
        /// </summary>
        /// <param name="controlPoints">Control point matrix.</param>
        /// <param name="degreeU">Surface degree in the U direction.</param>
        /// <param name="degreeV">Surface degree in the V direction.</param>
        /// <param name="u">U parameter to compute.</param>
        /// <param name="v">V parameter to compute.</param>
        /// <returns>Computed point on the surface.</returns>
        public static Point3d Horner2(Matrix<Point3d> controlPoints, int degreeU, int degreeV, double u, double v)
        {
            var b = new Point3d[degreeU];
            for (int i = 0; i <= degreeU; i++)
                b[i] = Horner1(controlPoints.Row(i), degreeV, v);
            return Horner1(b, degreeU, u);
        }

        /// <summary>
        /// Compute all nth-degree Bernstein polynomials.
        /// </summary>
        /// <param name="degree">Curve degree.</param>
        /// <param name="t">Parameter.</param>
        /// <returns></returns>
        public static double[] AllBernstein(int degree, double t)
        {
            var b = new double[degree + 1];
            b[0] = 1.0;
            double u1 = 1.0 - t;
            for (int j = 1; j <= degree; j++)
            {
                var saved = 0.0;
                for (int k = 0; k < j; k++)
                {
                    var temp = b[k];
                    b[k] = saved + (u1 * temp);
                    saved = t * temp;
                }

                b[j] = saved;
            }

            return b;
        }

        /// <summary>
        /// Compute point on Bezier curve.
        /// </summary>
        /// <param name="controlPoints">Control ponts of the curve.</param>
        /// <param name="degree">Curve degree.</param>
        /// <param name="t">Parameter to compute.</param>
        /// <returns></returns>
        public static Point3d PointOnBezierCurve(List<Point3d> controlPoints, int degree, double t)
        {
            Point3d c = new Point3d();

            double[] b = AllBernstein(degree, t);
            for (int k = 0; k <= degree; k++)
                c += b[k] * controlPoints[k];

            return c;
        }

        /// <summary>
        /// Compute point on a Bézier curve by deCasteljau.
        /// </summary>
        /// <param name="controlPoints">Control points of the curve.</param>
        /// <param name="degree">Curve degree.</param>
        /// <param name="t">Parameter of point to compute.</param>
        /// <returns>Computed point along the Bézier curve.</returns>
        public static Point3d DeCasteljau1(Point3d[] controlPoints, int degree, double t)
        {
            var q = new Point3d[degree + 1];
            for (var i = 0; i <= degree; i++)
            {
                q[i] = new Point3d(controlPoints[i]);
            }

            for (var k = 1; k <= degree; k++)
            {
                for (var i = 0; i <= degree - k; i++)
                {
                    q[i] = ((1.0 - t) * q[i]) + (t * q[i + 1]);
                }
            }

            return q[0];
        }

        /// <summary>
        /// Compute a point on a Bézier surface by deCasteljau.
        /// </summary>
        /// <param name="controlPoints">Control points of the curve.</param>
        /// <param name="degreeU">Surface degree in the U direction.</param>
        /// <param name="degreeV">Surface degree in the V direction.</param>
        /// <param name="u">U parameter to compute.</param>
        /// <param name="v">V parameter to compute.</param>
        /// <returns>The computed point.</returns>
        public static Point3d DeCasteljau2(Matrix<Point3d> controlPoints, int degreeU, int degreeV, double u, double v)
        {
            var q = new List<Point3d>();
            if (degreeU <= degreeV)
            {
                for (var j = 0; j <= degreeV; j++)
                {
                    q.Add(DeCasteljau1(controlPoints.Row(j), degreeU, u));
                }

                return DeCasteljau1(q.ToArray(), degreeV, v);
            }
            else
            {
                for (int i = 0; i <= degreeU; i++)
                    q.Add(DeCasteljau1(controlPoints.Column(i), degreeU, u));
                return DeCasteljau1(q.ToArray(), degreeU, u);
            }
        }

        /// <summary>
        /// Determine the knot span index.
        /// </summary>
        /// <param name="n">Degree.</param>
        /// <param name="degree">????.</param>
        /// <param name="t">Paramter.</param>
        /// <param name="knotVector">Knot vector.</param>
        /// <returns>The knot span index.</returns>
        public static int FindSpan(int n, int degree, double t, IList<double> knotVector)
        {
            if (t == knotVector[n + 1])
            {
                return n;
            }

            var low = degree;
            var high = n + 1;
            var mid = (low + high) / 2;
            while (t < knotVector[mid] || t >= knotVector[mid + 1])
            {
                if (t < knotVector[mid])
                {
                    high = mid;
                }
                else
                {
                    low = mid;
                }

                mid = (low + high) / 2;
            }

            return mid;
        }

        /// <summary>
        /// Compute all non-zero basis functions of all degrees from 0 to "degree".
        /// </summary>
        /// <param name="span">Knot span index.</param>
        /// <param name="param">Parameter to compute.</param>
        /// <param name="degree">Degree.</param>
        /// <param name="knotVector">The knot vector.</param>
        /// <returns>List with all non-zero basis functions up to the specified degree.</returns>
        public static double[,] AllBasisFuns(int span, double param, int degree, IList<double> knotVector)
        {
            var n = new double[degree + 1, degree + 1];
            for (int i = 0; i <= degree; i++)
            {
                for (int j = 0; j <= i; j++)
                    n[j, i] = OneBasisFun(degree, knotVector.Count - 1, knotVector, span - i + j, param);
            }

            return n;
        }

        /// <summary>
        /// Computes the basis functions of a span.
        /// </summary>
        /// <param name="span">Knot span.</param>
        /// <param name="param">Parameter to compute.</param>
        /// <param name="degree">Degree.</param>
        /// <param name="knotVector">Knot vector.</param>
        /// <returns>List of the basis functions of the specific span.</returns>
        public static double[] BasisFuns(int span, double param, int degree, IList<double> knotVector)
        {
            var basisFunctions = new double[degree + 1];
            var left = new double[degree + 1];
            var right = new double[degree + 1];
            basisFunctions[0] = 1.0;
            for (var j = 1; j <= degree; j++)
            {
                left[j] = param - knotVector[span + 1 - j];
                right[j] = knotVector[span + j] - param;
                var saved = 0.0;
                for (var r = 0; r < j; r++)
                {
                    var temp = basisFunctions[r] / (right[r + 1] + left[j - r]);
                    basisFunctions[r] = saved + (right[r + 1] * temp);
                    saved = left[j - r] * temp;
                }

                basisFunctions[j] = saved;
            }

            return basisFunctions;
        }

        /// <summary>
        /// Compute nonzero basis functions and their derivatives at a specified parameter.
        /// </summary>
        /// <param name="span">Knot span.</param>
        /// <param name="param">Parameter.</param>
        /// <param name="degree">Degree.</param>
        /// <param name="n">Derivatives to compute.</param>
        /// <param name="knotVector">Knot vector.</param>
        /// <returns>Multidimensional array holding the basis functions and their derivatives for that parameter.</returns>
        public static Matrix<double> DersBasisFuns(int span, double param, int degree, int n, IList<double> knotVector)
        {
            var ders = new Matrix<double>(n, degree);
            var ndu = new double[degree + 1, degree + 1];
            var a = new double[2, degree + 1];
            var left = new double[degree + 1];
            var right = new double[degree + 1];

            ndu[0, 0] = 1.0;
            for (int j = 1; j <= degree; j++)
            {
                left[j] = param - knotVector[span + 1 - j];
                right[j] = knotVector[span + j] - param;
                var saved = 0.0;
                for (int r = 0; r < j; r++)
                {
                    ndu[j, r] = right[r + 1] + left[j - r];
                    var temp = ndu[r, j - 1] / ndu[j, r];
                    ndu[r, j] = saved + (right[r + 1] * temp);
                    saved = left[j - r] * temp;
                }

                ndu[j, j] = saved;
            }

            for (int j = 0; j <= degree; j++)
                ders[0, j] = ndu[j, degree];

            for (int r = 0; r <= degree; r++)
            {
                var s1 = 0;
                var s2 = 1;
                a[0, 0] = 1.0;
                for (int k = 1; k <= n; k++)
                {
                    var d = 0.0;
                    var rk = r - k;
                    var pk = degree - k;
                    if (r >= k)
                    {
                        a[s2, 0] = a[s1, 0] / ndu[pk + 1, rk];
                        d = a[s2, 0] * ndu[rk, pk];
                    }

                    int j1 = (rk >= -1) ? 1 : -rk;
                    int j2 = (r - 1 <= pk) ? k - 1 : degree - r;

                    for (int j = j1; j <= j2; j++)
                    {
                        a[s2, j] = (a[s1, j] - a[s1, j - 1]) / ndu[pk + 1, rk + j];
                        d += a[s2, j] * ndu[rk + j, pk];
                    }

                    if (r <= pk)
                    {
                        a[s2, k] = -a[s1, k - 1] / ndu[pk + 1, r];
                        d += a[s2, k] * ndu[r, pk];
                    }

                    ders[k, r] = d;

                    // Switch rows
                    var temp = s1;
                    s1 = s2;
                    s2 = temp;
                }
            }

            var r0 = degree;
            for (var k = 1; k <= n; k++)
            {
                for (var j = 0; j <= degree; j++)
                    ders[k, j] *= r0;
                r0 *= degree - k;
            }

            return ders;
        }

        /// <summary>
        /// Compute the basis function 'Nip'.
        /// </summary>
        /// <param name="degree">Degree.</param>
        /// <param name="m">The high index of the knot vector.</param>
        /// <param name="knotVector">Knot vector.</param>
        /// <param name="span">Knot span index.</param>
        /// <param name="param">Parameter to compute.</param>
        /// <returns></returns>
        public static double OneBasisFun(int degree, int m, IList<double> knotVector, int span, double param)
        {
            if ((span == 0 && param == knotVector[0]) || (span == (m - degree - 1) && param == knotVector[m]))
                return 1.0;

            if (param < knotVector[span] || param >= knotVector[span + degree + 1])
                return 0.0;

            // Initialize zeroth-degree functions
            var n = new double[degree + 1];
            for (int j = 0; j <= degree; j++)
            {
                if (param >= knotVector[span + j] && param < knotVector[span + j + 1])
                    n[j] = 1.0;
                else
                    n[j] = 0.0;
            }

            for (int k = 1; k <= degree; k++)
            {
                double saved = n[0] == 0.0 ? 0.0 : (param - knotVector[span]) * n[0] / (knotVector[span + k] - knotVector[span]);
                for (int j = 0; j < (degree - k + 1); j++)
                {
                    var uLeft = knotVector[span + j + 1];
                    var uRight = knotVector[span + j + k + 1];
                    if (n[j + 1] == 0.0)
                    {
                        n[j] = saved;
                        saved = 0.0;
                    }
                    else
                    {
                        var temp = n[j + 1] / (uRight - uLeft);
                        n[j] = saved + ((uRight - param) * temp);
                        saved = (param - uLeft) * temp;
                    }
                }
            }

            return n[0];
        }

        /// <summary>
        /// Compute derivatives of basis function 'Nip'.
        /// </summary>
        /// <param name="p"></param>
        /// <param name="m"></param>
        /// <param name="knotVector"></param>
        /// <param name="i"></param>
        /// <param name="u"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static double[] DersOneBasisFun(int p, int m, IList<double> knotVector, int i, double u, int n)
        {
            // TODO: Check unused m parameter.
            var ders = new double[n + 1];
            if (u < knotVector[i] || u >= knotVector[i + p + 1])
            {
                for (int k = 0; k <= n; k++)
                    ders[k] = 0.0;
                return ders;
            }

            var tmpN = new double[p + 1, p + 1];
            for (int j = 0; j <= p; j++)
            {
                if (u >= knotVector[i + j] && u < knotVector[i + j + 1])
                    tmpN[j, 0] = 1.0;
            }

            for (int k = 1; k <= p; k++)
            {
                double saved;
                if (tmpN[0, k - 1] == 0.0)
                    saved = 0.0;
                else
                    saved = (u - knotVector[i]) * tmpN[0, k - 1] / (knotVector[i + k] - knotVector[i]);

                for (int j = 0; j < p - k + 1; j++)
                {
                    double uLeft = knotVector[i + j + 1];
                    double uRight = knotVector[i + j + k + 1];
                    if (tmpN[j + 1, k - 1] == 0.0)
                    {
                        tmpN[j, k] = saved;
                        saved = 0.0;
                    }
                    else
                    {
                        double temp = tmpN[j + 1, k - 1] / (uRight - uLeft);
                        tmpN[j, k] = saved + ((uRight - u) * temp);
                        saved = (u - uLeft) * temp;
                    }
                }
            }

            ders[0] = tmpN[0, p];

            double[] tempND = new double[n + 1];
            for (int k = 1; k <= n; k++)
            {
                for (int j = 0; j <= k; j++)
                {
                    tempND[j] = tmpN[j, p - k];
                }

                for (int jj = 1; jj <= k; jj++)
                {
                    double saved;
                    if (tempND[0] == 0.0)
                        saved = 0.0;
                    else
                        saved = tempND[0] / (knotVector[i + p - k + jj] - knotVector[i]);
                    for (int j = 0; j < k - jj + 1; j++)
                    {
                        double uLeft = knotVector[i + j + 1];
                        double uRight = knotVector[i + j + p + jj + 1];
                        if (tempND[j + 1] == 0.0)
                        {
                            tempND[j] = (p - k + jj) * saved;
                            saved = 0.0;
                        }
                        else
                        {
                            double temp = tempND[j + 1] / (uRight - uLeft);
                            tempND[j] = (p - k + jj) * (saved - temp);
                            saved = temp;
                        }
                    }
                }

                ders[k] = tempND[0];
            }

            return ders;
        }

        public static Point3d CurvePoint(int n, int p, IList<double> knotVector, IList<Point3d> controlPoints, double u)
        {
            int span = FindSpan(n, p, u, knotVector);
            double[] basisFuns = BasisFuns(span, u, p, knotVector);
            Point3d c = Point3d.Unset;
            for (int i = 0; i <= p; i++)
                c += basisFuns[i] * controlPoints[span - p + i];
            return c;
        }

        public static Vector3d[] CurveDerivsAlg1(int n, int p, IList<double> knotVector, IList<Point3d> controlPoints, double u, int d)
        {
            var ck = new Vector3d[d + 1];
            var du = Math.Min(d, p);
            for (int k = p + 1; k <= d; k++)
                ck[k] = new Vector3d();
            var span = FindSpan(n, p, u, knotVector);
            var nders = DersBasisFuns(span, u, p, du, knotVector);
            for (int k = 0; k <= du; k++)
            {
                ck[k] = new Vector3d();
                for (int j = 0; j <= p; j++)
                    ck[k] += nders[k, j] * (Vector3d)controlPoints[span - p + j];
            }

            return ck;
        }

        public static Point3d[,] CurveDerivCpts(int n, int p, IList<double> knotVector, IList<Point3d> controlPoints, int d, int r1, int r2)
        {
            var r = r2 - r1;
            var pk = new Point3d[d + 1, r];
            for (int i = 0; i <= r; i++)
                pk[0, i] = controlPoints[r1 + i];
            for (int k = 1; k <= d; k++)
            {
                var tmp = (p - k) + 1;
                for (int i = 0; i <= r - k; i++)
                {
                    pk[k, i] = (tmp * (Point3d)(pk[k - 1, i + 1] - pk[k - 1, i])) / (knotVector[r1 + i + p + 1] - knotVector[r1 + i + k]);
                }
            }

            return pk;
        }

        public static Vector3d[] CurveDerivsAlg2(int n, int p, IList<double> knotVector, IList<Point3d> controlPoints, double u, int d)
        {
            var du = Math.Min(d, p);
            var ck = new Vector3d[d + 1];
            for (int k = p + 1; k <= d; k++)
                ck[k] = new Vector3d();
            var span = FindSpan(n, p, u, knotVector);
            var basisFuns = AllBasisFuns(span, u, p, knotVector);
            var pk = CurveDerivCpts(n, p, knotVector, controlPoints, du, span - p, span);
            for (int k = 0; k <= du; k++)
            {
                ck[k] = new Vector3d();
                for (int j = 0; j <= p - k; j++)
                    ck[k] += basisFuns[j, p - k] * (Vector3d)pk[k, j];
            }

            return ck;
        }

        // B-Spline Surfaces
        public static Point3d SurfacePoint(int n, int p, IList<double> knotVectorU, int m, int q, IList<double> knotVectorV, Matrix<Point3d> controlPoints, double u, double v)
        {
            var uspan = FindSpan(n, p, u, knotVectorU);
            var nU = BasisFuns(uspan, u, p, knotVectorU);
            var vspan = FindSpan(m, q, v, knotVectorV);
            var nV = BasisFuns(vspan, v, q, knotVectorV);
            var uind = uspan - p;
            var surfPt = Point3d.Unset;
            for (int l = 0; l <= q; l++)
            {
                var temp = Point3d.Unset;
                var vind = vspan - q - l;
                for (int k = 0; k <= p; k++)
                    temp += nU[k] * controlPoints[uind + k, vind];
                surfPt += nV[l] * temp;
            }

            return surfPt;
        }

        public static Point3d[,] SurfaceDerivsAlg1(int n, int p, IList<double> knotVectorU, int m, int q, IList<double> knotVectorV, Matrix<Point3d> controlPoints, double u, double v, int derivCount)
        {
            var sKL = new Point3d[derivCount + 1, derivCount + 1];
            var du = Math.Min(derivCount, p);
            for (var k = p + 1; k <= derivCount; k++)
            {
                for (var l = 0; l <= derivCount - k; l++)
                {
                    sKL[k, l] = Point3d.Unset;
                }
            }

            var dv = Math.Min(derivCount, q);
            for (var l = q + 1; l <= derivCount; l++)
            {
                for (var k = 0; k <= derivCount - 1; k++)
                {
                    sKL[k, l] = Point3d.Unset;
                }
            }

            var uSpan = FindSpan(n, p, u, knotVectorU);
            var nU = DersBasisFuns(uSpan, u, p, du, knotVectorU);
            var vSpan = FindSpan(m, q, v, knotVectorV);
            var nV = DersBasisFuns(vSpan, v, q, dv, knotVectorV);

            for (var k = 0; k <= du; k++)
            {
                var temp = new Point3d[q];
                for (var s = 0; s <= q; s++)
                {
                    temp[s] = Point3d.Unset;
                    for (var r = 0; r <= p; r++)
                    {
                        temp[s] += nU[k, r] * controlPoints[uSpan - p + r, vSpan - q + s];
                    }

                    var dd = Math.Min(derivCount - k, dv);

                    for (var l = 0; l <= dd; l++)
                    {
                        sKL[k, l] = Point3d.Unset;

                        // TODO: Check ss, this was changed from 's' for naming conflicts but it might have been on purpose.
                        for (var ss = 0; ss <= q; ss++)
                        {
                            sKL[k, l] += nV[l, ss] * temp[ss];
                        }
                    }
                }
            }

            return sKL;
        }

        public static Point3d[][][][] SurfaceDerivCpts(int n, int p, IList<double> knotVectorU, int m, int q, IList<double> knotVectorV, Matrix<Point3d> controlPoints, int d, int r1, int r2, int s1, int s2)
        {
            var pkl = new Point3d[d][][][];

            var du = Math.Min(d, p);
            var dv = Math.Min(d, q);
            var r = r2 - r1;
            var s = s2 - s1;

            for (var j = s1; j <= s2; j++)
            {
                var temp = CurveDerivCpts(n, p, knotVectorU, controlPoints.Column(j), du, r1, r2);
                for (var k = 0; k <= du; k++)
                {
                    for (int i = 0; i <= r - k; i++)
                    {
                        pkl[k][0][i][j - s1] = temp[k, i];
                    }
                }
            }

            for (var k = 0; k <= du; k++)
            {
                for (var i = 0; i <= r - k; i++)
                {
                    var dd = Math.Min(d - k, dv);
                    var temp = CurveDerivCpts(m, q, knotVectorV, pkl[k][0][i], dd, 0, s);
                    for (var l = i; k <= dd; k++)
                    {
                        for (var j = 0; j <= s - l; j++)
                        {
                            pkl[k][l][i][j] = temp[l, j];
                        }
                    }
                }
            }

            return pkl;
        }

        public static Point3d[,] SurfaceDerivsAlg2(int n, int p, IList<double> knotVectorU, int m, int q, IList<double> knotVectorV, Matrix<Point3d> controlPoints, double u, double v, int d)
        {
            var skl = new Point3d[d + 1, d + 1];

            var du = Math.Min(d, p);
            for (var k = p + 1; k <= d; k++)
            {
                for (var l = 0; l <= d - k; l++)
                {
                    skl[k, l] = Point3d.Unset;
                }
            }

            var dv = Math.Min(d, q);
            for (var l = q + 1; l <= d; l++)
            {
                for (var k = 0; k <= d - l; k++)
                {
                    skl[k, l] = Point3d.Unset;
                }
            }

            var uSpan = FindSpan(n, p, u, knotVectorU);
            var nV = AllBasisFuns(uSpan, u, p, knotVectorU);
            var vSpan = FindSpan(m, q, v, knotVectorV);
            var nU = AllBasisFuns(vSpan, v, q, knotVectorV);
            var pkl = SurfaceDerivCpts(n, p, knotVectorU, m, q, knotVectorV, controlPoints, d, uSpan - p, uSpan, vSpan - q, vSpan);

            for (var k = 0; k <= du; k++)
            {
                var dd = Math.Min(d - k, dv);
                for (var l = 0; l <= dd; l++)
                {
                    skl[k, l] = Point3d.Unset;
                    for (var i = 0; i <= q - l; i++)
                    {
                        var tmp = Point3d.Unset;
                        for (var j = 0; j <= p - k; j++)
                            tmp += nV[j, p - k] * pkl[k][l][j][i];
                        skl[k, l] += nU[i, q - l] * tmp;
                    }
                }
            }

            return skl;
        }

        // Nubs methods
        public static Point4d CurvePoint(int n, int p, IList<double> knotVector, IList<Point4d> controlPoints, double u)
        {
            var span = FindSpan(n, p, u, knotVector);
            var N = BasisFuns(span, u, p, knotVector);
            var cw = new Point4d();
            for (int j = 0; j <= p; j++)
            {
                cw += N[j] * controlPoints[span - p + j];
            }

            return cw / cw.Weight;
        }

        /// <summary>
        /// Compute C(u) derivatives from Cw(u) derivatives.
        /// </summary>
        /// <param name="aDers"></param>
        /// <param name="wDers"></param>
        /// <param name="d"></param>
        /// <returns></returns>
        public static Vector3d[] RatCurveDerivs(IList<Vector3d> aDers, IList<double> wDers, int d)
        {
            var ders = new Vector3d[d + 1];
            double[,] bin = null; // TODO: Precompute 'binomial coefficients'
            for (int k = 0; k <= d; k++)
            {
                var v = aDers[k];
                for (int i = 1; i <= k; i++)
                {
                    v -= bin[k, i] * wDers[i] * ders[k - i];
                }

                ders[k] = v / wDers[0];
            }

            return ders;
        }

        /// <summary>
        /// Compute a point on a nurbs surface.
        /// </summary>
        /// <param name="n"></param>
        /// <param name="p"></param>
        /// <param name="knotVectorU"></param>
        /// <param name="m"></param>
        /// <param name="q"></param>
        /// <param name="knotVectorV"></param>
        /// <param name="controlPoints"></param>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static Point3d SurfacePoint(int n, int p, IList<double> knotVectorU, int m, int q, IList<double> knotVectorV, Matrix<Point4d> controlPoints, double u, double v)
        {
            var uspan = FindSpan(n, p, u, knotVectorU);
            var nU = BasisFuns(uspan, u, p, knotVectorU);
            var vspan = FindSpan(m, q, v, knotVectorV);
            var nV = BasisFuns(vspan, v, q, knotVectorV);

            var temp = new Point4d[q];
            for (var l = 0; l <= q; l++)
            {
                temp[l] = new Point4d();
                for (var k = 0; k <= p; k++)
                {
                    temp[l] += nU[k] * controlPoints[uspan - p + k, vspan - q + l];
                }
            }

            var sW = new Point4d();
            for (var l = 0; l <= q; l++)
            {
                sW += nV[l] * temp[l];
            }

            return (Point3d)(sW / sW.Weight);
        }
    }
}