﻿using System;
using System.Collections.Generic;
using Paramdigma.Core.Geometry;

namespace Paramdigma.Core.HalfEdgeMesh
{
    /// <summary>
    ///     Represents a Half-Edge Mesh data structure.
    /// </summary>
    public class Mesh
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Mesh" /> class.
        /// </summary>
        public Mesh()
        {
            this.Vertices = new List<MeshVertex>();
            this.Edges = new List<MeshEdge>();
            this.Faces = new List<MeshFace>();
            this.Corners = new List<MeshCorner>();
            this.HalfEdges = new List<MeshHalfEdge>();
            this.Boundaries = new List<MeshFace>();
        }


        /// <summary>
        ///     Initializes a new instance of the <see cref="Mesh" /> class from verticees and faces.
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
            var result = this.CreateFaces(faceIndexes);
            if (!result)
                throw new Exception("Couldn't create faces for this mesh");
        }


        /// <summary>
        ///     Initializes a new instance of the <see cref="Mesh" /> class from verticees and faces.
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
        ///     Initializes a new instance of the <see cref="Mesh" /> class from an existing one.
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
        ///     Gets or sets the vertices of the mesh.
        /// </summary>
        public List<MeshVertex> Vertices { get; set; }

        /// <summary>
        ///     Gets or sets the edges of the mesh.
        /// </summary>
        public List<MeshEdge> Edges { get; set; }

        /// <summary>
        ///     Gets or sets the faces of the mesh.
        /// </summary>
        public List<MeshFace> Faces { get; set; }

        /// <summary>
        ///     Gets or sets the corners of the mesh.
        /// </summary>
        public List<MeshCorner> Corners { get; set; }

        /// <summary>
        ///     Gets or sets the half-edges of the mesh.
        /// </summary>
        public List<MeshHalfEdge> HalfEdges { get; set; }

        /// <summary>
        ///     Gets or sets the boundaries of the mesh.
        /// </summary>
        public List<MeshFace> Boundaries { get; set; }

        /// <summary>
        ///     Gets the euler characteristic of the mesh.
        /// </summary>
        public int EulerCharacteristic => this.Vertices.Count - this.Edges.Count + this.Faces.Count;


        /// <summary>
        ///     Check if the mesh has isolated vertices.
        /// </summary>
        /// <returns>True if there are isolated vertices, false if not.</returns>
        public bool HasIsolatedVertices()
        {
            foreach (var v in this.Vertices)
            {
                if (v.IsIsolated())
                    return true;
            }

            return false;
        }


        /// <summary>
        ///     Check if the mesh contains isolated faces.
        /// </summary>
        /// <returns>True if there are isolated faces, false if not.</returns>
        public bool HasIsolatedFaces()
        {
            foreach (var f in this.Faces)
            {
                var boundaryEdges = 0;
                var adjacent = f.AdjacentHalfEdges();
                foreach (var e in adjacent)
                {
                    if (e.OnBoundary)
                        boundaryEdges++;
                }

                if (boundaryEdges == adjacent.Count)
                    return true;
            }

            return false;
        }


        /// <summary>
        ///     Check if the mesh contains non-manifold edges.
        /// </summary>
        /// <returns>True if there are non-manifold edges, false if not.</returns>
        public bool HasNonManifoldEdges()
        {
            foreach (var edge in this.Edges)
            {
                if (edge.AdjacentFaces().Count > 2)
                    return true;
            }

            return false;
        }


        /// <summary>
        ///     Assign an index number to each mesh member.
        /// </summary>
        public void IndexElements()
        {
            var index = -1;
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
        ///     Assign an index to each vertex of the mesh.
        /// </summary>
        /// <returns>Dictionary containing Vertex-Index assignments.</returns>
        public Dictionary<MeshVertex, int> IndexVertices()
        {
            var i = -1;
            var index = new Dictionary<MeshVertex, int>();
            foreach (var v in this.Vertices)
                index[v] = i++;
            return index;
        }


        /// <summary>
        ///     Assign an index to each face of the mesh.
        /// </summary>
        /// <returns>Dictionary containing Face-Index assignments.</returns>
        public Dictionary<MeshFace, int> IndexFaces()
        {
            var i = -1;
            var index = new Dictionary<MeshFace, int>();
            foreach (var v in this.Faces)
                index[v] = i++;
            return index;
        }


        /// <summary>
        ///     Assign an index to each edge of the mesh.
        /// </summary>
        /// <returns>Dictionary containing Edge-Index assignments.</returns>
        public Dictionary<MeshEdge, int> IndexEdges()
        {
            var i = -1;
            var index = new Dictionary<MeshEdge, int>();
            foreach (var v in this.Edges)
                index[v] = i++;
            return index;
        }


        /// <summary>
        ///     Assign an index to each Half-Edge of the mesh.
        /// </summary>
        /// <returns>Dictionary containing HalfEdge-Index assignments.</returns>
        public Dictionary<MeshHalfEdge, int> IndexHalfEdes()
        {
            var i = -1;
            var index = new Dictionary<MeshHalfEdge, int>();
            foreach (var f in this.HalfEdges)
                index[f] = i++;
            return index;
        }


        /// <summary>
        ///     Assign an index to each corner of the mesh.
        /// </summary>
        /// <returns>Dictionary containing Corner-Index assignments.</returns>
        public Dictionary<MeshCorner, int> IndexCorners()
        {
            var i = -1;
            var index = new Dictionary<MeshCorner, int>();
            foreach (var f in this.Corners)
                index[f] = i++;
            return index;
        }


        /// <summary>
        ///     Check if a mesh is triangular.
        /// </summary>
        /// <returns>Returns true if all faces are triangular.</returns>
        public bool IsTriangularMesh() => this.IsMesh() == IsMeshResult.Triangular;


        /// <summary>
        ///     Check if a mesh is quad.
        /// </summary>
        /// <returns>Returns true if all faces are quads.</returns>
        public bool IsQuadMesh() => this.IsMesh() == IsMeshResult.Quad;


        /// <summary>
        ///     Check if a mesh is n-gonal.
        /// </summary>
        /// <returns>Returns true if the mesh contains ANY ngons.</returns>
        public bool IsNgonMesh() => this.IsMesh() == IsMeshResult.Ngon;


        /// <summary>
        ///     Returns an enum corresponding to the mesh face topology  (triangular, quad or ngon).
        /// </summary>
        private IsMeshResult IsMesh()
        {
            var count = this.CountFaceEdges();
            if (count.Triangles == this.Faces.Count)
                return IsMeshResult.Triangular;
            if (count.Quads == this.Faces.Count)
                return IsMeshResult.Quad;
            if (count.Ngons != 0)
                return IsMeshResult.Ngon;
            return IsMeshResult.Error;
        }


        /// <summary>
        ///     Get human readable description of this mesh.
        /// </summary>
        /// <returns>Mesh description as text.</returns>
        public string GetMeshInfo()
        {
            const string head = "--- Mesh Info ---\n";

            var vef = "V: " + this.Vertices.Count + "; F: " + this.Faces.Count + "; E:"
                    + this.Edges.Count + "\n";
            var hec = "Half-edges: " + this.HalfEdges.Count + "; Corners: " + this.Corners.Count
                    + "\n";
            var bounds = "Boundaries: " + this.Boundaries.Count + "\n";
            var euler = "Euler characteristic: " + this.EulerCharacteristic + "\n";
            var isoVert = "Isolated vertices: " + this.HasIsolatedVertices() + "\n";
            var isoFace = "Isolated faces: " + this.HasIsolatedFaces() + "\n";
            var manifold = "Has Non-Manifold Edges: " + this.HasNonManifoldEdges() + "\n";

            var faceData = this.CountFaceEdges();
            var triangles = "Tri faces: " + faceData.Triangles + "\n";
            var quads = "Quad faces: " + faceData.Quads + "\n";
            var ngons = "Ngon faces: " + faceData.Ngons + "\n";

            const string tail = "-----       -----\n\n";

            return head + vef + hec + bounds + euler + isoVert + isoFace + manifold + triangles
                 + quads + ngons + tail;
        }


        /// <summary>
        ///     Gets string representation of the mesh.
        /// </summary>
        /// <returns>Mesh string.</returns>
        public override string ToString()
        {
            var vefh = "V: " + this.Vertices.Count + "; F: " + this.Faces.Count + "; E:"
                     + this.Edges.Count
                     + "; hE: " + this.HalfEdges.Count;
            return "HE_Mesh{" + vefh + "}";
        }


        private void CreateVertices(List<Point3d> points)
        {
            var verts = new List<MeshVertex>(points.Count);

            foreach (var pt in points)
            {
                var vertex = new MeshVertex(pt.X, pt.Y, pt.Z);
                verts.Add(vertex);
            }

            this.Vertices = verts;
        }


        // Takes a List containing another List per face with the vertex indexes belonging to that face
        private bool CreateFaces(IEnumerable<List<int>> faceIndexes)
        {
            var edgeCount = new Dictionary<string, int>();
            var existingHalfEdges = new Dictionary<string, MeshHalfEdge>();
            var hasTwinHalfEdge = new Dictionary<MeshHalfEdge, bool>();

            // Create the faces, edges and half-edges, non-boundary loops and link references when possible;
            foreach (var indexes in faceIndexes)
            {
                var f = new MeshFace();
                this.Faces.Add(f);

                var tempHEdges = new List<MeshHalfEdge>(indexes.Count);

                // Create empty half-edges
                for (var i = 0; i < indexes.Count; i++)
                {
                    var h = new MeshHalfEdge();
                    tempHEdges.Add(h);
                }

                // Fill out each half edge
                for (var i = 0; i < indexes.Count; i++)
                {
                    // Edge goes from v0 to v1
                    var v0 = indexes[i];
                    var v1 = indexes[(i + 1) % indexes.Count];

                    var h = tempHEdges[i];

                    // Set previous and next
                    h.Next = tempHEdges[(i + 1) % indexes.Count];
                    h.Prev = tempHEdges[(i + indexes.Count - 1) % indexes.Count];

                    h.OnBoundary = false;
                    hasTwinHalfEdge.Add(h, false);

                    // Set half-edge & vertex mutually
                    h.Vertex = this.Vertices[v0];
                    this.Vertices[v0].HalfEdge = h;

                    // Set half-edge face & vice versa
                    h.Face = f;
                    f.HalfEdge = h;

                    // Reverse v0 and v1 if v0 > v1
                    if (v0 > v1)
                    {
                        var temp = v0;
                        v0 = v1;
                        v1 = temp;
                    }

                    var key = v0 + " " + v1;
                    if (existingHalfEdges.ContainsKey(key))
                    {
                        // If this half-edge key already exists, it is the twin of this current half-edge
                        var twin = existingHalfEdges[key];
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
                        var e = new MeshEdge();
                        this.Edges.Add(e);
                        h.Edge = e;
                        e.HalfEdge = h;

                        // Record the newly created half-edge
                        existingHalfEdges.Add(key, h);
                        edgeCount.Add(key, 1);
                    }
                }

                this.HalfEdges.AddRange(tempHEdges);
            }

            // Create boundary edges
            for (var i = 0; i < this.HalfEdges.Count; i++)
            {
                var h = this.HalfEdges[i];
                if (!hasTwinHalfEdge[h])
                {
                    var f = new MeshFace();
                    this.Boundaries.Add(f);

                    var boundaryCycle = new List<MeshHalfEdge>();
                    var halfEdge = h;
                    do
                    {
                        var boundaryHalfEdge = new MeshHalfEdge();
                        this.HalfEdges.Add(boundaryHalfEdge);
                        boundaryCycle.Add(boundaryHalfEdge);

                        var nextHalfEdge = halfEdge.Next;
                        while (hasTwinHalfEdge[nextHalfEdge])
                            nextHalfEdge = nextHalfEdge.Twin.Next;

                        boundaryHalfEdge.Vertex = nextHalfEdge.Vertex;
                        boundaryHalfEdge.Edge = halfEdge.Edge;
                        boundaryHalfEdge.OnBoundary = true;
                        
                        boundaryHalfEdge.Face = f;
                        f.HalfEdge = boundaryHalfEdge;
                        
                        boundaryHalfEdge.Twin = halfEdge;
                        halfEdge.Twin = boundaryHalfEdge;

                        halfEdge = nextHalfEdge;
                    } while (halfEdge != h);

                    var n = boundaryCycle.Count;
                    for (var j = 0; j < n; j++)
                    {
                        boundaryCycle[j].Next = boundaryCycle[(j + n - 1) % n];
                        boundaryCycle[j].Prev = boundaryCycle[(j + 1) % n];
                        hasTwinHalfEdge[boundaryCycle[j]] = true;
                        hasTwinHalfEdge[boundaryCycle[j].Twin] = true;
                    }
                }

                if (h.OnBoundary)
                    continue;
                
                var corner = new MeshCorner {HalfEdge = h};
                h.Corner = corner;
                this.Corners.Add(corner);
            }

            // Index elements
            this.IndexElements();

            // Check mesh for common errors
            var check = !this.HasIsolatedFaces();
            var check2 = !this.HasIsolatedVertices();
            var check3 = !this.HasNonManifoldEdges();
            return check && check2 && check3;
        }


        private FaceData CountFaceEdges()
        {
            FaceData data = default;

            foreach (var face in this.Faces)
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


        /// <summary>
        ///     Type of mesh (Triangular, Quad, Ngon or Error).
        /// </summary>
        private enum IsMeshResult
        {
            Triangular, Quad, Ngon, Error
        }

        private struct FaceData
        {
            public int Triangles;
            public int Quads;
            public int Ngons;
        }
    }
}