using System.Collections.Generic;
using Paramdigma.Core.Geometry;

namespace Paramdigma.Core.HalfEdgeMesh
{
    /// <summary>
    /// Represents a Half-Edge Mesh data structure.
    /// </summary>
    public class Mesh
    {
        /// <summary>
        /// Gets or sets the vertices of the mesh.
        /// </summary>
        public List<MeshVertex> Vertices
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the edges of the mesh.
        /// </summary>
        public List<MeshEdge> Edges
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the faces of the mesh.
        /// </summary>
        public List<MeshFace> Faces
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the corners of the mesh.
        /// </summary>
        public List<MeshCorner> Corners
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the half-edges of the mesh.
        /// </summary>
        public List<MeshHalfEdge> HalfEdges
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the boundaries of the mesh.
        /// </summary>
        public List<MeshFace> Boundaries
        {
            get; set;
        }

        /// <summary>
        /// Gets the euler characteristic of the mesh.
        /// </summary>
        public int EulerCharacteristic => this.Vertices.Count - Edges.Count + Faces.Count;

        /// <summary>
        /// Initializes a new instance of the <see cref="Mesh"/> class.
        /// </summary>
        public Mesh()
        {
            Vertices = new List<MeshVertex>();
            Edges = new List<MeshEdge>();
            Faces = new List<MeshFace>();
            Corners = new List<MeshCorner>();
            HalfEdges = new List<MeshHalfEdge>();
            Boundaries = new List<MeshFace>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mesh"/> class from verticees and faces.
        /// </summary>
        /// <param name="vertices">list of mesh vertices.</param>
        /// <param name="faceIndexes">Nested list with face vertices index.</param>
        public Mesh(List<Point3d> vertices, List<List<int>> faceIndexes)
            : this()
        {
            // There are 3 steps for this process
            // - Iterate through vertices, create vertex objects
            this.CreateVertices(vertices);

            // - Iterate through faces, creating face, edge, and halfedge objects (and connecting where possible)
            this.CreateFaces(faceIndexes);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mesh"/> class from verticees and faces.
        /// </summary>
        /// <param name="vertices">list of mesh vertices.</param>
        /// <param name="faceIndexes">Nested list with face vertices index.</param>
        public Mesh(List<MeshVertex> vertices, List<List<int>> faceIndexes)
            : this()
        {
            this.Vertices = vertices;

            // - Iterate through faces, creating face, edge, and halfedge objects (and connecting where possible)
            this.CreateFaces(faceIndexes);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mesh"/> class from an existing one.
        /// </summary>
        /// <param name="halfEdgeMesh">Existing Half-Edge Mesh.</param>
        public Mesh(Mesh halfEdgeMesh)
        {
            this.Vertices = new List<MeshVertex>(halfEdgeMesh.Vertices);
            this.Edges = new List<MeshEdge>(halfEdgeMesh.Edges);
            this.Faces = new List<MeshFace>(halfEdgeMesh.Faces);
            this.Corners = new List<MeshCorner>(halfEdgeMesh.Corners);
            this.HalfEdges = new List<MeshHalfEdge>(halfEdgeMesh.HalfEdges);
            this.Boundaries = new List<MeshFace>(halfEdgeMesh.Boundaries);
        }

        /// <summary>
        /// Check if the mesh has isolated vertices.
        /// </summary>
        /// <returns>True if there are isolated vertices, false if not.</returns>
        public bool HasIsolatedVertices()
        {
            foreach (MeshVertex v in Vertices)
            {
                if (v.IsIsolated())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Check if the mesh contains isolated faces.
        /// </summary>
        /// <returns>True if there are isolated faces, false if not.</returns>
        public bool HasIsolatedFaces()
        {
            foreach (MeshFace f in Faces)
            {
                int boundaryEdges = 0;
                List<MeshHalfEdge> adjacent = f.AdjacentHalfEdges();
                foreach (MeshHalfEdge e in adjacent)
                {
                    if (e.OnBoundary)
                    {
                        boundaryEdges++;
                    }
                }

                if (boundaryEdges == adjacent.Count)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Check if the mesh contains non-manifold edges.
        /// </summary>
        /// <returns>True if there are non-manifold edges, false if not.</returns>
        public bool HasNonManifoldEdges()
        {
            foreach (var edge in this.Edges)
            {
                if (edge.AdjacentFaces().Count > 2)
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Assign an index number to each mesh member.
        /// </summary>
        public void IndexElements()
        {
            int index = -1;
            foreach (var v in this.Vertices)
            {
                index++;
                v.Index = index;
            }

            index = -1;
            foreach (var f in this.Faces)
            {
                index++;
                f.Index = index;
            }

            index = -1;
            foreach (var hE in this.HalfEdges)
            {
                index++;
                hE.Index = index;
            }

            index = -1;
            foreach (var e in this.Edges)
            {
                index++;
                e.Index = index;
            }

            index = -1;
            foreach (var c in this.Corners)
            {
                index++;
                c.Index = index;
            }

            index = -1;
            foreach (var b in this.Boundaries)
            {
                index++;
                b.Index = index;
            }
        }

        /// <summary>
        /// Assign an index to each vertex of the mesh.
        /// </summary>
        /// <returns>Dictionary containing Vertex-Index assignments.</returns>
        public Dictionary<MeshVertex, int> IndexVertices()
        {
            int i = -1;
            Dictionary<MeshVertex, int> index = new Dictionary<MeshVertex, int>();
            foreach (var v in Vertices)
                index[v] = i++;
            return index;
        }

        /// <summary>
        /// Assign an index to each face of the mesh.
        /// </summary>
        /// <returns>Dictionary containing Face-Index assignments.</returns>
        public Dictionary<MeshFace, int> IndexFaces()
        {
            int i = -1;
            Dictionary<MeshFace, int> index = new Dictionary<MeshFace, int>();
            foreach (var v in Faces)
                index[v] = i++;
            return index;
        }

        /// <summary>
        /// Assign an index to each edge of the mesh.
        /// </summary>
        /// <returns>Dictionary containing Edge-Index assignments.</returns>
        public Dictionary<MeshEdge, int> IndexEdges()
        {
            int i = -1;
            Dictionary<MeshEdge, int> index = new Dictionary<MeshEdge, int>();
            foreach (var v in Edges)
                index[v] = i++;
            return index;
        }

        /// <summary>
        /// Assign an index to each Half-Edge of the mesh.
        /// </summary>
        /// <returns>Dictionary containing HalfEdge-Index assignments.</returns>
        public Dictionary<MeshHalfEdge, int> IndexHalfEdes()
        {
            int i = -1;
            Dictionary<MeshHalfEdge, int> index = new Dictionary<MeshHalfEdge, int>();
            foreach (var f in HalfEdges)
                index[f] = i++;
            return index;
        }

        /// <summary>
        /// Assign an index to each corner of the mesh.
        /// </summary>
        /// <returns>Dictionary containing Corner-Index assignments.</returns>
        public Dictionary<MeshCorner, int> IndexCorners()
        {
            int i = -1;
            Dictionary<MeshCorner, int> index = new Dictionary<MeshCorner, int>();
            foreach (var f in Corners)
                index[f] = i++;
            return index;
        }

        /// <summary>
        /// Check if a mesh is triangular.
        /// </summary>
        /// <returns>Returns true if all faces are triangular.</returns>
        public bool IsTriangularMesh() => IsMesh() == IsMeshResult.Triangular;

        /// <summary>
        /// Check if a mesh is quad.
        /// </summary>
        /// <returns>Returns true if all faces are quads.</returns>
        public bool IsQuadMesh() => IsMesh() == IsMeshResult.Quad;

        /// <summary>
        /// Check if a mesh is n-gonal.
        /// </summary>
        /// <returns>Returns true if the mesh contains ANY ngons.</returns>
        public bool IsNgonMesh() => IsMesh() == IsMeshResult.Ngon;

        /// <summary>
        /// Returns an enum corresponding to the mesh face topology  (triangular, quad or ngon).
        /// </summary>
        private IsMeshResult IsMesh()
        {
            var count = CountFaceEdges();
            if (count.Triangles == this.Faces.Count)
                return IsMeshResult.Triangular;
            if (count.Quads == this.Faces.Count)
                return IsMeshResult.Quad;
            if (count.Ngons != 0)
                return IsMeshResult.Ngon;
            else
                return IsMeshResult.ERROR;
        }

        /// <summary>
        /// Type of mesh (Triangular, Quad, Ngon or Error).
        /// </summary>
        private enum IsMeshResult
        {
            Triangular,
            Quad,
            Ngon,
            ERROR,
        }

        /// <summary>
        /// Get human readable description of this mesh.
        /// </summary>
        /// <returns>Mesh description as text.</returns>
        public string GetMeshInfo()
        {
            const string head = "--- Mesh Info ---\n";

            string vef = "V: " + Vertices.Count + "; F: " + Faces.Count + "; E:" + Edges.Count + "\n";
            string hec = "Half-edges: " + HalfEdges.Count + "; Corners: " + Corners.Count + "\n";
            string bounds = "Boundaries: " + Boundaries.Count + "\n";
            string euler = "Euler characteristic: " + EulerCharacteristic + "\n";
            string isoVert = "Isolated vertices: " + HasIsolatedVertices().ToString() + "\n";
            string isoFace = "Isolated faces: " + HasIsolatedFaces().ToString() + "\n";
            string manifold = "Has Non-Manifold Edges: " + HasNonManifoldEdges().ToString() + "\n";

            FaceData faceData = CountFaceEdges();
            string triangles = "Tri faces: " + faceData.Triangles + "\n";
            string quads = "Quad faces: " + faceData.Quads + "\n";
            string ngons = "Ngon faces: " + faceData.Ngons + "\n";

            const string tail = "-----       -----\n\n";

            return head + vef + hec + bounds + euler + isoVert + isoFace + manifold + triangles + quads + ngons + tail;
        }

        /// <summary>
        /// Gets string representation of the mesh.
        /// </summary>
        /// <returns>Mesh string.</returns>
        public override string ToString()
        {
            string vefh = "V: " + Vertices.Count + "; F: " + Faces.Count + "; E:" + Edges.Count + "; hE: " + HalfEdges.Count;
            return "HE_Mesh{" + vefh + "}";
        }

        private void CreateVertices(List<Point3d> points)
        {
            List<MeshVertex> verts = new List<MeshVertex>(points.Count);

            foreach (Point3d pt in points)
            {
                MeshVertex vertex = new MeshVertex(pt.X, pt.Y, pt.Z);
                verts.Add(vertex);
            }

            Vertices = verts;
        }

        // Takes a List containing another List per face with the vertex indexes belonging to that face
        private bool CreateFaces(List<List<int>> faceIndexes)
        {
            Dictionary<string, int> edgeCount = new Dictionary<string, int>();
            Dictionary<string, MeshHalfEdge> existingHalfEdges = new Dictionary<string, MeshHalfEdge>();
            Dictionary<MeshHalfEdge, bool> hasTwinHalfEdge = new Dictionary<MeshHalfEdge, bool>();

            // Create the faces, edges and half-edges, non-boundary loops and link references when possible;
            foreach (List<int> indexes in faceIndexes)
            {
                MeshFace f = new MeshFace();
                Faces.Add(f);

                List<MeshHalfEdge> tempHEdges = new List<MeshHalfEdge>(indexes.Count);

                // Create empty half-edges
                for (int i = 0; i < indexes.Count; i++)
                {
                    MeshHalfEdge h = new MeshHalfEdge();
                    tempHEdges.Add(h);
                }

                // Fill out each half edge
                for (int i = 0; i < indexes.Count; i++)
                {
                    // Edge goes from v0 to v1
                    int v0 = indexes[i];
                    int v1 = indexes[(i + 1) % indexes.Count];

                    MeshHalfEdge h = tempHEdges[i];

                    // Set previous and next
                    h.Next = tempHEdges[(i + 1) % indexes.Count];
                    h.Prev = tempHEdges[(i + indexes.Count - 1) % indexes.Count];

                    h.OnBoundary = false;
                    hasTwinHalfEdge.Add(h, false);

                    // Set half-edge & vertex mutually
                    h.Vertex = Vertices[v0];
                    Vertices[v0].HalfEdge = h;

                    // Set half-edge face & vice versa
                    h.Face = f;
                    f.HalfEdge = h;

                    // Reverse v0 and v1 if v0 > v1
                    if (v0 > v1)
                    {
                        int temp = v0;
                        v0 = v1;
                        v1 = temp;
                    }

                    string key = v0 + " " + v1;
                    if (existingHalfEdges.ContainsKey(key))
                    {
                        // If this half-edge key already exists, it is the twin of this current half-edge
                        MeshHalfEdge twin = existingHalfEdges[key];
                        h.Twin = twin;
                        twin.Twin = h;
                        h.Edge = twin.Edge;
                        hasTwinHalfEdge[h] = true;
                        hasTwinHalfEdge[twin] = true;
                        edgeCount[key]++;
                    }
                    else
                    {
                        // Create an edge and set its half-edge
                        MeshEdge e = new MeshEdge();
                        Edges.Add(e);
                        h.Edge = e;
                        e.HalfEdge = h;

                        // Record the newly created half-edge
                        existingHalfEdges.Add(key, h);
                        edgeCount.Add(key, 1);
                    }
                }

                HalfEdges.AddRange(tempHEdges);
            }

            // Create boundary edges
            for (int i = 0; i < HalfEdges.Count; i++)
            {
                MeshHalfEdge h = HalfEdges[i];
                if (!hasTwinHalfEdge[h])
                {
                    MeshFace f = new MeshFace();
                    Boundaries.Add(f);

                    List<MeshHalfEdge> boundaryCycle = new List<MeshHalfEdge>();
                    MeshHalfEdge hE = h;
                    do
                    {
                        MeshHalfEdge bH = new MeshHalfEdge();
                        HalfEdges.Add(bH);
                        boundaryCycle.Add(bH);

                        MeshHalfEdge nextHE = hE.Next;
                        while (hasTwinHalfEdge[nextHE])
                            nextHE = nextHE.Twin.Next;

                        bH.Vertex = nextHE.Vertex;
                        bH.Edge = hE.Edge;
                        bH.OnBoundary = true;

                        bH.Face = f;
                        f.HalfEdge = bH;

                        bH.Twin = hE;
                        hE.Twin = bH;

                        hE = nextHE;
                    }
                    while (hE != h);

                    int n = boundaryCycle.Count;
                    for (int j = 0; j < n; j++)
                    {
                        boundaryCycle[j].Next = boundaryCycle[(j + n - 1) % n];
                        boundaryCycle[j].Prev = boundaryCycle[(j + 1) % n];
                        hasTwinHalfEdge[boundaryCycle[j]] = true;
                        hasTwinHalfEdge[boundaryCycle[j].Twin] = true;
                    }
                }

                if (!h.OnBoundary)
                {
                    MeshCorner corner = new MeshCorner
                    {
                        HalfEdge = h,
                    };
                    h.Corner = corner;
                    Corners.Add(corner);
                }
            }

            // Check mesh for common errors
            if (HasIsolatedFaces() || HasIsolatedVertices() || HasNonManifoldEdges())
            {
                return false;
            }

            // Index elements
            IndexElements();

            return true;
        }

        private FaceData CountFaceEdges()
        {
            FaceData data = default;

            foreach (MeshFace face in this.Faces)
            {
                switch (face.AdjacentCorners().Count)
                {
                    case 3:
                        data.Triangles++;
                        break;
                    case 4:
                        data.Quads++;
                        break;
                    default:
                        data.Ngons++;
                        break;
                }
            }

            return data;
        }

        private struct FaceData
        {
            public int Triangles;
            public int Quads;
            public int Ngons;
        }
    }
}