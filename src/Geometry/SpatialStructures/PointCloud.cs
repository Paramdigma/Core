using System.Collections;
using System.Collections.Generic;

namespace Paramdigma.Core.SpatialSearch
{
    /// <summary>
    /// Represents a collection of points with a color assigned to them.
    /// TODO: This is only a basic data structure for now.
    /// </summary>
    public class PointCloud : IList<PointCloudMember>
    {
        private List<PointCloudMember> points
        {
            get;
        }

        public IEnumerator<PointCloudMember> GetEnumerator() => this.points.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this.points).GetEnumerator();

        public void Add(PointCloudMember item) => this.points.Add(item);

        /// <inheritdoc />
        public void Clear() => this.points.Clear();

        /// <inheritdoc />
        public bool Contains(PointCloudMember item) => this.points.Contains(item);

        /// <inheritdoc />
        public void CopyTo(PointCloudMember[] array, int arrayIndex) => this.points.CopyTo(array, arrayIndex);

        /// <inheritdoc />
        public bool Remove(PointCloudMember item) => this.points.Remove(item);

        /// <inheritdoc />
        public int Count => this.points.Count;

        /// <inheritdoc />
        public bool IsReadOnly => ((ICollection<PointCloudMember>)this.points).IsReadOnly;

        /// <inheritdoc />
        public int IndexOf(PointCloudMember item) => this.points.IndexOf(item);

        /// <inheritdoc />
        public void Insert(int index, PointCloudMember item) => this.points.Insert(index, item);

        /// <inheritdoc />
        public void RemoveAt(int index) => this.points.RemoveAt(index);

        /// <inheritdoc />
        public PointCloudMember this[int index]
        {
            get => this.points[index];
            set => this.points[index] = value;
        }
    }
}