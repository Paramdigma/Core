using System.Drawing;
using Paramdigma.Core.Geometry;

namespace Paramdigma.Core.SpatialSearch
{
    /// <summary>
    ///     Class representing a point contained in a point cloud.
    /// </summary>
    public class PointCloudMember : BasePoint
    {
        /// <summary>
        ///     Gets or sets the color at this point.
        /// </summary>
        /// <value>The current color if set, defaults to white.</value>
        public Color Color
        {
            get;
            set;
        }
    }
}