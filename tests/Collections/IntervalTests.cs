using System;
using Paramdigma.Core.Collections;
using Xunit;

namespace Paramdigma.Core.Tests
{
    public class IntervalTests
    {
        [Fact]
        public void Can_CheckAndModifyDirection()
        {
            var i = new Interval(1, 4);
            var dir = i.HasInvertedDirection;
            i.FlipDirection();
            var dir2 = i.HasInvertedDirection;
            Assert.True(dir != dir2);
        }


        [Fact]
        public void Can_CheckContainment()
        {
            var i = new Interval(0.455, 4.134);
            const double n1 = 0.0;
            const double n2 = 5.0;
            const double n3 = 2.33;
            Assert.False(i.Contains(n1));
            Assert.False(i.Contains(n2));
            Assert.True(i.Contains(n3));
        }


        [Fact]
        public void Can_CropNumbers()
        {
            var i = new Interval(0.455, 4.134);
            const double n1 = 0.0;
            const double n2 = 5.0;
            const double n3 = 2.33;
            var n1c = i.Crop(n1);
            var n2c = i.Crop(n2);
            var n3c = i.Crop(n3);
            Assert.True(Math.Abs(n1c - i.Start) < Settings.Tolerance);
            Assert.True(Math.Abs(n2c - i.End) < Settings.Tolerance);
            Assert.True(Math.Abs(n3c - n3) < Settings.Tolerance);
        }


        [Fact]
        public void Can_RemapNumbers()
        {
            var i = new Interval(1, 3);
            const double n = 2.0;
            var nMap = i.RemapToUnit(n);
            Assert.True(Math.Abs(nMap - 0.5) < Settings.Tolerance);
            var nRemap = i.RemapFromUnit(nMap);
            Assert.True(Math.Abs(n - nRemap) < Settings.Tolerance);
        }


        [Fact]
        public void CanCreate_Interval()
        {
            var i0 = new Interval(0, 1);
            var i1 = Interval.Unit;
            var i2 = new Interval(i0);

            Assert.Equal(1, i0.Length);
            Assert.Throws<ArithmeticException>(() => new Interval(double.NaN, 1));
            Assert.Throws<ArithmeticException>(() => new Interval(0, double.NaN));
        }
    }
}