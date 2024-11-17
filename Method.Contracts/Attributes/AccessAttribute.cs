#pragma warning disable CA1019 // Define accessors for attribute arguments: Specifier is available through Specifiers.

namespace Contracts;

using System;

/// <summary>
/// Represents the generated method access specifiers attribute.
/// The primary constructor is not CLS-compliant.
/// </summary>
/// <param name="specifiers">The method access specifiers.</param>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public sealed class AccessAttribute(params string[] specifiers) : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AccessAttribute"/> class.
    /// The purpose of this constructor is to provide a workaround for programs that need to be CLS-compliant.
    /// </summary>
    /// <param name="specifier">The method access specifier.</param>
    public AccessAttribute(string specifier)
        : this([specifier])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessAttribute"/> class.
    /// The purpose of this constructor is to provide a workaround for programs that need to be CLS-compliant.
    /// </summary>
    /// <param name="specifier1">The first method access specifier.</param>
    /// <param name="specifier2">The second method access specifier.</param>
    public AccessAttribute(string specifier1, string specifier2)
        : this([specifier1, specifier2])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessAttribute"/> class.
    /// The purpose of this constructor is to provide a workaround for programs that need to be CLS-compliant.
    /// </summary>
    /// <param name="specifier1">The first method access specifier.</param>
    /// <param name="specifier2">The second method access specifier.</param>
    /// <param name="specifier3">The third method access specifier.</param>
    public AccessAttribute(string specifier1, string specifier2, string specifier3)
        : this([specifier1, specifier2, specifier3])
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AccessAttribute"/> class.
    /// The purpose of this constructor is to provide a workaround for programs that need to be CLS-compliant.
    /// </summary>
    /// <param name="specifier1">The first method access specifier.</param>
    /// <param name="specifier2">The second method access specifier.</param>
    /// <param name="specifier3">The third method access specifier.</param>
    /// <param name="specifier4">The fourth method access specifier.</param>
    public AccessAttribute(string specifier1, string specifier2, string specifier3, string specifier4)
        : this([specifier1, specifier2, specifier3, specifier4])
    {
    }

    /// <summary>
    /// Gets the method access specifiers.
    /// </summary>
    public string[] Specifiers { get; } = specifiers;
}