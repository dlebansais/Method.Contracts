#pragma warning disable CA1019 // Define accessors for attribute arguments: Expression is available through Expressions.

namespace Contracts;

using System;

/// <summary>
/// Represents one or more requirements.
/// The primary constructor is not CLS-compliant.
/// </summary>
/// <param name="expressions">The requirements.</param>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = true)]
public sealed class RequireAttribute(params string[] expressions) : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequireAttribute"/> class.
    /// The purpose of this constructor is to provide a workaround for programs that need to be CLS-compliant.
    /// </summary>
    /// <param name="expression">The requirement.</param>
    public RequireAttribute(string expression)
        : this([expression])
    {
    }

    /// <summary>
    /// Gets the requirements.
    /// </summary>
    public string[] Expressions { get; } = expressions;

    /// <summary>
    /// Gets or sets a value indicating whether code should be generated only if DEBUG is set.
    /// </summary>
    public bool DebugOnly { get; set; }
}
