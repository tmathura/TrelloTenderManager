namespace TrelloTenderManager.Domain.Models;

public class TenderValidationResult(List<Tender> validTenders, List<Tender> invalidTender)
{
    public List<Tender> ValidTenders { get; set; } = validTenders;
    public List<Tender> InvalidTenders { get; set; } = invalidTender;
}