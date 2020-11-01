using System.Collections.Generic;
using System.Linq;
using Paramdigma.Core.Geometry;

namespace Paramdigma.Core.HalfEdgeMesh
{
    /// <summary>
    ///     Represents a vertex of a mesh.
    /// </summary>
    public class MeshVertex : Point3d
    {
        private readonly Dictionary<string, double> userValues;

        // Constructor


        /// <summary>
        ///     Initializes a new instance of the <see cref="MeshVertex" /> class.
        /// </summary>
        public MeshVertex() => this.userValues = new Dictionary<string, double>();


        /// <summary>
        ///     Initializes a new instance of the <see cref="MeshVertex" /> class from a 3D point.
        /// </summary>
        /// <param name="pt">Point to copy coordinates from.</param>
        public MeshVertex(Point3d pt)
            : base(pt) =>
            this.userValues = new Dictionary<string, double>();


        /// <summary>
        ///     Initializes a new instance of the <see cref="MeshVertex" /> class from it's cartesian
        ///     coordiantes.
        /// </summary>
        /// <param name="x">X Coordiante.</param>
        /// <param name="y">Y Coordinate.</param>
        /// <param name="z">Z Coordinate.</param>
        public MeshVertex(double x, double y, double z)
            : base(x, y, z) =>
            this.userValues = new Dictionary<string, double>();


        /// <summary>
        ///     Gets or sets the half-edge this vertex is attached to.
        /// </summary>
        public MeshHalfEdge HalfEdge { get; set; }

        /// <summary>
        ///     Gets or sets the index of the vertex.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///     Gets dictionary of user values.
        /// </summary>
        /// <value></value>
        public Dictionary<string, double> UserValues =>
            // Set private property to auto initialize if null.
            this.userValues ?? new Dictionary<string, double>();

        // Calculate the valence of a vertex


        /// <summary>
        ///     Computes the valence of the vertex.
        /// </summary>
        /// <returns></returns>
        public int Valence() => this.AdjacentHalfEdges().Count;


        /// <summary>
        ///     Check if vertex is isolated, meaning corresponding half-edge is null.
        /// </summary>
        /// <returns></returns>
        public bool IsIsolated() => this.HalfEdge == null;


        /// <summary>
        ///     Check if vertex is on mesh boundary.
        /// </summary>
        /// <returns></returns>
        public bool OnBoundary() => this.AdjacentHalfEdges().Any(halfEdge => halfEdge.OnBoundary);


        /// <summary>
        ///     Returns a list with all adjacent HE_HalfEdge of this vertex.
        /// </summary>
        /// <returns></returns>
        public List<MeshHalfEdge> AdjacentHalfEdges()
        {
            var halfEdge = this.HalfEdge;
            var halfEdges = new List<MeshHalfEdge>();
            do
            {
                halfEdges.Add(halfEdge);
                halfEdge = halfEdge.Twin.Next;
            } while (halfEdge != this.HalfEdge);

            return halfEdges;
        }


        /// <summary>
        ///     Returns a list with all adjacent HE_Face of a vertex.
        /// </summary>
        /// <returns></returns>
        public List<MeshFace> AdjacentFaces()
        {
            var halfEdge = this.HalfEdge;
            var faces = new List<MeshFace>();
            do
            {
                if (!halfEdge.OnBoundary)
                    faces.Add(halfEdge.Face);
                halfEdge = halfEdge.Twin.Next;
            } while (halfEdge != this.HalfEdge);

            return faces;
        }


        /// <summary>
        ///     Returns a list with all the adjacent HE_Vertex of this vertex.
        /// </summary>
        /// <returns></returns>
        public List<MeshVertex> AdjacentVertices()
        {
            var vertices = new List<MeshVertex>();
            var halfEdge = this.HalfEdge;
            do
            {
                vertices.Add(halfEdge.Twin.Vertex);
                halfEdge = halfEdge.Twin.Next;
            } while (halfEdge != this.HalfEdge);

            return vertices;
        }


        /// <summary>
        ///     Returns a list with all the adjacent HE_Edge of this vertex.
        /// </summary>
        /// <returns></returns>
        public List<MeshEdge> AdjacentEdges()
        {
            var edges = new List<MeshEdge>();
            var halfEdge = this.HalfEdge;
            do
            {
                edges.Add(halfEdge.Edge);
                halfEdge = halfEdge.Twin.Next;
            } while (halfEdge != this.HalfEdge);

            return edges;
        }


        /// <summary>
        ///     Returns a list with all the adjacent HE_Corners of this vertex.
        /// </summary>
        /// <returns></returns>
        public List<MeshCorner> AdjacentCorners()
        {
            var corners = new List<MeshCorner>();
            var halfEdge = this.HalfEdge;
            do
            {
                if (!halfEdge.OnBoundary)
                    corners.Add(halfEdge.Next.Corner);
                halfEdge = halfEdge.Twin.Next;
            } while (halfEdge != this.HalfEdge);

            return corners;
        }


        /// <summary>
        ///     Returns the string representation of this vertex.
        /// </summary>
        /// <returns></returns>
        public override string ToString() => "V " + this.Index;
    }
}