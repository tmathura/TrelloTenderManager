using Bogus;
using TrelloTenderManager.Domain.Enums;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.UnitTests.Fakers;

public sealed class TenderFaker : Faker<Tender>
{
    public TenderFaker()
    {
        RuleFor(tender => tender.Id, f => f.Random.Guid());
        RuleFor(tender => tender.TenderId, f => f.Random.Guid());
        RuleFor(tender => tender.LotNumber, f => f.Random.Int(0, 10).ToString());
        RuleFor(tender => tender.Deadline, f => f.Date.Future());
        RuleFor(tender => tender.Name, f => f.Company.CompanyName());
        RuleFor(tender => tender.TenderName, f => f.Random.Words(3));
        RuleFor(tender => tender.ExpirationDate, f => f.Date.Past(2));
        RuleFor(tender => tender.HasDocuments, f => f.Random.Bool());
        RuleFor(tender => tender.Location, f => f.Address.Country());
        RuleFor(tender => tender.PublicationDate, f => f.Date.Past());
        RuleFor(tender => tender.Status, f => f.PickRandom<TenderStatus>());
        RuleFor(tender => tender.Currency, f => f.Finance.Currency().Code);
        RuleFor(tender => tender.Value, f => f.Finance.Amount(0, 100));
    }
}