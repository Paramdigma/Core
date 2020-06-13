using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Paramdigma.Core.Geometry
{
    /// <summary>
    /// Multidimensional vector.
    /// </summary>
    public class VectorNd : IEnumerable<double>, IEquatable<VectorNd>
    {
        private List<double> values;

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorNd"/> class.
        /// </summary>
        /// <param name="vector">Vector to copy.</param>
        public VectorNd(VectorNd vector)
        {
            this.values = new List<double>(vector);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorNd"/> class.
        /// Constructs a zero vector of a given dimension.
        /// </summary>
        /// <param name="dimension">Dimension.</param>
        public VectorNd(int dimension)
        {
            InitializeZeroVector(dimension);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorNd"/> class  from a given list of coordinates.
        /// </summary>
        /// <param name="values">List of values for the vector.</param>
        public VectorNd(List<double> values)
        {
            this.values = values;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VectorNd"/> class from a given list of parameters.
        /// </summary>
        /// <param name="values">Number parameters.</param>
        public VectorNd(params double[] values) : this(values.ToList())
        {
        }

        /// <summary>
        /// Gets or sets the coordinate at the given dimension.
        /// </summary>
        /// <value>The value of the specified dimension.</value>
        public double this[int dimension] { get => values[dimension]; set => values[dimension] = value; }

        /// <summary>
        /// Gets the current dimension of the vector.
        /// </summary>
        public int Dimension => values.Count;

        /// <summary>
        /// Gets the length of the vector.
        /// </summary>
        public double Length => Math.Sqrt(Length2);

        /// <summary>
        /// Gets the squared length of the vector.
        /// </summary>
        public double Length2 => ComputeLength2();

        /// <summary>
        /// Gets the dot product of this vector with another.
        /// </summary>
        /// <param name="vector">Vector to compute dot product with.</param>
        /// <returns>Dot product result.</returns>
        public double Dot(VectorNd vector) => DotProduct(this, vector);

        private double ComputeLength2()
        {
            var length = .0;
            values.ForEach(value => length += value * value);
            return length;
        }

        private void InitializeZeroVector(int dimension)
        {
            values = new List<double>(dimension);
            for (int i = 0; i < dimension; i++)
                values.Add(.0);
        }

        /// <inheritdoc/>
        public IEnumerator<double> GetEnumerator()
        {
            return values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }

        /// <summary>
        /// Computes the distance between two N-dimensional vectors.
        /// </summary>
        /// <param name="a">Vector A.</param>
        /// <param name="b">Vector B.</param>
        /// <returns>Distance between vectors.</returns>
        public static double Distance(VectorNd a, VectorNd b) => Math.Sqrt(Distance2(a, b));

        /// <summary>
        /// Computes the square distance between two N-dimensional vectors.
        /// </summary>
        /// <param name="a">Vector A.</param>
        /// <param name="b">Vector B.</param>
        /// <returns>Squared distance.</returns>
        public static double Distance2(VectorNd a, VectorNd b)
        {
            var dist = .0;
            for (int i = 0; i < Math.Max(a.Dimension, b.Dimension); i++)
            {
                var ai = i < a.Dimension ? a[i] : 0;
                var bi = i < b.Dimension ? b[i] : 0;
                dist += (bi - ai) * (bi - ai);
            }

            return dist;
        }

        /// <summary>
        /// Computes the cosine similarity between two vectors.
        /// </summary>
        /// <param name="a">Vector A.</param>
        /// <param name="b">Vector B.</param>
        /// <returns>Cosine similarity value.</returns>
        public static double CosineSimilarity(VectorNd a, VectorNd b) => a.Dot(b) / (a.Length * b.Length);

        /// <summary>
        /// Computes the angular distance between two vectors.
        /// </summary>
        /// <param name="a">Vector A.</param>
        /// <param name="b">Vector B.</param>
        /// <returns>Angular distance value.</returns>
        public static double AngularDistance(VectorNd a, VectorNd b) => Math.Acos(CosineSimilarity(a, b)) / Math.PI;

        /// <summary>
        /// Angular similarity value between two vectors.
        /// </summary>
        /// <param name="a">Vector A.</param>
        /// <param name="b">Vector B.</param>
        /// <returns>Angular similarity value.</returns>
        public static double AngularSimilarity(VectorNd a, VectorNd b) => 1 - AngularDistance(a, b);

        /// <summary>
        /// Adds two N dimensional vectors.
        /// </summary>
        /// <param name="a">Vector A.</param>
        /// <param name="b">Vector B.</param>
        /// <returns>Vector addition result.</returns>
        public static VectorNd Add(VectorNd a, VectorNd b)
        {
            var max = Math.Max(a.Dimension, b.Dimension);
            var result = new VectorNd(max);
            for (int index = 0; index < max; index++)
            {
                var iA = index < a.Dimension ? a[index] : 0;
                var iB = index < b.Dimension ? b[index] : 0;
                result[index] = iA + iB;
            }

            return result;
        }

        /// <summary>
        /// Subtract two N dimensional vectors.
        /// </summary>
        /// <param name="a">Vector A.</param>
        /// <param name="b">Vector B.</param>
        /// <returns>Vector substraction result.</returns>
        public static VectorNd Substract(VectorNd a, VectorNd b)
        {
            return Add(a, Negate(b));
        }

        /// <summary>
        /// Multiply an N dimensional vector by a number.
        /// </summary>
        /// <param name="vector">Vector to multiply.</param>
        /// <param name="scalar">Number to multiply by.</param>
        /// <returns>Vector multiplication result.</returns>
        public static VectorNd Multiply(VectorNd vector, double scalar)
        {
            var result = new VectorNd(vector.Dimension);
            for (var i = 0; i < vector.Dimension; i++)
                result[i] = vector[i] * scalar;
            return result;
        }

        /// <summary>
        /// Divide an N dimensional vector by a number.
        /// </summary>
        /// <param name="vector">Vector to divide.</param>
        /// <param name="scalar">Number to divide by.</param>
        /// <returns>Vector division result.</returns>
        public static VectorNd Divide(VectorNd vector, double scalar)
        {
            return Multiply(vector, 1 / scalar);
        }

        /// <summary>
        /// Negate a given vector.
        /// </summary>
        /// <param name="vector">Vector to negate.</param>
        /// <returns>Vector negation result.</returns>
        public static VectorNd Negate(VectorNd vector)
        {
            var result = new VectorNd(vector.Dimension);
            for (var i = 0; i < vector.Dimension; i++)
                result[i] = -vector[i];
            return result;
        }

        /// <summary>
        /// Computes the dot product between two vectors.
        /// </summary>
        /// <param name="vectorA">Vector A.</param>
        /// <param name="vectorB">Vector B.</param>
        /// <returns></returns>
        public static double DotProduct(VectorNd vectorA, VectorNd vectorB)
        {
            var max = Math.Max(vectorA.Dimension, vectorB.Dimension);
            var result = .0;
            for (int index = 0; index < max; index++)
            {
                var iA = index < vectorA.Dimension ? vectorA[index] : 0;
                var iB = index < vectorB.Dimension ? vectorB[index] : 0;
                result += iA * iB;
            }

            return result;
        }

        /// <inheritdoc/>
        public static VectorNd operator +(VectorNd vectorA, VectorNd vectorB) => Add(vectorA, vectorB);

        /// <inheritdoc/>
        public static VectorNd operator -(VectorNd vectorA, VectorNd vectorB) => Substract(vectorA, vectorB);

        /// <inheritdoc/>
        public static VectorNd operator *(VectorNd vector, double scalar) => Multiply(vector, scalar);

        /// <inheritdoc/>
        public static VectorNd operator /(VectorNd vector, double scalar) => Divide(vector, scalar);

        /// <summary>
        /// Compare this vector with another.
        /// </summary>
        /// <param name="vector">Vector to compare with.</param>
        /// <returns>Comparison result.</returns>
        public bool Equals(VectorNd vector)
        {
            var max = Math.Max(this.Dimension, vector.Dimension);
            for (int index = 0; index < max; index++)
            {
                var iA = index < this.Dimension ? this[index] : 0;
                var iB = index < vector.Dimension ? vector[index] : 0;
                var comp = Math.Abs(iA - iB) < Settings.Tolerance;
                if (!comp)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Compare this vector with another object.
        /// </summary>
        /// <param name="obj">Object to compare with.</param>
        /// <returns>Comparison result.</returns>
        public override bool Equals(object obj)
        {
            if (obj is VectorNd)
            {
                return Equals(obj as VectorNd);
            }

            return false;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                // Choose large primes to avoid hashing collisions
                const int hashingBase = (int)2166136261;
                const int hashingMultiplier = 16777619;
                double tol = Settings.Tolerance * 2;
                int hash = hashingBase;

                foreach (var coord in values)
                {
                    double tCoord = (int)(coord * (1 / tol)) * tol; // Round to tolerance
                    hash = (hash * hashingMultiplier) ^ tCoord.GetHashCode();
                }

                return hash;
            }
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return "VectorNd[" + this[0] + "," + this[1] + "," + this[2] + ",...]";
        }
    }
}