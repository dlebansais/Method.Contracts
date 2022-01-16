namespace Contracts;

using System;
using System.Diagnostics;

/// <summary>
/// A set of tools to enforce contracts in methods.
/// </summary>
public static class Contract
{
    /// <summary>
    /// Checks that <paramref name="obj"/> is not null, and provide an alias that is guaranteed to be non-null.
    /// </summary>
    /// <typeparam name="T">The type of <paramref name="obj"/> and <paramref name="result"/>.</typeparam>
    /// <param name="obj">The object instance to check.</param>
    /// <param name="result">The non-null alias upon return.</param>
    /// <exception cref="ArgumentNullException"><paramref name="obj"/> is null.</exception>
    public static void RequireNotNull<T>(object? obj, out T result)
        where T : class
    {
        Debug.Assert(obj is not null, "Invalid null reference");

        if (obj is null)
            throw new ArgumentNullException(nameof(obj));

        result = (T)obj;
    }

    /// <summary>
    /// Provides a value for variables that should be uninitialized.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="result">The unused value.</param>
    public static void Unused<T>(out T result)
        where T : class
    {
        result = default!;
    }

    /// <summary>
    /// Returns the provided value after checking that it's not null.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="value">The value that should not be null.</param>
    /// <returns>The provided value.</returns>
    public static T NullSupressed<T>(T? value)
        where T : class
    {
        Debug.Assert(value is not null);

        return value!;
    }
}
