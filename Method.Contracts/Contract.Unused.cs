namespace Contracts;

/// <summary>
/// A set of tools to enforce contracts in methods.
/// </summary>
public static partial class Contract
{
    /// <summary>
    /// Provides a value for variables that should be uninitialized.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="result">The unused value.</param>
    public static void Unused<T>(out T result)
        where T : class => result = default!;

    /// <summary>
    /// Provides a value for variables that should be uninitialized.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="result">The unused value.</param>
    public static void Unused<T>(out T? result)
        where T : struct => result = null;

    /// <summary>
    /// Provides a value for variables that should be uninitialized.
    /// </summary>
    /// <typeparam name="T">The value type.</typeparam>
    /// <param name="result">The unused value.</param>
    /// <param name="_1">Unused.</param>
    public static void Unused<T>(out T result,
#pragma warning disable SA1313 // Parameter names should begin with lower-case letter
                                 T? _1 = null)
#pragma warning restore SA1313 // Parameter names should begin with lower-case letter
        where T : struct => result = default;
}
