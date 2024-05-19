using TrelloTenderManager.Domain.Enums;

namespace TrelloTenderManager.Domain.Models;

public class Tender
{
    /// <summary>
    /// Gets or sets the unique identifier of the tender.
    /// </summary>
    public Guid? Id { get; set; }

    /// <summary>
    /// Gets or sets the unique identifier of the tender.
    /// </summary>
    public Guid? TenderId { get; set; }

    /// <summary>
    /// Gets or sets the lot number of the tender.
    /// </summary>
    public string? LotNumber { get; set; }

    /// <summary>
    /// Gets or sets the deadline of the tender.
    /// </summary>
    public DateTime? Deadline { get; set; }

    /// <summary>
    /// Gets or sets the name of the tender.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the name of the tender.
    /// </summary>
    public string? TenderName { get; set; }

    /// <summary>
    /// Gets or sets the expiration date of the tender.
    /// </summary>
    public DateTime? ExpirationDate { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the tender has documents.
    /// </summary>
    public bool? HasDocuments { get; set; }

    /// <summary>
    /// Gets or sets the location of the tender.
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// Gets or sets the publication date of the tender.
    /// </summary>
    public DateTime? PublicationDate { get; set; }

    /// <summary>
    /// Gets or sets the status of the tender.
    /// </summary>
    public TenderStatus Status { get; set; }

    /// <summary>
    /// Gets or sets the currency of the tender.
    /// </summary>
    public string? Currency { get; set; }

    /// <summary>
    /// Gets or sets the value of the tender.
    /// </summary>
    public decimal? Value { get; set; }

    /// <summary>
    /// Gets or sets the import data of the tender.
    /// </summary>
    public string? ImportData { get; set; }
}
