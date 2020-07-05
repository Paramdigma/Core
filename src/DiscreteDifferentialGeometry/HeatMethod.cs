using System;
using System.Collections.Generic;
using MathNet.Numerics.LinearAlgebra.Double;
using Paramdigma.Core.Geometry;
using Paramdigma.Core.HalfEdgeMesh;

namespace Paramdigma.Core.DiscreteDifferentialGeometry
{
    /// <summary>
    /// This class implements the heat method to compute geodesic distance on a surface mesh.
    /// From: http://cs.cmu.edu/~kmcrane/Projects/HeatMethod/
    /// </summary>
    public class HeatMethod
    {
        private readonly Mesh mesh;
        public readonly SparseMatrix laplace;
        public readonly SparseMatrix meanFlow;

        /// <summary>
        /// Initializes a new instance of the <see cref="HeatMethod"/> class.
        /// </summary>
        /// <param name="mesh">Mesh to compute geodesics distances from.</param>
        public HeatMethod(Mesh mesh)
        {
            this.mesh = mesh;
            var el = MeshGeometry.MeanEdgeLength(mesh);
            var t = Math.Pow(MeshGeometry.MeanEdgeLength(mesh), 2);
            var m = MeshGeometry.MassMatrix(mesh);
            this.laplace = MeshGeometry.LaplaceMatrix(mesh);
            var temp = this.laplace.Multiply(t);
            this.meanFlow = m.Add(temp) as SparseMatrix;
        }

        private Dictionary<MeshFace, Vector3d> ComputeVectorField(DenseMatrix u)
        {
            var x = new Dictionary<MeshFace, Vector3d>();
            this.mesh.Faces.ForEach(f =>
            {
                var gradU = new Vector3d();
                f.AdjacentHalfEdges().ForEach(h =>
                {
                    var i = h.Prev.Vertex.Index;
                    var ui = u[i, 0];
                    var ei = MeshGeometry.Vector(h);
                    gradU += f.Normal.Cross(ei) * ui;
                });
                gradU /= 2 * f.Area;
                gradU.Unitize();
                gradU.Negate();
                x[f] = gradU;
            });
            return x;
        }

        private DenseMatrix ComputeDivergence(IReadOnlyDictionary<MeshFace, Vector3d> x)
        {
            var vertices = this.mesh.Vertices;
            var v = this.mesh.Vertices.Count;
            var div = new DenseMatrix(v, 1);
            vertices.ForEach(vertex =>
            {
                var i = vertex.Index;
                var sum = .0;

                vertex.AdjacentHalfEdges().ForEach(h =>
                {
                    if (h.OnBoundary)
                        return;
                    var xj = x[h.Face];
                    var e1 = MeshGeometry.Vector(h);
                    var e2 = MeshGeometry.Vector(h.Prev.Twin);
                    var cotTheta1 = MeshGeometry.Cotan(h);
                    var cotTheta2 = MeshGeometry.Cotan(h.Prev);

                    sum += (cotTheta1 * e1.Dot(xj)) + (cotTheta2 * e2.Dot(xj));
                });
                div[i, 0] = 0.5 * sum;
            });
            return div;
        }

        private static void SubtractMinimumDistance(DenseMatrix phi)
        {
            var min = double.PositiveInfinity;
            for (int i = 0; i < phi.RowCount; i++)
            {
                min = Math.Min(phi[i, 0], min);
            }

            for (int i = 0; i < phi.RowCount; i++)
            {
                phi[i, 0] -= min;
            }
        }

        /// <summary>
        /// Computes the geodesic distances φ using the heat method.
        /// </summary>
        /// <param name="delta">A dense vector (i.e., delta.nCols() == 1) containing heat sources, i.e., u0 = δ(x).</param>
        /// <returns>Dense matrix with distances.</returns>
        public DenseMatrix Compute(DenseMatrix delta)
        {
            var llt = this.meanFlow.Cholesky();
            var u = llt.Solve(delta) as DenseMatrix;

            var x = ComputeVectorField(u);
            var div = ComputeDivergence(x);

            llt = this.laplace.Cholesky();
            var phi = llt.Solve(div.Negate()) as DenseMatrix;
            SubtractMinimumDistance(phi);

            return phi;
        }
    }
}