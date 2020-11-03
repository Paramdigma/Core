using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Paramdigma.Core.Collections;
using Paramdigma.Core.Geometry;
using Paramdigma.Core.Tests.Conversions;
using Xunit;
using Xunit.Abstractions;
using RG = Rhino.Geometry;

namespace Paramdigma.Core.Tests.Geometry._3D
{
    public class NurbsSurfaceTests
    {
        private readonly ITestOutputHelper testOutputHelper;


        public NurbsSurfaceTests(ITestOutputHelper testOutputHelper)
        {
            this.testOutputHelper = testOutputHelper;
        }

        
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
        public void CanGet_TangentAt(double u, double v) { }


        [Theory]
        [ClassData(typeof(NurbsSurfaceUnitParamData))]
        public void CanGet_NormalAt(double u, double v) { }


        [Theory]
        [ClassData(typeof(NurbsSurfaceUnitParamData))]
        public void CanGet_BiNormalAt(double u, double v) { }
    }

    public class NurbsSurfaceUnitParamData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {0.0, 1.0};
            yield return new object[] {0.1, .9};
            yield return new object[] {0.2, .8};
            yield return new object[] {0.3, .7};
            yield return new object[] {0.4, .6};
            yield return new object[] {0.5, .5};
            yield return new object[] {0.6, .4};
            yield return new object[] {0.7, .3};
            yield return new object[] {0.8, .2};
            yield return new object[] {0.9, .1};
            yield return new object[] {1.0, .0};
        }


        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}