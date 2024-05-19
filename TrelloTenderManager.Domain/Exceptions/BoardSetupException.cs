namespace TrelloTenderManager.Domain.Exceptions;

/// <summary>
/// Represents an exception that occurs during board setup.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BoardSetupException"/> class with a specified error message and a reference to the inner exception that is the cause of this exception.
/// </remarks>
/// <param name="message">The error message that explains the reason for the exception.</param>
/// <param name="inner">The exception that is the cause of the current exception.</param>
public class BoardSetupException(string message, Exception inner) : Exception(message, inner);
