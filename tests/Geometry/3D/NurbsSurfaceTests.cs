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


        private static Matrix<Point4d> FlatGrid(int size)
        {
            var m = new Matrix<Point4d>(size);
            for (var i = 0; i < size; i++)
            {
                for (var j = 0; j < size; j++)
                    m[i, j] = new Point4d(i, j, 0, 1);
            }

            return m;
        }


        private static NurbsSurface Surface => new NurbsSurface(FlatGrid(4), 3, 3);


        private RG.NurbsSurface RhSurface()
        {
            var surf = RG.NurbsSurface.Create(3, false, 3, 3, 4, 4);
            for (var i = 0; i < Surface.ControlPoints.N; i++)
            {
                for (var j = 0; j < Surface.ControlPoints.M; j++)
                {
                    var pt = Surface.ControlPoints[i, j];
                    surf.Points.SetPoint(i, j, pt.X, pt.Y, pt.Z, pt.Weight);
                }
            }

            surf.KnotsU.CreateUniformKnots(1);
            surf.KnotsV.CreateUniformKnots(1);
            
            surf.SetDomain(0, new RG.Interval(0, 1));
            surf.SetDomain(1, new RG.Interval(0, 1));
            
            return surf;
        }


        [Theory]
        [ClassData(typeof(NurbsSurfaceUnitParamData))]
        public void CanGet_PointAt(double u, double v)
        {
            // TODO: Fix inaccurate test.
            var pt = Surface.PointAt(u, v);
            var rhSurf = this.RhSurface();
            var ptRh = rhSurf.PointAt(u, v);
            var distance = pt.DistanceTo(ptRh.ToCore());
            this.testOutputHelper.WriteLine(distance.ToString());
            Assert.NotNull(pt);
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