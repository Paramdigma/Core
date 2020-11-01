using System.Collections.Generic;
using System.Linq;

namespace Paramdigma.Core.Extensions
{
    /// <summary>
    ///     Static class holding some utility methods regarding object collections.
    /// </summary>
    public static class Lists
    {
        /// <summary>
        ///     Initializes a new list full of objects initialized with their default constructor.
        /// </summary>
        /// <param name="count">Number of objects in the list.</param>
        /// <typeparam name="T">Type of object in the list.</typeparam>
        /// <returns>//TODO.</returns>
        public static List<T> RepeatedDefault<T>(int count) => Repeated(default(T), count);


        /// <summary>
        ///     Initializes a new list full of objects initialized the values of the specified instance of T.
        /// </summary>
        /// <param name="value">Object to insert on every index of the list.</param>
        /// <param name="count">Number of objects in the list.</param>
        /// <typeparam name="T">Type of object in the list.</typeparam>
        /// <returns>//TODO.</returns>
        public static List<T> Repeated<T>(T value, int count)
        {
            var repeated = new List<T>(count);
            repeated.AddRange(Enumerable.Repeat(value, count));
            return repeated;
        }
    }
}