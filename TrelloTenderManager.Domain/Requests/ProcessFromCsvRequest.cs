namespace TrelloTenderManager.Domain.Requests;

public class ProcessFromCsvRequest
{
    /// <summary>
    /// Gets or sets the content of the file.
    /// </summary>
    public string[]? FileContent { get; set; }
}
