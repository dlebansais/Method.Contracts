namespace Contracts;

using System;

/// <summary>
/// Represents the name of an initializing method that must be called right after an object is constructed.
/// </summary>
/// <param name="methodName">The method name.</param>
[AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false)]
public sealed class InitializeWithAttribute(string methodName) : Attribute
{
    /// <summary>
    /// Gets the method name.
    /// </summary>
    public string MethodName { get; } = methodName;
}
