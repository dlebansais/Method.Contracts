namespace Contracts;

using System;

/// <summary>
/// Represents one or more arguments that must not be null.
/// </summary>
[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public sealed class RequireNotNullAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequireNotNullAttribute"/> class.
    /// </summary>
    /// <param name="argumentNames">The argument names.</param>
    public RequireNotNullAttribute(params string[] argumentNames)
    {
        ArgumentNames = argumentNames;
    }

    /// <summary>
    /// Gets the argument names.
    /// </summary>
    public string[] ArgumentNames { get; }

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