namespace Contracts;

using System;
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
