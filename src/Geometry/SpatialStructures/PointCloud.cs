using System.Collections.Generic;

namespace Paramdigma.Core.SpatialSearch
{
    /// <summary>
    /// Represents a collection of points with a color assigned to them.
    /// TODO: This is only a basic data structure for now.
    /// </summary>
    public class PointCloud
    {
        /// <summary>
        /// Gets collection of points in the point cloud.
        /// </summary>
        public List<PointCloudMember> Points { get; }
    }
}