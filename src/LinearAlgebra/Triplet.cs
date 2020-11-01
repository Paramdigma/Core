using System.Collections.Generic;

#pragma warning disable 1591

namespace Paramdigma.Core.LinearAlgebra
{
    /// <summary>
    ///     Represents a set of data in a sparse matrix.
    /// </summary>
    public class Triplet
    {
        // Constructor
        public Triplet(int m, int n)
        {
            this.M = m;
            this.N = n;
            this.Values = new List<TripletData>();
        }
        // Public fields


        /// <summary>
        ///     Gets values held by this triplet.
        /// </summary>
        /// <value></value>
        public List<TripletData> Values { get; }

        public int M { get; }

        public int N { get; }


        // Methods
        public void AddEntry(double value, int m, int n)
        {
            var tD = new TripletData {Value = value, Row = m, Column = n};

            this.Values.Add(tD);
        }
    }

    public struct TripletData
    {
        public int Row { get; set; }

        public int Column { get; set; }

        public double Value { get; set; }
    }
}