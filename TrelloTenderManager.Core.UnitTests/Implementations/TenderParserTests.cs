using Bogus;
using TrelloTenderManager.Core.Implementations;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.UnitTests.Implementations;

public class TenderParserTests
{
    private readonly Faker _faker = new();

    [Fact, Trait("Category", "UnitTests")]
    public void Validate_ValidTenders_ReturnsValidTenderValidationResult()
    {
        // Arrange
        var tenders = new List<Tender>
        {
            new() { Id = _faker.Random.Guid(), TenderId = _faker.Random.Guid(), Name = _faker.Company.CompanyName(), TenderName = _faker.Random.Words(3) },
            new() { Id = _faker.Random.Guid(), TenderId = _faker.Random.Guid(), Name = _faker.Company.CompanyName(), TenderName = _faker.Random.Words(3) },
            new() { Id = _faker.Random.Guid(), TenderId = _faker.Random.Guid(), Name = _faker.Company.CompanyName(), TenderName = _faker.Random.Words(3) }
        };

        // Act
        var result = TenderParser.Validate(tenders);

        // Assert
        Assert.Equal(tenders.Count, result.ValidTenders.Count);
        Assert.Empty(result.InvalidTenders);
    }

    [Fact, Trait("Category", "UnitTests")]
    public void Validate_InvalidTenders_ReturnsInvalidTenderValidationResult()
    {
        // Arrange
        var tenders = new List<Tender>
        {
            null!,
            new() { Id = Guid.Empty, TenderId = _faker.Random.Guid(), Name = "Tender 1", TenderName = "Tender Name 1" },
            new() { Id = _faker.Random.Guid(), TenderId = Guid.Empty, Name = "Tender 2", TenderName = "Tender Name 2" },
            new() { Id = _faker.Random.Guid(), TenderId = _faker.Random.Guid(), Name = "Tender 3", TenderName = "" },
            new() { Id = _faker.Random.Guid(), TenderId = _faker.Random.Guid(), Name = "", TenderName = "Tender Name 4" }
        };

        // Act
        var result = TenderParser.Validate(tenders);

        // Assert
        Assert.Empty(result.ValidTenders);
        Assert.Equal(tenders.Count, result.InvalidTenders.Count);
    }

    [Fact, Trait("Category", "UnitTests")]
    public void Validate_ValidAndInvalidTenders_ReturnsValidAndInvalidTenderValidationResult()
    {
        // Arrange
        var tenders = new List<Tender>
        {
            new() { Id = _faker.Random.Guid(), TenderId = _faker.Random.Guid(), Name = _faker.Company.CompanyName(), TenderName = _faker.Random.Words(3) },
            new() { Id = _faker.Random.Guid(), TenderId = _faker.Random.Guid(), Name = _faker.Company.CompanyName(), TenderName = _faker.Random.Words(3) },
            new() { Id = _faker.Random.Guid(), TenderId = _faker.Random.Guid(), Name = _faker.Company.CompanyName(), TenderName = _faker.Random.Words(3) }
        };

        var invalidTenders = new List<Tender>
        {
            null!,
            new() { Id = Guid.Empty, TenderId = _faker.Random.Guid(), Name = "Tender 1", TenderName = "Tender Name 1" },
            new() { Id = _faker.Random.Guid(), TenderId = Guid.Empty, Name = "Tender 2", TenderName = "Tender Name 2" },
            new() { Id = _faker.Random.Guid(), TenderId = _faker.Random.Guid(), Name = "Tender 3", TenderName = "" },
            new() { Id = _faker.Random.Guid(), TenderId = _faker.Random.Guid(), Name = "", TenderName = "Tender Name 4" }
        };

        tenders.AddRange(invalidTenders);

        // Act
        var result = TenderParser.Validate(tenders);

        // Assert
        Assert.Equal(tenders.Count - invalidTenders.Count, result.ValidTenders.Count);
        Assert.Equal(invalidTenders.Count, result.InvalidTenders.Count);
    }

    [Fact, Trait("Category", "UnitTests")]
    public void Filter_AllTheSameTenders()
    {
        // Arrange
        var tender = new Tender { Id = _faker.Random.Guid(), TenderId = _faker.Random.Guid(), Name = _faker.Company.CompanyName(), TenderName = _faker.Random.Words(3) };
        var tenders = new List<Tender>
        {
            tender,
            tender,
            tender
        };

        // Act
        var filteredTenders = TenderParser.Filter(tenders);

        // Assert
        Assert.Single(filteredTenders);
    }

    [Fact, Trait("Category", "UnitTests")]
    public void Filter_MultipleDifferentTenders()
    {
        // Arrange
        var tender = new Tender { Id = _faker.Random.Guid(), TenderId = _faker.Random.Guid(), Name = _faker.Company.CompanyName(), TenderName = _faker.Random.Words(3) };
        var tenders = new List<Tender>
        {
            tender,
            new() { Id = _faker.Random.Guid(), TenderId = _faker.Random.Guid(), Name = _faker.Company.CompanyName(), TenderName = _faker.Random.Words(3) },
            tender
        };

        // Act
        var filteredTenders = TenderParser.Filter(tenders);

        // Assert
        Assert.Equal(2, filteredTenders.Count);
    }
}