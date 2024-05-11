namespace Contracts;

using System;

/// <summary>
/// Represents one or more guarantees.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class EnsureAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="EnsureAttribute"/> class.
    /// </summary>
    /// <param name="expressions">The guarantees.</param>
    public EnsureAttribute(params string[] expressions)
    {
        Expressions = expressions;
    }

    /// <summary>
    /// Gets the guarantees.
    /// </summary>
    public string[] Expressions { get; }

    /// <summary>
    /// Gets or sets a value indicating whether code should be generated only if DEBUG is set.
    /// </summary>
    public bool DebugOnly { get; set; }
}