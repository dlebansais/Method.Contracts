namespace Contracts;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;

/// <summary>
/// A set of tools to enforce contracts in methods.
/// </summary>
public static partial class Contract
{
    /// <summary>
    /// Performs the equivalent of a switch expression when the conversion is already computed by the caller.
    /// Checks whether all enum values are in the provided dictionary.
    /// </summary>
    /// <typeparam name="TEnumKey">The enum key type.</typeparam>
    /// <typeparam name="TValue">The returned type.</typeparam>
    /// <param name="expression">The expression to map.</param>
    /// <param name="dictionary">The dictionary of keys and values.</param>
    /// <param name="expressionText">The text of the expression for diagnostic purpose.</param>
    /// <param name="dictionaryText">The text of the dictionary for diagnostic purpose.</param>
    public static TValue Map<TEnumKey, TValue>(TEnumKey expression,
                                               Dictionary<TEnumKey, TValue> dictionary,
                                               [CallerArgumentExpression(nameof(expression))] string? expressionText = default,
                                               [CallerArgumentExpression(nameof(dictionary))] string? dictionaryText = default)
        where TEnumKey : struct, Enum
    {
#if DEBUG
        Debug.Assert(dictionary is not null, $"Invalid null dictionary: {dictionaryText}");
        bool IsValid = IsValidDictionary(dictionary);
        Debug.Assert(IsValid, $"Invalid dictionary: {dictionaryText}");

#pragma warning disable CA1508 // Avoid dead conditional code: when unit testing, this code is not dead.
        if (dictionary is null || !IsValid)
            return default!;
#pragma warning restore CA1508 // Avoid dead conditional code
#if NET481_OR_GREATER || NETSTANDARD2_0
        Dictionary<TEnumKey, TValue> Dictionary = dictionary!; // .NET Framework does not detect that Debug.Assert(obj is not null...) means obj is not null.
#else
        Dictionary<TEnumKey, TValue> Dictionary = dictionary;
#endif
#else // #if DEBUG
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(dictionary, $"Invalid null dictionary: {dictionaryText}");
#else
        if (dictionary is null)
            throw new ArgumentNullException(nameof(dictionary), $"Invalid null dictionary: {dictionaryText}");
#endif
        if (!IsValidDictionary(dictionary))
            throw new BrokenContractException($"Invalid dictionary: {dictionaryText}");

        Dictionary<TEnumKey, TValue> Dictionary = dictionary;
#endif // #if DEBUG #else

        if (Dictionary.TryGetValue(expression, out TValue? Value))
            return Value;

        object IntValue = Convert.ChangeType(expression, expression.GetTypeCode(), CultureInfo.InvariantCulture);
        string FailureText = $"Enum '{expressionText}' with value {IntValue} not in dictionary.";
#if DEBUG
        Debug.Assert(false, FailureText);
#else // #if DEBUG
        throw new BrokenContractException(FailureText);
#endif // #if DEBUG #else

        return default!;
    }

    /// <summary>
    /// Performs the equivalent of a switch expression when the conversion is not already computed by the caller.
    /// Checks whether all enum values are in the provided dictionary.
    /// </summary>
    /// <typeparam name="TEnumKey">The enum key type.</typeparam>
    /// <typeparam name="TValue">The returned type.</typeparam>
    /// <param name="expression">The expression to map.</param>
    /// <param name="dictionary">The dictionary of keys and functions.</param>
    /// <param name="expressionText">The text of the expression for diagnostic purpose.</param>
    /// <param name="dictionaryText">The text of the dictionary for diagnostic purpose.</param>
    public static TValue Map<TEnumKey, TValue>(TEnumKey expression,
                                               Dictionary<TEnumKey, Func<TValue>> dictionary,
                                               [CallerArgumentExpression(nameof(expression))] string? expressionText = default,
                                               [CallerArgumentExpression(nameof(dictionary))] string? dictionaryText = default)
        where TEnumKey : struct, Enum
    {
#if DEBUG
        Debug.Assert(dictionary is not null, $"Invalid null dictionary: {dictionaryText}");
        bool IsValid = IsValidDictionary(dictionary);
        Debug.Assert(IsValid, $"Invalid dictionary: {dictionaryText}");

#pragma warning disable CA1508 // Avoid dead conditional code: when unit testing, this code is not dead.
        if (dictionary is null || !IsValid)
            return default!;
#pragma warning restore CA1508 // Avoid dead conditional code
#if NET481_OR_GREATER || NETSTANDARD2_0
        Dictionary<TEnumKey, Func<TValue>> Dictionary = dictionary!; // .NET Framework does not detect that Debug.Assert(obj is not null...) means obj is not null.
#else
        Dictionary<TEnumKey, Func<TValue>> Dictionary = dictionary;
#endif
#else // #if DEBUG
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(dictionary, $"Invalid null dictionary: {dictionaryText}");
#else
        if (dictionary is null)
            throw new ArgumentNullException(nameof(dictionary), $"Invalid null dictionary: {dictionaryText}");
#endif
        if (!IsValidDictionary(dictionary))
            throw new BrokenContractException($"Invalid dictionary: {dictionaryText}");

        Dictionary<TEnumKey, TValue> Dictionary = dictionary;
#endif // #if DEBUG #else

        if (Dictionary.TryGetValue(expression, out Func<TValue>? FuncValue))
            return FuncValue();

        object IntValue = Convert.ChangeType(expression, expression.GetTypeCode(), CultureInfo.InvariantCulture);
        string FailureText = $"Enum '{expressionText}' with value {IntValue} not in dictionary.";
#if DEBUG
        Debug.Assert(false, FailureText);
#else // #if DEBUG
        throw new BrokenContractException(FailureText);
#endif // #if DEBUG #else

        return default!;
    }

