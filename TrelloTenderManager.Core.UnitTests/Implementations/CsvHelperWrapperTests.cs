using CsvHelper;
using TrelloTenderManager.Core.Implementations;
using TrelloTenderManager.Core.UnitTests.Factories;
using TrelloTenderManager.Domain.CsvClassMaps;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.UnitTests.Implementations;

public class CsvHelperWrapperTests
{
    private readonly CsvHelperWrapper _csvHelperWrapper = new();

    [Fact, Trait("Category", "UnitTests")]
    public void GetRecords_ValidData()
    {
        // Arrange
        const int numberOfRecords = 3;

        var stringBuilder = CsvRecordFactory.Generate(numberOfRecords);

        // Act
        var csvRecords = _csvHelperWrapper.GetRecords<Tender>(stringBuilder.ToString(), typeof(TenderMap));

        // Assert
        Assert.Equal(numberOfRecords, csvRecords.Count);
    }

    [Fact, Trait("Category", "UnitTests")]
    public void GetRecords_1RecordAllFieldsEmpty()
    {
        // Arrange
        const int numberOfRecords = 3;

        var stringBuilder = CsvRecordFactory.Generate(numberOfRecords);
        stringBuilder.AppendLine(",,,,,,,,,,,,");

        // Act
        var csvRecords = _csvHelperWrapper.GetRecords<Tender>(stringBuilder.ToString(), typeof(TenderMap));

        // Assert
        Assert.Equal(numberOfRecords + 1, csvRecords.Count);
    }

    [Fact, Trait("Category", "UnitTests")]
    public void GetRecords_1RecordMissingFields()
    {
        // Arrange
        const int numberOfRecords = 3;

        var stringBuilder = CsvRecordFactory.Generate(numberOfRecords);
        stringBuilder.AppendLine(",,,");
            
        // Act
        var csvRecords = _csvHelperWrapper.GetRecords<Tender>(stringBuilder.ToString(), typeof(TenderMap));

        // Assert
        Assert.Equal(numberOfRecords + 1, csvRecords.Count);
    }

    [Fact, Trait("Category", "UnitTests")]
    public void GetRecords_1RecordBadData()
    {
        // Arrange
        const int numberOfRecords = 3;

        var stringBuilder = CsvRecordFactory.Generate(numberOfRecords);
        stringBuilder.AppendLine($"{Guid.NewGuid()},\",,1,");

        // Assert
        Assert.Throws<BadDataException>(Action);
        return;

        // Act
        void Action() => _csvHelperWrapper.GetRecords<Tender>(stringBuilder.ToString(), typeof(TenderMap));
    }
}