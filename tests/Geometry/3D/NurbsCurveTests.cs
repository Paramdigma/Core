using System.Collections.Generic;
using System.Linq;
using Paramdigma.Core.Geometry;
using Paramdigma.Core.Tests.Conversions;
using Xunit;
using RG = Rhino.Geometry;

namespace Paramdigma.Core.Tests.Geometry
{
    public class NurbsCurveTests
    {
        private readonly List<Point4d> controlPoints = new List<Point4d>
        {
            new Point4d(0, 0, 0,4),
            new Point4d(1, 3, 0,3.5),
            new Point4d(1.4, 5, 0,2),
            new Point4d(0, 7, 0,1),
        };

        private NurbsCurve Curve => new NurbsCurve(this.controlPoints, 3);

        private RG.NurbsCurve RhCurve
        {
            get
            {
                var rhcrv = RG.Curve.CreateControlPointCurve(
                    this.controlPoints.Select(pt => pt.Position.ToRhino()),
                    3).ToNurbsCurve();
                for (var i = 0; i < this.controlPoints.Count; i++)
                {
                    rhcrv.Points.SetWeight(i, this.controlPoints[i].Weight);
                }
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