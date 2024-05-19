namespace TrelloTenderManager.Domain.Models;

public class ProcessFromCsvResult
{
    /// <summary>
    /// Gets or sets the count of items created.
    /// </summary>
    public int CreatedCount { get; set; }

    /// <summary>
    /// Gets or sets the count of items updated.
    /// </summary>
    public int UpdatedCount { get; set; }
}
