using System;
using System.Collections.Generic;
using Paramdigma.Core.Geometry;

namespace Paramdigma.Core.LinearAlgebra
{
    /// <summary>
    ///     Fit a line through a set of 2-dimensional points.
    /// </summary>
    public static class LeastSquaresLinearFit
    {
        // Find the least squares linear fit.
        // Return the total error.
        // Found at: http://csharphelper.com/blog/2014/10/find-a-linear-least-squares-fit-for-a-set-of-points-in-c/

        /// <summary>
        ///     Find the least squares best fitting line to the given points.
        /// </summary>
        /// <param name="points">The points to fit the line through.</param>
        /// <param name="m">Height.</param>
        /// <param name="b">Slope.</param>
        /// <returns></returns>
        public static double FindLinearLeastSquaresFit(
            List<Point2d> points, out double m, out double b)
        {
            // Perform the calculation.
            // Find the values S1, Sx, Sy, Sxx, and Sxy.
            double s1 = points.Count;
            double sx = 0, sy = 0, sxx = 0, sxy = 0;

            foreach (var pt in points)
            {
                sx += pt.X;
                sy += pt.Y;
                sxx += pt.X * pt.X;
                sxy += pt.X * pt.Y;
            }

            // Solve for m and b.
            m = ((sxy * s1) - (sx * sy)) / ((sxx * s1) - (sx * sx));
            b = ((sxy * sx) - (sy * sxx)) / ((sx * sx) - (s1 * sxx));

            return Math.Sqrt(ErrorSquared(points, m, b));
        }

        // Return the error squared.
        private static double ErrorSquared(List<Point2d> points, double m, double b)
        {
            double total = 0;
            foreach (var pt in points)
            {
                var dy = pt.Y - ((m * pt.X) + b);
                total += dy * dy;
            }

            return total;
        }
    }
}