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
    /// Returns the provided value after checking that it's not null and of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="value">The value that should be of the expected type.</param>
    /// <param name="text">The text of the value for diagnostic purpose.</param>
    /// <param name="lineNumber">The line number where the error occurred for diagnostic purpose.</param>
    /// <returns>The provided value.</returns>
    public static T AssertOfType<T>(object? value, [CallerArgumentExpression(nameof(value))] string? text = default, [CallerLineNumber] int lineNumber = -1)
        where T : class
    {
        AssertNotNull(value, text, lineNumber);

#if DEBUG
        if (value is null)
        {
            // ! AssertNotNull(value, ...) enforced that 'value' is not null.
            return default!;
        }
#endif

        string Message = $"Expected type '{typeof(T)}' for value: {text}, line {lineNumber}";

#if DEBUG
        T? Result = value as T;

        Debug.Assert(Result is not null, Message);

#if NET481_OR_GREATER || NETSTANDARD2_0
        // ! Debug.Assert(Result is not null, ...) enforced that 'Result' is not null but .NET Framework and .NET Standard 2.0 don't detect it.
        return Result!;
#else
        return Result;
#endif
#else

        return value is T Result ? Result : throw new BrokenContractException(Message);
#endif
    }
}
