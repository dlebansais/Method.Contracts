namespace Contracts;

using System;

/// <summary>
/// Represents one or more arguments that must not be null.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
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
}