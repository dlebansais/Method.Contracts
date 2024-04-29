namespace Contracts;

using System;

/// <summary>
/// Represents one or more requirements.
/// </summary>
[AttributeUsage(AttributeTargets.Method)]
public sealed class RequireAttribute : Attribute
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RequireAttribute"/> class.
    /// </summary>
    /// <param name="requirements">The requirements.</param>
    public RequireAttribute(params string[] requirements)
    {
        Requirements = requirements;
    }

    /// <summary>
    /// Gets the requirements.
    /// </summary>
    public string[] Requirements { get; }
}