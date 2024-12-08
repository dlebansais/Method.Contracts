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
    /// Checks whether a boolean expression evaluates to <see langword="true"/>.
    /// </summary>
    /// <param name="expression">The expression to check.</param>
    /// <param name="text">The text of the expression for diagnostic purpose.</param>
    public static void Assert(bool expression, [CallerArgumentExpression(nameof(expression))] string? text = default)
#if DEBUG
    => Debug.Assert(expression, $"Assert failed: {text}");
#else
    {
        if (!expression)
            throw new BrokenContractException($"Assert failed: {text}");
    }
#endif
}
