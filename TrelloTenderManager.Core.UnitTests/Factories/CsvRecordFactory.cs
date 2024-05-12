using Bogus;
using System.Text;
using TrelloTenderManager.Domain.Enums;

namespace TrelloTenderManager.Core.UnitTests.Factories;

public static class CsvRecordFactory
{
    private static readonly Faker Faker = new();

    public static StringBuilder Generate(int numberOfRecords)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.AppendLine("Id,TenderId,LotNumber,Deadline,Name,TenderName,ExpirationDate,HasDocuments,Location,PublicationDate,Status,Currency,Value");

        for (var i = 0; i < numberOfRecords; i++)
        {
            stringBuilder.AppendLine(GenerateCsvRecord());
        }

        return stringBuilder;
    }

    private static string GenerateCsvRecord()
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append($"{Faker.Random.Guid()},");
        stringBuilder.Append($"{Faker.Random.Guid()},");
        stringBuilder.Append($"{Faker.Random.Int(0, 10)},");
        stringBuilder.Append($"{Faker.Date.Future().ToUniversalTime().ToString("u")},");
        stringBuilder.Append($"{Faker.Company.CompanyName()},");
        stringBuilder.Append($"{Faker.Random.Words(3)},");
        stringBuilder.Append($"{Faker.Date.Past(2).ToUniversalTime().ToString("u")},");
        stringBuilder.Append($"{Faker.Random.Bool()},");
        stringBuilder.Append($"{Faker.Address.Country()},");
        stringBuilder.Append($"{Faker.Date.Past().ToUniversalTime().ToString("u")},");
        stringBuilder.Append($"{Faker.PickRandom<TenderStatus>()},");
        stringBuilder.Append($"{Faker.Finance.Currency().Code},");
        stringBuilder.Append($"{Faker.Finance.Amount(0, 100)}");

        return stringBuilder.ToString();
    }
}