    /// <summary>
    /// Performs the equivalent of a switch instruction and only call one of the provided actions.
    /// Checks whether all enum values are in the provided dictionary.
    /// </summary>
    /// <typeparam name="TEnumKey">The enum key type.</typeparam>
    /// <param name="expression">The expression to map.</param>
    /// <param name="dictionary">The dictionary of keys and actions.</param>
    /// <param name="expressionText">The text of the expression for diagnostic purpose.</param>
    /// <param name="dictionaryText">The text of the dictionary for diagnostic purpose.</param>
    public static void Map<TEnumKey>(TEnumKey expression,
                                     Dictionary<TEnumKey, Action> dictionary,
                                     [CallerArgumentExpression(nameof(expression))] string? expressionText = default,
                                     [CallerArgumentExpression(nameof(dictionary))] string? dictionaryText = default)
        where TEnumKey : struct, Enum
    {
#if DEBUG
        Debug.Assert(dictionary is not null, $"Invalid null dictionary: {dictionaryText}");
        bool IsValid = IsValidDictionary(dictionary);
        Debug.Assert(IsValid, $"Invalid dictionary: {dictionaryText}");

#pragma warning disable CA1508 // Avoid dead conditional code: when unit testing, this code is not dead.
        if (dictionary is null || !IsValid)
            return;
#pragma warning restore CA1508 // Avoid dead conditional code
#if NET481_OR_GREATER || NETSTANDARD2_0
        Dictionary<TEnumKey, Action> Dictionary = dictionary!; // .NET Framework does not detect that Debug.Assert(obj is not null...) means obj is not null.
#else
        Dictionary<TEnumKey, Action> Dictionary = dictionary;
#endif
#else // #if DEBUG
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(dictionary, $"Invalid null dictionary: {dictionaryText}");
#else
        if (dictionary is null)
            throw new ArgumentNullException(nameof(dictionary), $"Invalid null dictionary: {dictionaryText}");
#endif
        if (!IsValidDictionary(dictionary))
            throw new BrokenContractException($"Invalid dictionary: {dictionaryText}");

        Dictionary<TEnumKey, TValue> Dictionary = dictionary;
#endif // #if DEBUG #else

        if (Dictionary.TryGetValue(expression, out Action? ActionValue))
        {
            ActionValue();
            return;
        }

        object IntValue = Convert.ChangeType(expression, expression.GetTypeCode(), CultureInfo.InvariantCulture);
        string FailureText = $"Enum '{expressionText}' with value {IntValue} not in dictionary.";
#if DEBUG
        Debug.Assert(false, FailureText);
#else // #if DEBUG
        throw new BrokenContractException(FailureText);
#endif // #if DEBUG #else
    }

    private static bool IsValidDictionary<TEnumKey, TValue>(Dictionary<TEnumKey, TValue>? dictionary)
        where TEnumKey : struct, Enum
    {
        // There is already a check for null before. We return true to not have two assert messages queued.
        if (dictionary is null)
            return true;

        int EnumCount = typeof(TEnumKey).GetEnumValues().Length;

        return dictionary.Count == EnumCount;
    }
}
