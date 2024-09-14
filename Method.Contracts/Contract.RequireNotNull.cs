namespace Contracts;

using System;
using System.Diagnostics;

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
    /// <exception cref="ArgumentNullException"><paramref name="obj"/> is null.</exception>
    public static void RequireNotNull<T>(object? obj, out T result)
        where T : class
    {
#if DEBUG
        T? asT = obj as T;
        Debug.Assert(obj is not null, "Invalid null argument.");
#pragma warning disable CA1508
        Debug.Assert(asT is not null, $"Invalid argument type. Expected {typeof(T)}, got {obj?.GetType()}.");
#pragma warning restore CA1508
#if NET481_OR_GREATER || NETSTANDARD2_0
        result = asT!; // .NET Framework does not detect that Debug.Assert(obj is not null...) means obj is not null.
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
            throw new ArgumentException($"Invalid argument type. Expected {typeof(T)}, got {obj.GetType()}.");

        result = asT;
#endif // #if DEBUG #else
    }

    /// <summary>
    /// Checks that <paramref name="value"/> is not null, and provide an alias that is guaranteed to be non-null.
    /// </summary>
    /// <typeparam name="T">The type of <paramref name="value"/> and returned alias.</typeparam>
    /// <param name="value">The value that should not be null.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is null.</exception>
    /// <returns>The non-null alias.</returns>
    public static T RequireNotNull<T>(T? value)
        where T : class, IDisposable
    {
#if DEBUG
        Debug.Assert(value is not null, "Invalid null argument");
#if NET481_OR_GREATER || NETSTANDARD2_0
        return value!; // .NET Framework does not detect that Debug.Assert(obj is not null...) means obj is not null.
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
