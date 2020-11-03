using System.Collections.Generic;
using System.Linq;
using Paramdigma.Core.Geometry;
using Paramdigma.Core.Tests.Conversions;
using Xunit;
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
        };

        private NurbsCurve Curve => new NurbsCurve(this.controlPoints, 3);

        private RG.NurbsCurve RhCurve
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
        private void CanGet_PointAt(double t)
        {
            var point = this.Curve.PointAt(t);
            var rhPoint = this.RhCurve.PointAt(t);
            Assert.True(point.DistanceTo(rhPoint.ToCore()) < Settings.Tolerance);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(0.1)]
        [InlineData(0.2)]
        [InlineData(1.0)]
        public void CanGet_TangentAt(double t)
        {
            var vector = this.Curve.TangentAt(t);
            var rhVector = this.RhCurve.TangentAt(t);
            Assert.True((vector - rhVector.ToCore()).Length < Settings.Tolerance);
        }


        [Theory]
        [ClassData(typeof(NurbsCurveUnitParamData))]
        public void CanGet_NormalAt(double t)
        {
            var vector = this.Curve.NormalAt(t);
            var rhVector = this.RhCurve.DerivativeAt(t, 2)[2];
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
            var vector = this.Curve.BinormalAt(t);
            var rhVector = this.RhCurve.DerivativeAt(t, 3)[3];
            var length = (vector - rhVector.ToCore().Unit()).Length;
            Assert.True(length < Settings.Tolerance);
        }
    }
}