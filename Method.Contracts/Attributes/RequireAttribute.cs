namespace Contracts;

using System;

/// <summary>
/// Represents one or more requirements.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class RequireAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequireAttribute"/> class.
    /// </summary>
    /// <param name="expressions">The requirements.</param>
    public RequireAttribute(params string[] expressions)
    {
        Expressions = expressions;
    }

    /// <summary>
    /// Gets the requirements.
    /// </summary>
    public string[] Expressions { get; }
}