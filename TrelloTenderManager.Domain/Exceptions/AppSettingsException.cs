namespace TrelloTenderManager.Domain.Exceptions;

/// <summary>
/// Represents an exception that is thrown when there is an error with the application settings.
/// </summary>
public class AppSettingsException(string message) : Exception(message);
