using System.Collections.Generic;
using System.Linq;
using Paramdigma.Core.Geometry;
using Paramdigma.Core.Tests.Conversions;
using Xunit;
using Xunit.Abstractions;
using RG = Rhino.Geometry;

namespace Paramdigma.Core.Tests.Geometry._3D
{
    public class NurbsCurveTests
    {
        private readonly List<Point3d> controlPoints = new List<Point3d>
        {
            new Point3d(0, 0, 0),
            new Point3d(1, 3, 0),
            new Point3d(1.4, 5, 0),
            new Point3d(0, 7, 0),
            new Point3d(0, 9, 0)
        };

        private readonly ITestOutputHelper testOutputHelper;


        public NurbsCurveTests(ITestOutputHelper testOutputHelper) =>
            this.testOutputHelper = testOutputHelper;


        private NurbsCurve curve => new NurbsCurve(this.controlPoints, 3);

        private RG.NurbsCurve rhCurve
        {
            get
            {
                var rhcrv = RG.Curve.CreateControlPointCurve(
                    this.controlPoints.Select(pt => pt.ToRhino()),
                    3);
                rhcrv.Domain = new RG.Interval(0, 1);
                return rhcrv.ToNurbsCurve();
            }
        }


        [Theory]
        [InlineData(0)]
        [InlineData(0.1)]
        [InlineData(0.2)]
        [InlineData(1.0)]
        public void CanGet_PointAt(double t)
        {
            var point = this.curve.PointAt(t);
            var rhPoint = this.rhCurve.PointAt(t);
            Assert.True(point.DistanceTo(rhPoint.ToCore()) < Settings.Tolerance);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(0.1)]
        [InlineData(0.2)]
        [InlineData(1.0)]
        public void CanGet_TangentAt(double t)
        {
            var vector = this.curve.TangentAt(t);
            var rhVector = this.rhCurve.TangentAt(t);
            Assert.True((vector - rhVector.ToCore()).Length < Settings.Tolerance);
        }


        [Theory]
        [ClassData(typeof(NurbsCurveUnitParamData))]
        public void CanGet_NormalAt(double t)
        {
            var vector = this.curve.NormalAt(t);
            var rhVector = this.rhCurve.DerivativeAt(t, 2)[2];
            var length = (vector - rhVector.ToCore().Unit()).Length;
            Assert.True(length < Settings.Tolerance);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(0.1)]
        [InlineData(0.2)]
        [InlineData(1.0)]
        public void CanGet_BiNormalAt(double t)
        {
            var vector = this.curve.BinormalAt(t);
            var rhVector = this.rhCurve.DerivativeAt(t, 3)[3];
            var length = (vector - rhVector.ToCore().Unit()).Length;
            Assert.True(length < Settings.Tolerance);
        }
    }
}