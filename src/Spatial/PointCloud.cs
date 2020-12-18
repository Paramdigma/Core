using System.Collections;
using System.Collections.Generic;

namespace Paramdigma.Core.Spatial
{
    /// <summary>
    ///     Represents a collection of points with a color assigned to them.
    ///     TODO: This is only a basic data structure for now.
    /// </summary>
    public class PointCloud : IList<PointCloudMember>
    {
        /// <summary>
        ///     Constructs a new point-cloud from a list of point cloud members..
        /// </summary>
        /// <param name="points">List of point-cloud members.</param>
        public PointCloud(List<PointCloudMember> points) => this.Points = points;


        private List<PointCloudMember> Points { get; }


        /// <inheritdoc />
        public IEnumerator<PointCloudMember> GetEnumerator() => this.Points.GetEnumerator();


        IEnumerator IEnumerable.GetEnumerator() => (( IEnumerable ) this.Points).GetEnumerator();


        /// <inheritdoc />
        public void Add(PointCloudMember item) => this.Points.Add(item);


        /// <inheritdoc />
        public void Clear() => this.Points.Clear();


        /// <inheritdoc />
        public bool Contains(PointCloudMember item) => this.Points.Contains(item);


        /// <inheritdoc />
        public void CopyTo(PointCloudMember[] array, int arrayIndex) =>
            this.Points.CopyTo(array, arrayIndex);


        /// <inheritdoc />
        public bool Remove(PointCloudMember item) => this.Points.Remove(item);


        /// <inheritdoc />
        public int Count => this.Points.Count;

        /// <inheritdoc />
        public bool IsReadOnly => (( ICollection<PointCloudMember> ) this.Points).IsReadOnly;


        /// <inheritdoc />
        public int IndexOf(PointCloudMember item) => this.Points.IndexOf(item);


        /// <inheritdoc />
        public void Insert(int index, PointCloudMember item) => this.Points.Insert(index, item);


        /// <inheritdoc />
        public void RemoveAt(int index) => this.Points.RemoveAt(index);


        /// <inheritdoc />
        public PointCloudMember this[int index]
        {
            get => this.Points[index];
            set => this.Points[index] = value;
        }
    }
}