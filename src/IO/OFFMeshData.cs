using System.Collections.Generic;
using Paramdigma.Core.Geometry;

#pragma warning disable 1591

namespace Paramdigma.Core.IO
{
    /// <summary>
    ///     Class containing the resulting mesh data extracted from an .OFF file.
    /// </summary>
    public class OFFMeshData
    {
        /// <summary>
        ///     Gets or sets the mesh vertices.
        /// </summary>
        public List<Point3d> Vertices { get; set; }

        /// <summary>
        ///     Gets or sets the mesh face indices.
        /// </summary>
        public List<List<int>> Faces { get; set; }
    }
}