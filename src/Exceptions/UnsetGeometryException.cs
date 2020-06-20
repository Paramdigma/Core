using System;

namespace Paramdigma.Core.Exceptions
{
    /// <summary>
    ///     Represents errors that ocur when using a geometry that has the 'isUnset' flag set to true.
    /// </summary>
    public class UnsetGeometryException : Exception
    {
        /// <inheritdoc />
        public UnsetGeometryException() { }

        /// <inheritdoc />
        public UnsetGeometryException(string message) : base(message) { }

        /// <inheritdoc />
        public UnsetGeometryException(string message, Exception innerException) : base(message, innerException) { }
    }
}