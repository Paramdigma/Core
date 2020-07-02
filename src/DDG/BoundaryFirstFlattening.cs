using System.Collections.Generic;
using Paramdigma.Core.HalfEdgeMesh;
using MathNet.Numerics.LinearAlgebra.Double;
using System;
using Paramdigma.Core.Geometry;

class BoundaryFirstFlattening
{
    /// <summary>
    /// The input geometry of the mesh this class acts on.
    /// </summary>
    public Mesh Mesh;
    /// <summary>
    /// The boundary of the input mesh.
    /// </summary>
    public MeshFace boundary;
    public int nV;
    public int nI;
    public int nB;
    /// <summary>
    /// The integrated gaussian curvatures of the input mesh.
    /// </summary>
    public DenseMatrix K;
    /// <summary>
    /// The integrated geodesic curvatures of the input mesh.
    /// </summary>
    public DenseMatrix k;
    /// <summary>
    /// The boundary edge lengths of the input mesh.
    /// </summary>
    public DenseMatrix l;
    /// <summary>
    /// The laplace matrix of the input mesh partitioned by interior and boundary vertices.
    /// </summary>
    public SparseMatrix A;
    /// <summary>
    /// The upper left block of the partitioned laplace matrix.
    /// </summary>
    public SparseMatrix Aii;
    /// <summary>
    /// The upper right block of the partitioned laplace matrix.
    /// </summary>
    public SparseMatrix Aib;
    /// <summary>
    /// The lower right block of the partitioned laplace matrix.
    /// </summary>
    public SparseMatrix Abb;
    public BoundaryFirstFlattening(Mesh mesh)
    {
        this.Mesh = mesh;
        this.boundary = mesh.Boundaries[0];
        this.ComputeItegrateCurvatures();
        this.ComputeBoundaryLengths();
        this.A = MeshGeometry.LaplaceMatrix(mesh);
        this.Aii = this.A.SubMatrix(0, this.nI, 0, this.nI) as SparseMatrix;
        this.Aib = this.A.SubMatrix(0, this.nI, this.nI, this.nV) as SparseMatrix;
        this.Abb = this.A.SubMatrix(this.nI, this.nV, this.nI, this.nV) as SparseMatrix;
    }
    public void ComputeItegrateCurvatures()
    {

    }
    public void ComputeBoundaryLengths()
    {
        this.l = DenseMatrix.Build.Dense(this.nB, 1) as DenseMatrix;
        this.boundary.AdjacentHalfEdges().ForEach(he =>
        {
            var i = he.Vertex.Index;
            this.l[i, 0] = MeshGeometry.Length(he.Edge);
        });
    }
    private DenseMatrix ComputeTargetBoundaryLengths(DenseMatrix u)
    {
        var lstar = DenseMatrix.Build.Dense(this.nB, 1);
        this.boundary.AdjacentHalfEdges().ForEach(he =>
        {
            var i = he.Vertex.Index;
            var j = he.Next.Vertex.Index;
            var ui = u[i, 0];
            var uj = u[j, 0];
            var lij = this.l[i, 0];
            lstar[i, 0] = Math.Exp((ui + uj) / 2) * lij;
        });
        return lstar as DenseMatrix;
    }
    private DenseMatrix ComputeDualBoundaryLengths(Dictionary<MeshVertex, Vector3d> flattening)
    {

        throw new NotImplementedException();
    }
    private DenseMatrix DirichletToNeumann(DenseMatrix phi, DenseMatrix g)
    {
        var llt = this.Aii.Cholesky();
        var a = llt.Solve(phi.Subtract(this.Aib.Multiply(g)));
        return this.Aib.Transpose().Multiply(a).Add(this.Abb.Multiply(g)).Negate() as DenseMatrix;
    }
    private DenseMatrix NeumannToDirichlet(DenseMatrix phi, DenseMatrix h)
    {
        var llt = this.A.Cholesky();
        var a = llt.Solve(phi.Append(h.Negate()));
        return a.SubMatrix(this.nI, this.nV, 0, 1) as DenseMatrix;
    }
    private Dictionary<MeshVertex, Point3d> ConstructBestFitCurve(DenseMatrix lstar, DenseMatrix ktilde)
    {
        throw new NotImplementedException();
    }
    private DenseMatrix ExtendHarmonic(DenseMatrix g)
    {
        var llt = this.Aii.Cholesky();
        var a = llt.Solve(this.Aib.Multiply(g).Negate());
        return a.Append(g) as DenseMatrix;
    }
    private Dictionary<MeshVertex, Vector3d> ExtendCurve(DenseMatrix gamma, DenseMatrix gammaRe, DenseMatrix gammaIm, bool extendHolomorphically)
    {
        throw new NotImplementedException();
    }
    private Dictionary<MeshVertex, Vector3d> FlattenWithScaleFactorsAndCurvatures(DenseMatrix u, DenseMatrix ktilde, bool extendHolomorphically, bool rescale)
    {
        throw new NotImplementedException();

    }
    public Dictionary<MeshVertex, Vector3d> Flatten(DenseMatrix target, bool givenScaleFactors, bool rescale)
    {
        throw new NotImplementedException();
    }
    public Dictionary<MeshVertex, Vector3d> flattenToDisk()
    {
        throw new NotImplementedException();
    }
}

