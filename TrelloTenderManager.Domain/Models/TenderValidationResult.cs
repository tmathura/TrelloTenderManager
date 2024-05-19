namespace TrelloTenderManager.Domain.Models;

public class TenderValidationResult(List<Tender> validTenders, List<Tender> invalidTender)
{
    /// <summary>
    /// Gets or sets the list of valid tenders.
    /// </summary>
    public List<Tender> ValidTenders { get; set; } = validTenders;

    /// <summary>
    /// Gets or sets the list of invalid tenders.
    /// </summary>
    public List<Tender> InvalidTenders { get; set; } = invalidTender;
}
