using Bogus;
using TrelloTenderManager.Domain.Enums;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.UnitTests.Fakers
{
    public sealed class TenderFaker : Faker<Tender>
    {
        public TenderFaker()
        {
             RuleFor(d => d.Id, f => f.Random.Guid());
             RuleFor(d => d.TenderId, f => f.Random.Guid());
             RuleFor(d => d.LotNumber, f => f.Random.Int(0, 10).ToString());
             RuleFor(d => d.Deadline, f => f.Date.Future());
             RuleFor(d => d.Name, f => f.Company.CompanyName());
             RuleFor(d => d.TenderName, f => f.Random.Words(3));
             RuleFor(d => d.ExpirationDate, f => f.Date.Past(2));
             RuleFor(d => d.HasDocuments, f => f.Random.Bool());
             RuleFor(d => d.Location, f => f.Address.Country());
             RuleFor(d => d.PublicationDate, f => f.Date.Past());
             RuleFor(d => d.Status, f => f.PickRandom<TenderStatus>());
             RuleFor(d => d.Currency, f => f.Finance.Currency().Code);
             RuleFor(d => d.Value, f => f.Finance.Amount(0, 100));
        }
    }
}
