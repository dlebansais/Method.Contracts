namespace Contracts;

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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
#if DEBUG
        Debug.Assert(obj is not null, "Invalid null reference");

#if NET481_OR_GREATER
        result = (T)obj!; // .NET Framework does not detect that Debug.Assert(obj is not null...) means obj is not null.
#else
        result = (T)obj;
#endif

#else

#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(obj);
#else
        if (obj is null)
            throw new ArgumentNullException(nameof(obj));
#endif

        result = (T)obj;
#endif
    }

    /// <summary>
    /// Checks whether a requirement in the form of a boolean expression is met.
    /// </summary>
    /// <param name="expression">The expression to check.</param>
    /// <param name="text">The text of the expression for diagnostic purpose.</param>
    public static void Require(bool expression, [CallerArgumentExpression(nameof(expression))]string? text = default)
    {
#if DEBUG
        Debug.Assert(expression, $"Requirement not met: {text}");
#else
        if (!expression)
            throw new ArgumentException($"Requirement not met: {text}");
#endif
    }

    /// <summary>
    /// Checks whether a guarantee in the form of a boolean expression is enforced.
    /// </summary>
    /// <param name="expression">The expression to check.</param>
    /// <param name="text">The text of the expression for diagnostic purpose.</param>
    public static void Ensure(bool expression, [CallerArgumentExpression(nameof(expression))] string? text = default)
    {
#if DEBUG
        Debug.Assert(expression, $"Guarantee not enforced: {text}");
#else
        if (!expression)
            throw new InvalidOperationException($"Guarantee not enforced: {text}");
#endif
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
        Debug.Assert(value is not null, "Unexpected null reference");

        return value!;
    }
}
