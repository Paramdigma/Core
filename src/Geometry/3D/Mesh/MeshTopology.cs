using System.Collections.Generic;

namespace Paramdigma.Core.HalfEdgeMesh
{
    /// <summary>
    ///     Topology exporer for meshes. Contains all methods to explore mesh connections between members.
    /// </summary>
    public class MeshTopology
    {
        // Per Vertex adjacency index lists
        // Returns 2 dimensional array: 1 array per vertex index containing an array with the corresponding adjacent member index
        private readonly Mesh mesh;


        /// <summary>
        ///     Initializes a new instance of the <see cref="MeshTopology" /> class.
        /// </summary>
        /// <param name="mesh">Mesh to construct topology connections from.</param>
        public MeshTopology(Mesh mesh)
        {
            this.mesh = mesh;

            this.VertexVertex = new Dictionary<int, List<int>>();
            this.VertexFaces = new Dictionary<int, List<int>>();
            this.VertexEdges = new Dictionary<int, List<int>>();
            this.FaceVertex = new Dictionary<int, List<int>>();
            this.FaceFace = new Dictionary<int, List<int>>();
            this.FaceEdge = new Dictionary<int, List<int>>();
            this.EdgeVertex = new Dictionary<int, List<int>>();
            this.EdgeFace = new Dictionary<int, List<int>>();
            this.EdgeEdge = new Dictionary<int, List<int>>();
            
            this.ComputeEdgeAdjacency();
            this.ComputeFaceAdjacency();
            this.ComputeVertexAdjacency();
        }


        /// <summary>
        ///     Gets vertex-Vertex topological connections.
        /// </summary>
        public Dictionary<int, List<int>> VertexVertex { get; }

        /// <summary>
        ///     Gets vertex-Face topological connections.
        /// </summary>
        public Dictionary<int, List<int>> VertexFaces { get; }

        /// <summary>
        ///     Gets vertex-Edge topological connections.
        /// </summary>
        public Dictionary<int, List<int>> VertexEdges { get; }

        /// <summary>
        ///     Gets edge-Edge topological connections.
        /// </summary>
        public Dictionary<int, List<int>> EdgeEdge { get; }

        /// <summary>
        ///     Gets edge-Vertex topological connections.
        /// </summary>
        public Dictionary<int, List<int>> EdgeVertex { get; }

        /// <summary>
        ///     Gets edge-Face topological connections.
        /// </summary>
        public Dictionary<int, List<int>> EdgeFace { get; }

        /// <summary>
        ///     Gets face-Vertex topological connections.
        /// </summary>
        public Dictionary<int, List<int>> FaceVertex { get; }

        /// <summary>
        ///     Gets face-Edge topological connections.
        /// </summary>
        public Dictionary<int, List<int>> FaceEdge { get; }

        /// <summary>
        ///     Gets face-Face topological connections.
        /// </summary>
        public Dictionary<int, List<int>> FaceFace { get; }


        /// <summary>
        ///     Computes vertex adjacency for the whole mesh and stores it in the appropriate dictionaries.
        /// </summary>
        public void ComputeVertexAdjacency()
        {
            foreach (var vertex in this.mesh.Vertices)
            {
                foreach (var adjacent in vertex.AdjacentVertices())
                {
                    if (!this.VertexVertex.ContainsKey(vertex.Index))
                        this.VertexVertex.Add(vertex.Index, new List<int> {adjacent.Index});
                    else
                        this.VertexVertex[vertex.Index].Add(adjacent.Index);
                }

                foreach (var adjacent in vertex.AdjacentFaces())
                {
                    if (!this.VertexFaces.ContainsKey(vertex.Index))
                        this.VertexFaces.Add(vertex.Index, new List<int> {adjacent.Index});
                    else
                        this.VertexFaces[vertex.Index].Add(adjacent.Index);
                }

                foreach (var adjacent in vertex.AdjacentEdges())
                {
                    if (!this.VertexEdges.ContainsKey(vertex.Index))
                        this.VertexEdges.Add(vertex.Index, new List<int> {adjacent.Index});
                    else
                        this.VertexEdges[vertex.Index].Add(adjacent.Index);
                }
            }
        }


        /// <summary>
        ///     Computes face adjacency for the whole mesh and stores it in the appropriate dictionaries.
        /// </summary>
        public void ComputeFaceAdjacency()
        {
            foreach (var face in this.mesh.Faces)
            {
                foreach (var adjacent in face.AdjacentVertices())
                {
                    if (!this.FaceVertex.ContainsKey(face.Index))
                        this.FaceVertex.Add(face.Index, new List<int> {adjacent.Index});
                    else
                        this.FaceVertex[face.Index].Add(adjacent.Index);
                }

                foreach (var adjacent in face.AdjacentFaces())
                {
                    if (!this.FaceFace.ContainsKey(face.Index))
                        this.FaceFace.Add(face.Index, new List<int> {adjacent.Index});
                    else
                        this.FaceFace[face.Index].Add(adjacent.Index);
                }

                foreach (var adjacent in face.AdjacentEdges())
                {
                    if (!this.FaceEdge.ContainsKey(face.Index))
                        this.FaceEdge.Add(face.Index, new List<int> {adjacent.Index});
                    else
                        this.FaceEdge[face.Index].Add(adjacent.Index);
                }
            }
        }


        /// <summary>
        ///     Computes edge adjacency for the whole mesh and stores it in the appropriate dictionaries.
        /// </summary>
        public void ComputeEdgeAdjacency()
        {
            foreach (var edge in this.mesh.Edges)
            {
                foreach (var adjacent in edge.AdjacentVertices())
                {
                    if (!this.EdgeVertex.ContainsKey(edge.Index))
                        this.EdgeVertex.Add(edge.Index, new List<int> {adjacent.Index});
                    else
                        this.EdgeVertex[edge.Index].Add(adjacent.Index);
                }

                foreach (var adjacent in edge.AdjacentFaces())
                {
                    if (!this.EdgeFace.ContainsKey(edge.Index))
                        this.EdgeFace.Add(edge.Index, new List<int> {adjacent.Index});
                    else
                        this.EdgeFace[edge.Index].Add(adjacent.Index);
                }

                foreach (var adjacent in edge.AdjacentEdges())
                {
                    if (!this.EdgeEdge.ContainsKey(edge.Index))
                        this.EdgeEdge.Add(edge.Index, new List<int> {adjacent.Index});
                    else
                        this.EdgeEdge[edge.Index].Add(adjacent.Index);
                }
            }
        }


        /// <summary>
        ///     Gets the string representation of a given topology dictionary.
        /// </summary>
        /// <param name="dict">Dictionary to convert.</param>
        /// <returns></returns>
        public string TopologyDictToString(Dictionary<int, List<int>> dict)
        {
            var finalString = string.Empty;

            foreach (var pair in dict)
            {
                var tmpString = "Key: " + pair.Key + " --> ";
                foreach (var i in pair.Value)
                    tmpString += i + " ";

                tmpString += "\n";
                finalString += tmpString;
            }

            return finalString;
        }
    }
}