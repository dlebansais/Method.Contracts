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
    /// Checks whether a guarantee in the form of a boolean expression is enforced.
    /// </summary>
    /// <param name="expression">The expression to check.</param>
    /// <param name="text">The text of the expression for diagnostic purpose.</param>
    public static void Ensure(bool expression, [CallerArgumentExpression(nameof(expression))] string? text = default)
#if DEBUG
    => Debug.Assert(expression, $"Postcondition failed: {text}");
#else
    {
        if (!expression)
            throw new BrokenContractException($"Postcondition failed: {text}");
    }
#endif
}
