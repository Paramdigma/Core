using System.Collections;
using System.Collections.Generic;
using Paramdigma.Core.Geometry;

namespace Paramdigma.Core.Tests.Geometry
{
    public class Point3dEqualDataset : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] {new Point3d(1, 1, 1), new Point3d(1, 1, 1)};
            yield return new object[] {new Point3d(2, 2, -1), new Point3d(2, 2, -1)};
        }


        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}