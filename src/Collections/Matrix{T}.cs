using System;
using System.Collections.Generic;

namespace Paramdigma.Core.Collections
{
    /// <summary>
    ///     2-Dimensional generic matrix.
    /// </summary>
    /// <typeparam name="T">Type of the objects in the matrix.</typeparam>
    public class Matrix<T>
    {
        // Matrix Class
        // This class was obtained from:
        // https://codereview.stackexchange.com/questions/194732/class-matrix-implementation
        private T[,] data;


        /// <summary>
        ///     Initializes a new instance of the <see cref="Matrix{T}" /> class.
        /// </summary>
        /// <param name="n">Size of the square Matrix.</param>
        public Matrix(int n) => this.data = new T[n, n];


        /// <summary>
        ///     Initializes a new instance of the <see cref="Matrix{T}" /> class of the specified size.
        /// </summary>
        /// <param name="n">Column size.</param>
        /// <param name="m">Row size.</param>
        public Matrix(int n, int m) => this.data = new T[n, m];


        /// <summary>
        ///     Initializes a new instance of the <see cref="Matrix{T}" /> class from a 2D array.
        /// </summary>
        /// <param name="data">2D array of data.</param>
        public Matrix(T[,] data) => this.data = data;


        /// <summary>
        ///     Gets columns.
        /// </summary>
        /// <returns>Number of columns on the Matrix.</returns>
        public int N => this.data.GetLength(0);

        /// <summary>
        ///     Gets rows.
        /// </summary>
        /// <returns>Number of rows on the Matrix.</returns>
        public int M => this.data.GetLength(1);

        /// <summary>
        ///     Gets a specific item in the matrix.
        /// </summary>
        /// <param name="row">Row.</param>
        /// <param name="column">Column.</param>
        public ref T this[int row, int column] => ref this.data[row, column];

        /// <summary>
        ///     Gets the amount of items in this matrix.
        /// </summary>
        public int Count => this.data.Length;


        /// <summary>
        ///     Get the row of a matrix at the specified index.
        /// </summary>
        /// <param name="n">Row index.</param>
        /// <returns>Matrix row as list.</returns>
        public T[] Row(int n)
        {
            var row = new T[this.N];
            for (var i = 0; i < this.M; i++)
                row[i] = this.data[n, i];

            return row;
        }


        /// <summary>
        ///     Get the column of a matrix at the specified index.
        /// </summary>
        /// <param name="m">Column index.</param>
        /// <returns>Matrix column as list.</returns>
        public T[] Column(int m)
        {
            var col = new T[this.M];
            for (var i = 0; i < this.N; i++)
                col[i] = this.data[i, m];

            return col;
        }


        // ----- ORDERING METHODS -----


        /// <summary>
        ///     Turns columns into rows and rows into columns.
        /// </summary>
        public void FlipMatrix() => throw
                                        // TODO: Implement FlipMatrix()
                                        new NotImplementedException();


        /// <summary>
        ///     Increment Matrix column size by a specified amount.
        ///     It accepts both increasing and decreasing the size.
        /// </summary>
        /// <param name="incrementN">Positive or negative increment.</param>
        public void IncrementColumns(int incrementN) => this.ResizeMatrix(
            ref this.data,
            this.N + incrementN,
            this.M);


        /// <summary>
        ///     Increment Matrix row size by a specified amount.
        ///     It accepts both increasing and decreasing the size.
        /// </summary>
        /// <param name="incrementM">Positive or negative increment.</param>
        public void IncrementRows(int incrementM) => this.ResizeMatrix(
            ref this.data,
            this.N,
            this.M + incrementM);


        /// <summary>
        ///     Increase or decrease the matrix size symetrically.
        /// </summary>
        /// <param name="symetricIncrement">Symetric increase/decrease.</param>
        public void IncrementMatrixSize(int symetricIncrement) =>
            this.IncrementMatrixSize(symetricIncrement, symetricIncrement);


        /// <summary>
        ///     Increase or decrease the column size of the matrix.
        /// </summary>
        /// <param name="columnIncrement">Column increment.</param>
        /// <param name="rowIncrement">Row increment.</param>
        public void IncrementMatrixSize(int columnIncrement, int rowIncrement)
        {
            this.IncrementColumns(columnIncrement);
            this.IncrementRows(rowIncrement);
        }


        /// <summary>
        ///     Obtains all neighbour entities surrounding the specified matrix coordinates.
        /// </summary>
        /// <param name="column">Column location.</param>
        /// <param name="row">Row location.</param>
        /// <returns>List of all neighbour entities.</returns>
        public List<T> GetAllNeighboursAt(int column, int row)
        {
            // HACK: This is a hacked up implementation. It provides the neighbours out of order (first contiguous, then corners)
            var neighbours = this.GetContiguousNeighboursAt(column, row);
            neighbours.AddRange(this.GetCornerNeighboursAt(column, row));

            return neighbours;
        }


        /// <summary>
        ///     Obtains corner neighbour entities surrounding the specified matrix coordinates.
        /// </summary>
        /// <param name="column">Column location.</param>
        /// <param name="row">Row location.</param>
        /// <returns>List of corner neighbours (Diagonally connected).</returns>
        public List<T> GetCornerNeighboursAt(int column, int row) => throw
            // TODO: Implement GetCornerNeighboursOfEntityAt()
            new NotImplementedException();


        /// <summary>
        ///     Obtains contiguous neighbour entities surrounding the specified matrix coordinates.
        /// </summary>
        /// <param name="column">Column location.</param>
        /// <param name="row">Row location.</param>
        /// <returns>List of contiguous neighbours ( Up / Left / Down / Right ).</returns>
        public List<T> GetContiguousNeighboursAt(int column, int row) => throw
            // TODO: Implement GetContiguousNeighboursOfEntityAt()
            new NotImplementedException();


        /// <summary>
        ///     Resizes any given 2 dimensional array.
        ///     It accepts smaller and bigger array outputs.
        ///     Obtained from:
        ///     https://stackoverflow.com/questions/6539571/how-to-resize-multidimensional-2d-array-in-c .
        /// </summary>
        /// <param name="original">2D Array to resize.</param>
        /// <param name="newCoNum">Number of resulting columns in the array.</param>
        /// <param name="newRoNum">Number of resulting rows in the array.</param>
        private void ResizeMatrix(ref T[,] original, int newCoNum, int newRoNum)
        {
            var newArray = new T[newCoNum, newRoNum];
            var columnCount = original.GetLength(1);
            var columnCount2 = newRoNum;
            var columns = original.GetUpperBound(0);
            for (var co = 0; co <= columns; co++)
                Array.Copy(original, co * columnCount, newArray, co * columnCount2, columnCount);
            original = newArray;
        }
    }
}