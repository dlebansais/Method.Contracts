namespace Contracts;

using System;

/// <summary>
/// Represents the generated method access specifiers attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class AccessAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AccessAttribute"/> class.
    /// </summary>
    /// <param name="specifiers">The method access specifiers.</param>
    public AccessAttribute(params string[] specifiers)
    {
        Specifiers = specifiers;
    }

    /// <summary>
    /// Gets the method access specifiers.
    /// </summary>
    public string[] Specifiers { get; }
}