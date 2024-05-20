namespace TrelloTenderManager.Domain.Requests;

public class QueueFromCsvRequest
{
    /// <summary>
    /// Gets or sets the filename of the CSV.
    /// </summary>
    public string? Filename { get; set; }

    /// <summary>
    /// Gets or sets the content of the file.
    /// </summary>
    public string? FileContent { get; set; }
}
