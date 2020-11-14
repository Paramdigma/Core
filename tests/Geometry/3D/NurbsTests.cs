using System.Collections.Generic;
using System.Diagnostics;
using Paramdigma.Core.Collections;
using Paramdigma.Core.Geometry;
using Xunit;
using Xunit.Abstractions;

namespace Paramdigma.Core.Tests
{
    public class NurbsTests
    {
        private readonly ITestOutputHelper testOutputHelper;


        public NurbsTests(ITestOutputHelper testOutputHelper) =>
            this.testOutputHelper = testOutputHelper;


        public Matrix<Point3d> FlatGrid(int size)
        {
            var m = new Matrix<Point3d>(size);
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                    m[i, j] = new Point3d(i, j, 0);
            }

            return m;
        }


        [Theory]
        [InlineData(0.0, 0.0)]
        public void Decasteljau2_Works(double u, double v)
        {
            const int n = 5;
            var points = this.FlatGrid(n);
            var pt = NurbsCalculator.DeCasteljau2(points, 3, 3, u, v);

            Assert.NotNull(pt);
        }


        [Fact]
        public void Decasteljau1_Works()
        {
            var points = new List<Point3d>();
            for (var i = 0; i < 5; i++)
                points.Add(new Point3d(i, 0, 0));
            var pt = NurbsCalculator.DeCasteljau1(points.ToArray(), points.Count - 1, 1);
            Assert.NotNull(pt);
        }


        [Fact]
        public void NurbsCurvePoint_Works()
        {
            var p0 = new Point3d(0, 0, 0);
            var p1 = new Point3d(1, 3, 0);
            var p2 = new Point3d(1.4, 5, 0);
            var p3 = new Point3d(0, 7, 0);
            var pts = new[] {p0, p1, p2, p3};

            var u = NurbsCalculator.CreateUniformKnotVector(pts.Length, 1);
            var u2 = NurbsCalculator.CreateUniformKnotVector(pts.Length, 2);
            var u3 = NurbsCalculator.CreateUniformKnotVector(pts.Length, 3);
            var watch = new Stopwatch();
            watch.Start();
            const int n = 100;
            for (var i = 0; i <= n; i++)
            {
                var pt = NurbsCalculator.CurvePoint(
                    3,
                    1,
                    u,
                    pts,
                    ( double ) i / n);
                var pt2 = NurbsCalculator.CurvePoint(
                    3,
                    2,
                    u2,
                    pts,
                    ( double ) i / n);
                var pt3 = NurbsCalculator.CurvePoint(
                    3,
                    3,
                    u3,
                    pts,
                    ( double ) i / n);
                Assert.NotNull(pt);
                Assert.NotNull(pt2);
                Assert.NotNull(pt3);
            }

            watch.Stop();
            this.testOutputHelper.WriteLine(watch.Elapsed.ToString());
        }


        [Fact]
        public void TestKnotVector()
        {
            var u = NurbsCalculator.CreateUniformKnotVector(3, 1);
            var u2 = NurbsCalculator.CreateUniformKnotVector(4, 2);
            var u3 = NurbsCalculator.CreateUniformKnotVector(5, 3);
            Assert.NotNull(u);
            Assert.NotNull(u2);
            Assert.NotNull(u3);
        }
    }
}