using TrelloTenderManager.Domain.Enums;

namespace TrelloTenderManager.Domain.Models;

public class Tender
{
    public Guid? Id { get; set; }
    public Guid? TenderId { get; set; }
    public string? LotNumber { get; set; }
    public DateTime? Deadline { get; set; }
    public string? Name { get; set; }
    public string? TenderName { get; set; }
    public DateTime? ExpirationDate { get; set; }
    public bool? HasDocuments { get; set; }
    public string? Location { get; set; }
    public DateTime? PublicationDate { get; set; }
    public TenderStatus Status { get; set; }
    public string? Currency { get; set; }
    public decimal? Value { get; set; }
    public string? ImportData { get; set; }
}