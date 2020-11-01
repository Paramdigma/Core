using System.Collections;
using System.Collections.Generic;

namespace Paramdigma.Core.Tests.Geometry._3D
{
    public class NurbsCurveUnitParamData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {0};
            yield return new object[] {0.1};
            yield return new object[] {0.2};
            yield return new object[] {0.3};
            yield return new object[] {0.4};
            yield return new object[] {0.5};
            yield return new object[] {0.6};
            yield return new object[] {0.7};
            yield return new object[] {0.8};
            yield return new object[] {0.9};
            yield return new object[] {1.0};
        }


        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}