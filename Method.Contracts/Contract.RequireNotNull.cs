namespace Contracts;

using System;
#if DEBUG
using System.Diagnostics;
#endif
using System.Runtime.CompilerServices;

/// <summary>
/// A set of tools to enforce contracts in methods.
/// </summary>
public static partial class Contract
{
    /// <summary>
    /// Checks that <paramref name="obj"/> is not null, and provide an alias that is guaranteed to be non-null.
    /// </summary>
    /// <typeparam name="T">The type of <paramref name="obj"/> and <paramref name="result"/>.</typeparam>
    /// <param name="obj">The object instance to check.</param>
    /// <param name="result">The non-null alias upon return.</param>
    /// <param name="text">The text of the expression for diagnostic purpose.</param>
    /// <param name="lineNumber">The line number where the error occurred for diagnostic purpose.</param>
    /// <exception cref="ArgumentNullException"><paramref name="obj"/> is null.</exception>
    public static void RequireNotNull<T>(object? obj, out T result, [CallerArgumentExpression(nameof(obj))] string? text = default, [CallerLineNumber] int lineNumber = -1)
        where T : class
    {
#if DEBUG
        AssertNotNull(obj, text, lineNumber);

        if (obj is null)
        {
            // ! AssertNotNull(obj, ...) enforced that 'obj' is not null.
            result = default!;
            return;
        }

        T? asT = obj as T;

#pragma warning disable CA1508
        Debug.Assert(asT is not null, $"Invalid argument type, expected '{typeof(T)}', got '{obj.GetType()}', line {lineNumber}");
#pragma warning restore CA1508

#if NET481_OR_GREATER || NETSTANDARD2_0
        // ! Debug.Assert(asT is not null, ...) enforced that 'asT' is not null but .NET Framework and .NET Standard 2.0 don't detect it.
        result = asT!;
#else
        result = asT;
#endif
#else // #if DEBUG
#pragma warning disable IDE0019
        T? asT = obj as T;
#pragma warning restore IDE0019
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(obj);
#else
        if (obj is null)
            throw new ArgumentNullException(nameof(obj));
#endif
        if (asT is null)
            throw new ArgumentException($"Invalid argument type, xpected {typeof(T)}, got {obj.GetType()}, line {lineNumber}");

        result = asT;
#endif // #if DEBUG #else
    }

    /// <summary>
    /// Checks that <paramref name="value"/> is not null, and provide an alias that is guaranteed to be non-null.
    /// </summary>
    /// <typeparam name="T">The type of <paramref name="value"/> and returned alias.</typeparam>
    /// <param name="value">The value that should not be null.</param>
    /// <param name="text">The text of the expression for diagnostic purpose.</param>
    /// <param name="lineNumber">The line number where the error occurred for diagnostic purpose.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
    /// <returns>The non-null alias.</returns>
    public static T RequireNotNull<T>(T? value, [CallerArgumentExpression(nameof(value))] string? text = default, [CallerLineNumber] int lineNumber = -1)
        where T : class, IDisposable
    {
#if DEBUG
        Debug.Assert(value is not null, $"Invalid null argument '{text}', line {lineNumber}");

#if NET481_OR_GREATER || NETSTANDARD2_0
        // ! Debug.Assert(value is not null, ...) enforced that 'value' is not null but .NET Framework and .NET Standard 2.0 don't detect it.
        return value!;
#else
        return value;
#endif
#else // #if DEBUG
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(value);
#else
        if (value is null)
            throw new ArgumentNullException(nameof(value));
#endif
        return value;
#endif // #if DEBUG #else
    }
}
