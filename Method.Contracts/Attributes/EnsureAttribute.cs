namespace Contracts;

using System;

/// <summary>
/// Represents one or more guarantees.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
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
}