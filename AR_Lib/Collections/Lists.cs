using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Static class holding some utility methods regarding object collections
/// </summary>
public static class Lists
{
    /// <summary>
    /// Initializes a new list full of objects initialized with their default constructor. 
    /// </summary>
    /// <param name="count">Number of objects in the list.</param>
    /// <typeparam name="T">Type of object in the list.</typeparam>
    /// <returns></returns>
    public static List<T> RepeatedDefault<T>(int count)
    {
        return Repeated(default(T), count);
    }

    /// <summary>
    /// Initializes a new list full of objects initialized with their default constructor. 
    /// </summary>
    /// <param name="value">Object to insert on every index of the list.</param>
    /// <param name="count">Number of objects in the list.</param>
    /// <typeparam name="T">Type of object in the list.</typeparam>
    /// <returns></returns>
    public static List<T> Repeated<T>(T value, int count)
    {
        List<T> repeated = new List<T>(count);
        repeated.AddRange(Enumerable.Repeat(value, count));
        return repeated;
    }
}