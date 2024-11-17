#pragma warning disable CA1019 // Define accessors for attribute arguments: ArgumentName is available through ArgumentNames.

namespace Contracts;

using System;

/// <summary>
/// Represents one or more arguments that must not be null.
/// The primary constructor is not CLS-compliant.
/// </summary>
/// <param name="argumentNames">The argument names.</param>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class RequireNotNullAttribute(params string[] argumentNames) : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequireNotNullAttribute"/> class.
    /// The purpose of this constructor is to provide a workaround for programs that need to be CLS-compliant.
    /// </summary>
    /// <param name="argumentName">The argument name.</param>
    public RequireNotNullAttribute(string argumentName)
        : this([argumentName])
    {
    }

    /// <summary>
    /// Gets the argument names.
    /// </summary>
    public string[] ArgumentNames { get; } = argumentNames;

    /// <summary>
    /// Gets or sets the type.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the alias name.
    /// </summary>
    public string AliasName { get; set; } = string.Empty;
}