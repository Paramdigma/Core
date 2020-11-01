namespace Paramdigma.Core.HalfEdgeMesh
{
    /// <summary>
    ///     Represents a corner of a given mesh face.
    /// </summary>
    public class MeshCorner
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MeshCorner" /> class.
        /// </summary>
        public MeshCorner() => this.Index = -1;


        /// <summary>
        ///     Gets or sets the corner's first half-edge.
        /// </summary>
        public MeshHalfEdge HalfEdge { get; set; }

        /// <summary>
        ///     Gets or sets the index of the mesh corner.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///     Gets the mesh corner vertex.
        /// </summary>
        public MeshVertex Vertex => this.HalfEdge.Prev.Vertex;

        /// <summary>
        ///     Gets the face the mesh corner belongs to.
        /// </summary>
        public MeshFace Face => this.HalfEdge.Face;

        /// <summary>
        ///     Gets the next corner.
        /// </summary>
        public MeshCorner Next => this.HalfEdge.Next.Corner;

        /// <summary>
        ///     Gets the previous corner.
        /// </summary>
        public MeshCorner Prev => this.HalfEdge.Prev.Corner;
    }
}