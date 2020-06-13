using System;
using System.Collections.Generic;
using Paramdigma.Core.Geometry;
using Xunit;

namespace Paramdigma.Core.Tests
{
    public class VectorNdTests : VectorEntityTests<VectorNd>
    {
        [Theory]
        [MemberData(nameof(VectorAddData))]
        public override void CanAdd(VectorNd a, VectorNd b, VectorNd expected)
        {
            var c = a + b;
            Assert.Equal(c, expected);
        }

        [Theory]
        [MemberData(nameof(VectorAddData))]
        public override void CanAdd_Itself(VectorNd a, VectorNd b, VectorNd expected)
        {
            Assert.Equal(a + b, expected);
        }

        public static IEnumerable<object[]> VectorAddData => new List<object[]> {new object[] {new VectorNd(0, 0, 0, 9, 3), new VectorNd(4, 5, 6), new VectorNd(4, 5, 6, 9, 3)}, new object[] {new VectorNd(3, 5, 0, 3), new VectorNd(4, 5, 6, 1, 3, 5), new VectorNd(7, 10, 6, 4, 3, 5)}};

        [Theory]
        [MemberData(nameof(VectorDivideData))]
        public override void CanDivide_New(VectorNd a, double scalar, VectorNd expected)
        {
            Assert.Equal(a / scalar, expected);
        }

        public static IEnumerable<object[]> VectorDivideData => new List<object[]> {new object[] {new VectorNd(0, 0, 0), 4, new VectorNd(0, 0, 0)}, new object[] {new VectorNd(3, 5, 1), 2, new VectorNd(1.5, 2.5, 0.5)}};

        [Theory]
        [MemberData(nameof(VectorMultiplyData))]
        public override void CanMultiply_New(VectorNd a, double scalar, VectorNd expected)
        {
            Assert.Equal(a * scalar, expected);
        }

        public static IEnumerable<object[]> VectorMultiplyData => new List<object[]> {new object[] {new VectorNd(0, 0, 0), 5, new VectorNd(0, 0, 0)}, new object[] {new VectorNd(3, 5, 2), 6, new VectorNd(18, 30, 12)}};

        [Theory]
        [MemberData(nameof(VectorSubstractData))]
        public override void CanSubstract_New(VectorNd a, VectorNd b, VectorNd expected)
        {
            Assert.Equal(a - b, expected);
        }

        [Theory]
        [MemberData(nameof(VectorSubstractData))]
        public override void CanSubstract_ToItself(VectorNd a, VectorNd b, VectorNd expected)
        {
            Assert.Equal(a - b, expected);
        }

        public static IEnumerable<object[]> VectorSubstractData => new List<object[]> {new object[] {new VectorNd(0, 0, 0), new VectorNd(4, 5, 6), new VectorNd(-4, -5, -6)}, new object[] {new VectorNd(3, 5, 0), new VectorNd(4, 5, 6), new VectorNd(-1, 0, -6)}};

        [Theory]
        [MemberData(nameof(IsEqualData))]
        public override void IsEqual_HasEqualHashcodes(VectorNd a, VectorNd b)
        {
            Assert.Equal(a.GetHashCode(), b.GetHashCode());
        }

        [Theory]
        [MemberData(nameof(IsEqualData))]
        public override void IsEqual_WithinTolerance(VectorNd a, VectorNd b)
        {
            Assert.Equal(a, b);
        }

        public override void CanMultiply_Itself(VectorNd a, double scalar, VectorNd expected)
        {
            throw new NotImplementedException();
        }

        public override void CanDivide_Itself(VectorNd a, double scalar, VectorNd expected)
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<object[]> IsEqualData => new List<object[]> {new object[] {new VectorNd(3, 3, 3), new VectorNd(3, 3, 3 + 1E-12)}, new object[] {new VectorNd(4, 5, 6), new VectorNd(4 + 1E-9, 5 + 1E-8, 6)}};
    }
}