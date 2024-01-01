namespace Contracts;

using System;

/// <summary>
/// Represents errors that occur during contract checking.
/// </summary>
[Serializable]
public class BrokenContractException : Exception
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BrokenContractException"/> class.
    /// </summary>
    public BrokenContractException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BrokenContractException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public BrokenContractException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BrokenContractException"/> class.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    /// <param name="innerException">The exception that is the cause of the current exception, or a null reference if no inner exception is specified.</param>
    public BrokenContractException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
