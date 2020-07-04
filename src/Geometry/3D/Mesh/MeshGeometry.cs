using System;
using System.Linq;
using MathNet.Numerics.LinearAlgebra.Double;
using Complex = MathNet.Numerics.LinearAlgebra.Complex;
using Paramdigma.Core.HalfEdgeMesh;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    ///     Represents the geometry of a HE_Mesh such as positions at vertices.
    /// </summary>
    public static class MeshGeometry
    {
        /// <summary>
        ///     Calculate the vector of a specified half-edge.
        /// </summary>
        /// <returns>The half-edge vector.</returns>
        /// <param name="halfEdge">Half edge.</param>
        public static Vector3d Vector(MeshHalfEdge halfEdge) =>
            halfEdge.Vertex - halfEdge.Next.Vertex;


        /// <summary>
        ///     Calculates the length of the specified edge.
        /// </summary>
        /// <returns>The length.</returns>
        /// <param name="edge">Edge.</param>
        public static double Length(MeshEdge edge) => Vector(edge.HalfEdge).Length;


        /// <summary>
        ///     Calculates the midpoint of the specifiec edge.
        /// </summary>
        /// <returns>The point.</returns>
        /// <param name="edge">Edge.</param>
        public static Point3d MidPoint(MeshEdge edge)
        {
            var halfEdge = edge.HalfEdge;
            Point3d a = halfEdge.Vertex;
            Point3d b = halfEdge.Twin.Vertex;
            return (a + b) / 2;
        }


        /// <summary>
        ///     Calculates the mean edge length of the mesh.
        /// </summary>
        /// <param name="mesh">Mesh.</param>
        /// <returns>The mean edge length of the mesh.</returns>
        public static double MeanEdgeLength(Mesh mesh)
        {
            var sum = 0.0;
            foreach (var e in mesh.Edges)
                sum += Length(e);
            return sum / mesh.Edges.Count;
        }


        /// <summary>
        ///     Computes the area of the specified face.
        /// </summary>
        /// <returns>The face area.</returns>
        /// <param name="face">Face.</param>
        public static double Area(MeshFace face)
        {
            if (face.IsBoundaryLoop())
                return 0.0;

            var u = Vector(face.HalfEdge);
            var v = -Vector(face.HalfEdge.Prev);
            return 0.5 * u.Cross(v).Length;
        }


        /// <summary>
        ///     Computes the total area of the mesh.
        /// </summary>
        /// <param name="mesh">Mesh.</param>
        /// <returns>The mesh area.</returns>
        public static double TotalArea(Mesh mesh)
        {
            var sum = 0.0;
            foreach (var f in mesh.Faces)
                sum += Area(f);
            return sum;
        }


        /// <summary>
        ///     Compute the normal vector of the specified face.
        /// </summary>
        /// <returns>The normal.</returns>
        /// <param name="face">Face.</param>
        public static Vector3d FaceNormal(MeshFace face)
        {
            if (face.IsBoundaryLoop())
                return null;

            var u = Vector(face.HalfEdge);
            var v = -Vector(face.HalfEdge.Prev);
            return u.Cross(v).Unit();
        }


        /// <summary>
        ///     Compute the centroid of the specified face.
        /// </summary>
        /// <returns>The centroid.</returns>
        /// <param name="face">Face.</param>
        public static Point3d Centroid(MeshFace face)
        {
            var hE = face.HalfEdge;
            Point3d a = hE.Vertex;
            Point3d b = hE.Next.Vertex;
            Point3d c = hE.Prev.Vertex;

            if (face.IsBoundaryLoop())
                return (a + b) / 2;

            return (a + b + c) / 3;
        }


        /// <summary>
        ///     Compute the circumcenter the specified face.
        /// </summary>
        /// <returns>The circumcenter.</returns>
        /// <param name="face">Face.</param>
        public static Point3d Circumcenter(MeshFace face)
        {
            var hE = face.HalfEdge;

            Point3d a = hE.Vertex;
            Point3d b = hE.Next.Vertex;
            Point3d c = hE.Prev.Vertex;

            if (face.IsBoundaryLoop())
                return (a + b) / 2;

            var ac = c - a;
            var ab = b - a;
            var w = ab.Cross(ac);

            var u = w.Cross(ab) * ac.LengthSquared;
            var v = ac.Cross(w) * ab.LengthSquared;

            var x = ( Point3d ) (u + v) / (2 * w.LengthSquared);

            return x + a;
        }


        /// <summary>
        ///     Compute the orthonormal bases of the specified face.
        /// </summary>
        /// <returns>Array containing the 2 Vector3d.</returns>
        /// <param name="face">Face.</param>
        public static Vector3d[] OrthonormalBases(MeshFace face)
        {
            var e1 = Vector(face.HalfEdge).Unit();
            var normal = FaceNormal(face);
            var e2 = normal.Cross(e1);

            return new[] {e1, e2};
        }


        /// <summary>
        ///     Compute the angle (in radians) at the specified corner.
        /// </summary>
        /// <returns>The angle (in radians).</returns>
        /// <param name="corner">Corner.</param>
        public static double Angle(MeshCorner corner)
        {
            var u = Vector(corner.HalfEdge.Prev).Unit();
            var v = -Vector(corner.HalfEdge.Next).Unit();

            return Math.Acos(Math.Max(-1, Math.Min(1.0, u.Dot(v))));
        }


        /// <summary>
        ///     Computes the cotangent of the angle opposite to a halfedge.
        /// </summary>
        /// <returns>The cotan.</returns>
        /// <param name="hE">H e.</param>
        public static double Cotan(MeshHalfEdge hE)
        {
            if (hE.OnBoundary)
                return 0.0;

            var u = Vector(hE.Prev);
            var v = -Vector(hE.Next);

            return u.Dot(v) / u.Cross(v).Length;
        }


        /// <summary>
        ///     Computes the signed angle (in radians) between the faces adjacent to the specified half-edge.
        /// </summary>
        /// <returns>The angle (in radians) between faces.</returns>
        /// <param name="hE">H e.</param>
        public static double DihedralAngle(MeshHalfEdge hE)
        {
            if (hE.OnBoundary || hE.Twin.OnBoundary)
                return 0.0;

            var n1 = FaceNormal(hE.Face);
            var n2 = FaceNormal(hE.Twin.Face);
            var w = Vector(hE).Unit();

            var cosTheta = n1.Dot(n2);
            var sinTheta = n1.Cross(n2).Dot(w);

            return Math.Atan2(sinTheta, cosTheta);
        }


        /// <summary>
        ///     Computes the barycentric dual area around a given mesh vertex.
        /// </summary>
        /// <returns>The dual area.</returns>
        /// <param name="vertex">Vertex.</param>
        public static double BarycentricDualArea(MeshVertex vertex)
        {
            var area = 0.0;
            foreach (var f in vertex.AdjacentFaces())
                area += Area(f);
            return area;
        }


        /// <summary>
        ///     Computes the circumcentric dual area around a given mesh vertex.
        /// </summary>
        /// <returns>The dualarea.</returns>
        /// <param name="vertex">Vertex.</param>
        public static double CircumcentricDualArea(MeshVertex vertex)
        {
            var area = 0.0;
            foreach (var hE in vertex.AdjacentHalfEdges())
            {
                var u2 = Vector(hE.Prev).LengthSquared;
                var v2 = Vector(hE).LengthSquared;
                var cotAlpha = Cotan(hE.Prev);
                var cotBeta = Cotan(hE);

                area += (u2 * cotAlpha + v2 * cotBeta) / 8;
            }

            return area;
        }


        /// <summary>
        ///     Computes the equally weighted normal arround the specified vertex.
        /// </summary>
        /// <returns>The normal vector at that vertex.</returns>
        /// <param name="vertex">Vertex.</param>
        public static Vector3d VertexNormalEquallyWeighted(MeshVertex vertex)
        {
            var n = new Vector3d();
            foreach (var f in vertex.AdjacentFaces())
                n += FaceNormal(f);

            return n.Unit();
        }


        /// <summary>
        ///     Computes the area weighted normal arround the specified vertex.
        /// </summary>
        /// <returns>The normal vector at that vertex.</returns>
        /// <param name="vertex">Vertex.</param>
        public static Vector3d VertexNormalAreaWeighted(MeshVertex vertex)
        {
            var n = new Vector3d();
            foreach (var f in vertex.AdjacentFaces())
            {
                var normal = FaceNormal(f);
                var area = Area(f);

                n += normal * area;
            }

            return n.Unit();
        }


        /// <summary>
        ///     Computes the angle weighted normal arround the specified vertex.
        /// </summary>
        /// <returns>The normal vector at that vertex.</returns>
        /// <param name="vertex">Vertex.</param>
        public static Vector3d VertexNormalAngleWeighted(MeshVertex vertex)
        {
            var n = new Vector3d();
            foreach (var c in vertex.AdjacentCorners())
            {
                var normal = FaceNormal(c.HalfEdge.Face);
                var angle = Angle(c);

                n += normal * angle;
            }

            return n.Unit();
        }


        /// <summary>
        ///     Computes the gauss curvature weighted normal arround the specified vertex.
        /// </summary>
        /// <returns>The normal vector at that vertex.</returns>
        /// <param name="vertex">Vertex.</param>
        public static Vector3d VertexNormalGaussCurvature(MeshVertex vertex)
        {
            var n = new Vector3d();
            foreach (var hE in vertex.AdjacentHalfEdges())
            {
                var weight = 0.5 * DihedralAngle(hE) / Length(hE.Edge);
                n -= Vector(hE) * weight;
            }

            return n.Unit();
        }


        /// <summary>
        ///     Computes the mean curvature weighted normal arround the specified vertex.
        /// </summary>
        /// <returns>The normal vector at that vertex.</returns>
        /// <param name="vertex">Vertex.</param>
        public static Vector3d VertexNormalMeanCurvature(MeshVertex vertex)
        {
            var n = new Vector3d();
            foreach (var hE in vertex.AdjacentHalfEdges())
            {
                var weight = 0.5 * Cotan(hE) + Cotan(hE.Twin);
                n -= Vector(hE) * weight;
            }

            return n.Unit();
        }


        /// <summary>
        ///     Computes the sphere inscribed normal arround the specified vertex.
        /// </summary>
        /// <returns>The normal vector at that vertex.</returns>
        /// <param name="vertex">Vertex.</param>
        public static Vector3d VertexNormalSphereInscribed(MeshVertex vertex)
        {
            var n = new Vector3d();
            foreach (var c in vertex.AdjacentCorners())
            {
                var u = Vector(c.HalfEdge.Prev);
                var v = -Vector(c.HalfEdge.Next);

                n += u.Cross(v) / (u.LengthSquared * v.LengthSquared);
            }

            return n.Unit();
        }


        /// <summary>
        ///     Computes the angle defect at the given vertex.
        /// </summary>
        /// <param name="vertex">Vertex to compute angle defect.</param>
        /// <returns>Number representing the deviation of the current vertex from $2\PI$.</returns>
        public static double AngleDefect(MeshVertex vertex)
        {
            var angleSum = 0.0;
            foreach (var c in vertex.AdjacentCorners())
                angleSum += Angle(c);

            // if (vertex.OnBoundary()) angleSum = Math.PI - angleSum;
            return vertex.OnBoundary() ? Math.PI - angleSum : 2 * Math.PI - angleSum;
        }


        /// <summary>
        ///     Compute the Gaussian curvature at the given vertex.
        /// </summary>
        /// <param name="vertex">Vertex to compute Gaussian curvature.</param>
        /// <returns>Number representing the gaussian curvature at that vertex.</returns>
        public static double ScalarGaussCurvature(MeshVertex vertex) => AngleDefect(vertex);


        /// <summary>
        ///     Compute the Mean curvature at the given vertex.
        /// </summary>
        /// <param name="vertex">Vertex to compute Mean curvature.</param>
        /// <returns>Number representing the Mean curvature at that vertex.</returns>
        public static double ScalarMeanCurvature(MeshVertex vertex)
        {
            var sum = 0.0;
            foreach (var hE in vertex.AdjacentHalfEdges())
                sum += 0.5 * Length(hE.Edge) * DihedralAngle(hE);
            return sum;
        }


        /// <summary>
        ///     Compute the total angle defect of the mesh.
        /// </summary>
        /// <param name="mesh">Mesh to compute angle defect.</param>
        /// <returns>Returns the total angle defect as a scalar value.</returns>
        public static double TotalAngleDefect(Mesh mesh)
        {
            var totalDefect = 0.0;
            foreach (var v in mesh.Vertices)
                totalDefect += AngleDefect(v);
            return totalDefect;
        }


        /// <summary>
        ///     Compute the principal curvature scalar values at a given vertes.
        /// </summary>
        /// <param name="vertex">Vertex to compute the curvature.</param>
        /// <returns>Returns an array of 2 values {k1, k2}.</returns>
        public static double[] PrincipalCurvatures(MeshVertex vertex)
        {
            var a = CircumcentricDualArea(vertex);
            var h = ScalarMeanCurvature(vertex) / a;
            var k = AngleDefect(vertex) / a;

            var discriminant = h * h - k;
            if (discriminant > 0)
                discriminant = Math.Sqrt(discriminant);
            else
                discriminant = 0;

            var k1 = h - discriminant;
            var k2 = h + discriminant;

            return new[] {k1, k2};
        }

        /// <summary>
        /// Builds a sparse laplace matrix. The laplace operator is negative semidefinite; instead we build a positive definite matrix by multiplying the entries of the laplace matrix by -1 and shifting the diagonal elements by a small constant (e.g. 1e-8).
        /// </summary>
        /// <param name="mesh">Mesh to compute the matrix from.</param>
        /// <returns>Laplace matrix as a <see cref="SparseMatrix"/> instance.</returns>
        public static SparseMatrix LaplaceMatrix(Mesh mesh)
        {
            var v = mesh.Vertices.Count;
            var matrix = new SparseMatrix(v, v);
            mesh.Vertices.ForEach(vertex =>
            {
                var i = vertex.Index;
                var sum = double.MinValue;
                vertex.AdjacentHalfEdges().ForEach(hE =>
                {
                    var j = hE.Twin.Vertex.Index;
                    var weight = (Cotan(hE) + Cotan(hE.Twin)) / 2;
                    sum += weight;

                    matrix[i, j] = -weight;
                });
                matrix[i, i] = sum;
            });
            return matrix;
        }

        /// <summary>
        /// Builds a sparse diagonal mass matrix containing the barycentric dual area of each vertex of a mesh.
        /// </summary>
        /// <param name="mesh">Mesh to compute mass matrix from.</param>
        /// <returns>Mass matrix of the mesh as a <see cref="SparseMatrix"/> instance.</returns>
        public static SparseMatrix MassMatrix(Mesh mesh)
        {
            var v = mesh.Vertices.Count;
            var matrix = new SparseMatrix(v, v);
            mesh.Vertices.ForEach(vertex =>
            {
                var i = vertex.Index;
                matrix[i, i] = BarycentricDualArea(vertex);
            });
            return matrix;
        }

        /// <summary>
        /// Builds a sparse complex laplace matrix. The laplace operator is negative semidefinite; instead we build a positive definitive matrix by multiplyng the entries of the laplace matrix by -1 and shifting the diagonal elements by a small constant.
        /// </summary>
        /// <param name="mesh">The mesh to compute the matrix from</param>
        /// <returns>The complex laplace matrix of the mesh.</returns>
        public static Complex.SparseMatrix ComplexLaplaceMatrix(Mesh mesh)
        {
            var v = mesh.Vertices.Count;
            var matrix = new Complex.SparseMatrix(v, v);
            mesh.Vertices.ForEach(vertex =>
            {
                var i = vertex.Index;
                var sum = double.MinValue;
                vertex.AdjacentHalfEdges().ForEach(h =>
                {
                    var j = h.Twin.Vertex.Index;
                    var weight = (Cotan(h) + Cotan(h.Twin)) / 2;
                    sum += weight;
                    matrix[i, j] = new System.Numerics.Complex(-weight, 0);
                });
                matrix[i, i] = new System.Numerics.Complex(sum, 0);
            });
            return matrix;
        }

        /// <summary>
        /// Centers a mesh about the origin and rescales it to unit radius.
        /// </summary>
        /// <param name="mesh">Mesh to center.</param>
        /// <param name="rescale">Flag to rescale to unit sphere.</param>
        public static void Normalize(Mesh mesh, bool rescale)
        {
            var cm = new Vector3d();
            mesh.Vertices.ForEach(vertex => cm += vertex);
            cm /= mesh.Vertices.Count;
            var radius = -1.0;

            mesh.Vertices.ForEach(vertex =>
            {
                vertex.Substract(cm);
                radius = Math.Max(radius, ((Vector3d)vertex).Length);
            });

            if (!rescale)
                return;

            foreach (MeshVertex vertex in mesh.Vertices)
            {
                vertex.Divide(radius);
            }
        }
    }
}