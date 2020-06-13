using System.Collections.Generic;

namespace Paramdigma.Core.Optimization
{
    /// <summary>
    ///     Contains the result values of a Gradient Descent minimization.
    /// </summary>
    public struct GradientDescentResult
    {
        /// <summary>
        /// Resulting values after gradient descen minimization.
        /// </summary>
        public List<double> Values;

        /// <summary>
        /// Final gradient descent error.
        /// </summary>
        public double Error;

        /// <summary>
        /// Final length of the gradient.
        /// </summary>
        public double GradientLength;
    }
}