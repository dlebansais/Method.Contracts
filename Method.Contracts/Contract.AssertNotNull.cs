﻿namespace Contracts;

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
    /// Returns the provided value after checking that it's not null.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="value">The value that should not be null.</param>
    /// <param name="text">The text of the value for diagnostic purpose.</param>
    /// <param name="lineNumber">The line number where the error occurred for diagnostic purpose.</param>
    /// <returns>The provided value.</returns>
    public static T AssertNotNull<T>(T? value, [CallerArgumentExpression(nameof(value))] string? text = default, [CallerLineNumber] int lineNumber = -1)
        where T : class
    {
        string Message = $"Unexpected null value, line {lineNumber}: {text}";

#if DEBUG
        Debug.Assert(value is not null, Message);
#else
        if (value is null)
            throw new BrokenContractException(Message);
#endif

        return value!;
    }
}
