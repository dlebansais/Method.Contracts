namespace Contracts;

using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

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
        Debug.Assert(expression, $"Postcondition failed: {text}");
#else
        if (!expression)
            throw new BrokenContractException($"Postcondition failed: {text}");
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
    /// <param name="text">The text of the value for diagnostic purpose.</param>
    /// <returns>The provided value.</returns>
    public static T AssertNotNull<T>(T? value, [CallerArgumentExpression(nameof(value))] string? text = default)
        where T : class
    {
#if DEBUG
        Debug.Assert(value is not null, $"Unexpected null value: {text}");
#else
        if (value is null)
            throw new BrokenContractException($"Unexpected null value: {text}");
#endif

        return value!;
    }

    /// <summary>
    /// Executes an action, checking that no exception was thrown.
    /// </summary>
    /// <param name="action">The action that should not throw exceptions to call.</param>
    /// <param name="text">The text of the action call for diagnostic purpose.</param>
    public static void AssertNoThrow(Action action, [CallerArgumentExpression(nameof(action))] string? text = default)
    {
        RequireNotNull(action, out Action Action);

        try
        {
            Action();
        }
        catch (Exception exception)
        {
#if DEBUG
            Debug.WriteLine(exception.StackTrace);
            Debug.Fail("Unexpected exception", exception.Message);
#else
            throw new BrokenContractException("Unexpected exception", exception);
#endif
        }
    }

    /// <summary>
    /// Executes a function and return its result, checking that no exception was thrown.
    /// </summary>
    /// <typeparam name="T">The result type.</typeparam>
    /// <param name="function">The function that should not throw exceptions to call.</param>
    /// <param name="text">The text of the function call for diagnostic purpose.</param>
    /// <returns>The value returned by <paramref name="function"/>.</returns>
    public static T AssertNoThrow<T>(Func<T> function, [CallerArgumentExpression(nameof(function))] string? text = default)
        where T : class
    {
        RequireNotNull(function, out Func<T> Function);

        try
        {
            return Function();
        }
        catch (Exception exception)
        {
#if DEBUG
            Debug.WriteLine(exception.StackTrace);
            Debug.Fail("Unexpected exception", exception.Message);
            return default!;
#else
            throw new BrokenContractException("Unexpected exception", exception);
#endif
        }
    }
}
