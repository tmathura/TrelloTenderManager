namespace TrelloTenderManager.WebApi.Filters;

/// <summary>
/// Represents an exception that is used to return an HTTP response with a specific status code and optional value.
/// </summary>
public class HttpResponseException(int statusCode, object? value = null) : Exception
{
    /// <summary>
    /// Gets the status code of the HTTP response.
    /// </summary>
    public int StatusCode { get; } = statusCode;

    /// <summary>
    /// Gets the optional value associated with the HTTP response.
    /// </summary>
    public object? Value { get; } = value;
}
