namespace Contracts;

using System.Diagnostics;
using System.Runtime.CompilerServices;

/// <summary>
/// A set of tools to enforce contracts in methods.
/// </summary>
public static partial class Contract
{
    /// <summary>
    /// Returns the provided value after checking that it's not null and of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="value">The value that should be of the expected type.</param>
    /// <param name="text">The text of the value for diagnostic purpose.</param>
    /// <returns>The provided value.</returns>
    public static T AssertOfType<T>(object? value, [CallerArgumentExpression(nameof(value))] string? text = default)
        where T : class
    {
#if DEBUG
        T? Result = value as T;
        Debug.Assert(Result is not null, $"Expected type '{typeof(T).Name}' for value: {text}");
#else
        if (value is not T Result)
            throw new BrokenContractException($"Expected type '{typeof(T).Name}' for value: {text}");
#endif

        return Result!;
    }
}
