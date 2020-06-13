namespace Paramdigma.Core.Optimization
{
    /// <summary>
    ///     Contains the different options of a Gradient Descent minimization.
    /// </summary>
    public struct GradientDescentOptions
    {
        /// <summary>
        /// Threshold to stop minimization.
        /// </summary>
        public double Limit;

        /// <summary>
        /// Maximum iterations for the gradient descent.
        /// </summary>
        public int MaxIterations;

        /// <summary>
        /// Step size to compute partial derivatives.
        /// </summary>
        public double DerivativeStep;

        /// <summary>
        /// Scaling factor for the gradient. Effectively speeds up or down the minimization.
        /// </summary>
        public double LearningRate;

        /// <summary>
        /// Minimum error to consider the results acceptable.
        /// </summary>
        public double ErrorThreshold;

        /// <summary>
        /// Initializes a new instance of the <see cref="GradientDescentOptions"/> struct given an existing one.
        /// </summary>
        /// <param name="options">Options to duplicate.</param>
        public GradientDescentOptions(GradientDescentOptions options)
        {
            Limit = options.Limit;
            MaxIterations = options.MaxIterations;
            DerivativeStep = options.DerivativeStep;
            LearningRate = options.LearningRate;
            ErrorThreshold = options.ErrorThreshold;
        }

        // TODO: Fill in this fields!

        /// <summary>
        /// Initializes a new instance of the <see cref="GradientDescentOptions"/> struct given all it's values individually.
        /// </summary>
        /// <param name="threshold"></param>
        /// <param name="maxIterations"></param>
        /// <param name="derivativeStep"></param>
        /// <param name="learningRate"></param>
        /// <param name="errorThreshold"></param>
        public GradientDescentOptions(double threshold, int maxIterations, double derivativeStep, double learningRate, double errorThreshold)
        {
            Limit = threshold;
            MaxIterations = maxIterations;
            DerivativeStep = derivativeStep;
            LearningRate = learningRate;
            ErrorThreshold = errorThreshold;
        }

        /// <summary>
        /// Gets a GradientDescentOptions instance with the default values.
        /// </summary>
        public static GradientDescentOptions Default => new GradientDescentOptions(0.001, 10000, 0.01, 20, .01);

        /// <summary>
        /// Gets a GradientDescentOptions instance with small values.
        /// </summary>
        /// <returns></returns>
        public static GradientDescentOptions DefaultSmall => new GradientDescentOptions(0.0001, 10000, 0.02, 40, .001);
    }
}