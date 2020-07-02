using System;
using MathNet.Numerics.LinearAlgebra.Double;
using Paramdigma.Core.Geometry;
using Paramdigma.Core.HalfEdgeMesh;

namespace Paramdigma.Core.DiscreteDifferentialGeometry
{
    /// <summary>
    /// This class contains methods to build common discrete exterior calculus operators.
    /// From: https://cs.cmu.edu/~kmcrane/Projects/DDG/paper.pdf.
    /// </summary>
    public static class DiscreteExteriorCalculus
    {
        public static SparseMatrix BuildHodgeStar0Form(Mesh mesh)
        {
            var v = mesh.Vertices.Count;
            var matrix = new SparseMatrix(v, v);
            mesh.Vertices.ForEach(vertex =>
            {
                var i = vertex.Index;
                var area = MeshGeometry.BarycentricDualArea(vertex);
                matrix[i, i] = area;
            });
            return matrix;
        }

        public static Matrix BuildHodgeStar1Form(Mesh mesh)
        {
            var e = mesh.Edges.Count;
            var matrix = new SparseMatrix(e, e);
            mesh.Edges.ForEach(edge =>
            {
                var i = edge.Index;
                var w = (MeshGeometry.Cotan(edge.HalfEdge) + MeshGeometry.Cotan(edge.HalfEdge.Twin)) / 2;
                matrix[i, i] = w;
            });
            return matrix;
        }

        public static Matrix BuildHodgeStar2Form(Mesh mesh)
        {
            var v = mesh.Faces.Count;
            var matrix = new SparseMatrix(v, v);
            mesh.Faces.ForEach(face =>
            {
                var i = face.Index;
                var area = MeshGeometry.Area(face);
                matrix[i, i] = 1 / area;
            });
            return matrix;
        }

        public static Matrix BuildExteriorDerivative0Form(Mesh mesh)
        {
            var e = mesh.Edges.Count;
            var v = mesh.Vertices.Count;
            var matrix = new SparseMatrix(e, v);
            mesh.Edges.ForEach(edge =>
            {
                var i = edge.Index;
                var j = edge.HalfEdge.Vertex.Index;
                var k = edge.HalfEdge.Twin.Vertex.Index;
                matrix[i, j] = 1;
                matrix[i, k] = -1;
            });
            return matrix;
        }

        public static Matrix BuildExteriorDerivative1Form(Mesh mesh)
        {
            var e = mesh.Edges.Count;
            var f = mesh.Faces.Count;
            var matrix = new SparseMatrix(f, e);
            mesh.Faces.ForEach(face =>
            {
                var i = face.Index;
                face.AdjacentHalfEdges().ForEach(hE =>
                {
                    var j = hE.Edge.Index;
                    var sign = hE.Edge.HalfEdge == hE ? 1 : -1;
                    matrix[i, j] = sign;
                });
            });
            return matrix;
        }
    }
}