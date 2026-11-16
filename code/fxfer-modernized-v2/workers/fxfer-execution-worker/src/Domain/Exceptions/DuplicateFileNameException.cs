using System;

namespace Fxfer.ExecutionWorker.Domain.Exceptions;

/// <summary>
/// Indicates that a duplicate file was found at the source
/// </summary>
public class DuplicateFileNameException : Exception
{
    /// <summary>
    /// Construct an exception with a message
    /// </summary>
    /// <param name="message"></param>
    public DuplicateFileNameException(string message) : base(message)
    {
    }
}

