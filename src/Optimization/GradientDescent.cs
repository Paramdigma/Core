using System;
using System.Collections.Generic;

namespace Paramdigma.Core.Optimization
{
    /// <summary>
    ///     Gradient descent algorithm class.
    /// </summary>
    public class GradientDescent
    {
        /// <summary>
        ///     Delegate property in charge of computing the fitness error.
        /// </summary>
        /// <param name="inputValues">Input values to comute the fitness from.</param>
        /// <returns>Scalar value representing the current fitness for the given values.</returns>
        public delegate double FitnessFunction(List<double> inputValues);

        /// <summary>
        ///     Contains all the options of the current minimization.
        /// </summary>
        public GradientDescentOptions Options;

        // INFO: Algorithm seems to be working fine, although it is prone to get stuck in multidimensional minimizations if the initial values aren't already close to the global minimum
        // FIXME: Add option to choose between computing the 2 point partial derivative or the 5-point.
        // FIXME: Add option to compute the gradient using a delegate method (if you want to compute gradient's analytically).

        /// <summary>
        ///     Contains the result of the current minimization.
        /// </summary>
        public GradientDescentResult Result;

        /// <summary>
        ///     Run gradient descent algorithm.
        /// </summary>
        /// <param name="function">Delegate function to compute the fitness.</param>
        /// <param name="inputValues">Input values to compute the fitness from.</param>
        public void Minimize(FitnessFunction function, List<double> inputValues)
        {
            Result.Values = inputValues;

            // Run minimization at least once
            var iter = 0;
            double gLength;
            do
            {
                List<double> gradient = ComputeGradient(function, Result.Values);

                // Compute gradient length
                gLength = 0;
                gradient.ForEach(g => gLength += g * g);
                gLength = Math.Sqrt(gLength);

                // Update values
                for (var i = 0; i < Result.Values.Count; i++)
                    Result.Values[i] -= gradient[i];
                Result.Error = function(Result.Values);
                Result.GradientLength = gLength;
                iter++; // Increase iteration count
            }
            while (gLength > Options.Limit && iter < Options.MaxIterations &&
                     Result.Error > Options.ErrorThreshold);
            string customError = Result.Error > 1E5 ? $"{Result.Error:0.###e-000}" : $"{Result.Error:0.00000}";
            Console.ResetColor();
        }

        /// <summary>
        ///     Computes the gradient vector of a given function at some specified input values.
        /// </summary>
        /// <param name="func">Function to compute the fitness error.</param>
        /// <param name="inputValues">Input values to compute the fitness from.</param>
        /// <returns>List of all partial derivatives at the specified input values.</returns>
        private List<double> ComputeGradient(FitnessFunction func, List<double> inputValues)
        {
            var gradient = new List<double>();

            // Obtain partial derivatives of all inputs and their added squares.
            double derivativeSquareSum = 0;

            for (var i = 0; i < inputValues.Count; i++)
            {
                double dV = ComputePartialDerivative(i, func, inputValues, Options.DerivativeStep);
                gradient.Add(dV * Options.LearningRate);
                derivativeSquareSum += dV * dV;
            }

            // Root of the sum of squares is the length
            double gradientLength = Math.Sqrt(derivativeSquareSum);

            // gradientLength = Math.Sqrt(gradientLength);
            // Divide the gradient values by the gradient lenght
            gradient.ForEach(value => value /= gradientLength);

            return gradient;
        }

        /// <summary>
        ///     Computes the partial derivative at a given input value of a given function.
        /// </summary>
        /// <param name="inputIndex">Index of the value from the inputValues list.</param>
        /// <param name="func">Function that will return the fitness.</param>
        /// <param name="inputValues">Input values to compute the fitness from.</param>
        /// <param name="step">Size of the step used to compute the partial derivative.</param>
        /// <returns>Scalar value representing the partial derivative of that particular input.</returns>
        private double ComputePartialDerivative(
            int inputIndex,
            FitnessFunction func,
            List<double> inputValues,
            double step)
        {
            double partialDerivative, error1, error2;

            // Compute 2-point errors
            inputValues[inputIndex] += step;
            error1 = func(inputValues);
            inputValues[inputIndex] -= 2 * step;
            error2 = func(inputValues);
            inputValues[inputIndex] += step; // Reset value to original

            // Compute partial derivative using 2-point method
            partialDerivative = (error1 - error2) / 2 * step;

            return partialDerivative;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GradientDescent"/> class with given options.
        /// </summary>
        /// <param name="options">Settings to apply to this GD instance.</param>
        public GradientDescent(GradientDescentOptions options)
        {
            Result = default;
            Options = options;
        }
    }
}