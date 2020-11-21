using System;
using Paramdigma.Core.Collections;
using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests.Geometry
{
    public class SphereTests
    {
        [Fact]
        public void CanCompute_FrameAtParameter() { }


        [Fact]
        public void CanCompute_NormalAtParameter()
        {
            var sphere = new Sphere(Plane.WorldXY, 1);
            var actual = sphere.NormalAt(0, 0);
            var expected = Vector3d.UnitZ;
            Assert.Equal(expected, actual);
        }


        [Fact]
        public void ComputeClosestParam_ThenPointAtParam_GivesSamePoint()
        {
            var radius = 4.55;
            var sphere = new Sphere(Plane.WorldXY, radius);
            var pt = new Point3d(radius, 0, 0);
            var param = sphere.ClosestParam(pt);
            Assert.Equal(new Point2d(0, 0.5), param);
            Assert.Equal(pt, sphere.PointAt(param));
        }


        [Fact]
        public void ComputeClosestPoint_GivesAccurateResult()
        {
            var sphere = new Sphere();
            var pt = new Point3d(4, 0, 0);
            Assert.Equal(new Point3d(1, 0, 0), sphere.ClosestPointTo(pt));
        }


        [Fact]
        public void ComputeDistance_GivesAccurateResult()
        {
            var sphere = new Sphere();
            var pt = new Point3d(4, 0, 0);
            Assert.Equal(3.0, sphere.DistanceTo(pt));
        }


        [Fact]
        public void Create_SphereWithEmptyConstructor_ReturnsXYPlaneUnitSphere()
        {
            var sphere = new Sphere();
            Assert.Equal(1, sphere.Radius);
            Assert.Equal(Plane.WorldXY, sphere.Plane);
            Assert.Equal(Interval.Unit, sphere.DomainU);
            Assert.Equal(Interval.Unit, sphere.DomainV);
        }


        [Fact]
        public void Create_SphereWithPlaneAndRadius_ReturnsValidSphere()
        {
            var sphere = new Sphere(Plane.WorldXY, 1);
            Assert.Equal(1, sphere.Radius);
            Assert.Equal(Plane.WorldXY, sphere.Plane);
            Assert.Equal(Interval.Unit, sphere.DomainU);
            Assert.Equal(Interval.Unit, sphere.DomainV);
        }


        [Fact]
        public void Create_SphereWithZeroRadius_ThrowsException() =>
            Assert.Throws<ArithmeticException>(() => new Sphere(Plane.WorldXY, 0));
    }
}