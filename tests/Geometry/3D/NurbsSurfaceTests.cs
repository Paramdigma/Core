using System.Collections;
using System.Collections.Generic;
using Paramdigma.Core.Collections;
using Paramdigma.Core.Geometry;
using Paramdigma.Core.Tests.Conversions;
using Xunit;
using Xunit.Abstractions;
using RG = Rhino.Geometry;

namespace Paramdigma.Core.Tests.Geometry
{
    public class NurbsSurfaceTests
    {
        private readonly ITestOutputHelper testOutputHelper;


        public NurbsSurfaceTests(ITestOutputHelper testOutputHelper) =>
            this.testOutputHelper = testOutputHelper;


        [Theory]
        [ClassData(typeof(NurbsSurfaceUnitParamData))]
        public void CanGet_PointAt(double u, double v)
        {
            var surf = NurbsSurface.CreateFlatSurface(Interval.Unit, Interval.Unit, 4, 4);
            var pt = surf.PointAt(u, v);
            var rhSurf = surf.ToRhino();
            var ptRh = rhSurf.PointAt(u, v);
            var distance = pt.DistanceTo(ptRh.ToCore());
            Assert.True(distance < Settings.Tolerance);
        }


        [Theory]
        [ClassData(typeof(NurbsSurfaceUnitParamData))]
        public void CanGet_TangentAt(double u, double v)
        {
            var surf = NurbsSurface.CreateFlatSurface(Interval.Unit, Interval.Unit, 4, 4);
            var uT = surf.DerivativesAt(u, v, 1)[0, 1].Unit();
            var vT = surf.DerivativesAt(u, v, 1)[1, 0].Unit();
            var rhSurf = surf.ToRhino();

            var uCrv = rhSurf.IsoCurve(1, v);
            uCrv.Domain = new RG.Interval(0, 1);
            var vCrv = rhSurf.IsoCurve(0, u);
            vCrv.Domain = new RG.Interval(0, 1);

            var uVector = uCrv.TangentAt(u);
            var vVector = vCrv.TangentAt(v);

            Assert.True((uVector.ToCore() - uT).Length <= Settings.Tolerance);
            Assert.True((vVector.ToCore() - vT).Length <= Settings.Tolerance);
        }


        [Theory]
        [ClassData(typeof(NurbsSurfaceUnitParamData))]
        public void CanGet_NormalAt(double u, double v)
        {
            var surf = NurbsSurface.CreateFlatSurface(Interval.Unit, Interval.Unit, 4, 4);
            var uT = surf.DerivativesAt(u, v, 1)[0, 1];
            var vT = surf.DerivativesAt(u, v, 1)[1, 0];
            var cross = vT.Cross(uT).Unit();
            var rhSurf = surf.ToRhino();
            var rhVector = rhSurf.NormalAt(u, v);
            rhVector.Unitize();
            Assert.True((rhVector.ToCore() - cross).Length <= Settings.Tolerance);
        }
    }

    public class NurbsSurfaceUnitParamData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {0.0, 0.99};
            yield return new object[] {0.1, .9};
            yield return new object[] {0.2, .8};
            yield return new object[] {0.3, .7};
            yield return new object[] {0.4, .6};
            yield return new object[] {0.5, .5};
            yield return new object[] {0.6, .4};
            yield return new object[] {0.7, .3};
            yield return new object[] {0.8, .2};
            yield return new object[] {0.9, .1};
            yield return new object[] {0.99, .0};
            yield return new object[] {1, .0};
        }


        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}