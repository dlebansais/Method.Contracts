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
}
