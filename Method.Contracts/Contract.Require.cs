namespace Contracts;

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
    /// Checks whether a requirement in the form of a boolean expression is met.
    /// </summary>
    /// <param name="expression">The expression to check.</param>
    /// <param name="text">The text of the expression for diagnostic purpose.</param>
    public static void Require(bool expression, [CallerArgumentExpression(nameof(expression))] string? text = default)
#if DEBUG
    => Debug.Assert(expression, $"Requirement not met: {text}");
#else
    {
        if (!expression)
            throw new System.ArgumentException($"Requirement not met: {text}");
    }
#endif
}
