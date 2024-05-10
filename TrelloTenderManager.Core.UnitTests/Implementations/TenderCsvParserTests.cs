using Moq;
using TrelloTenderManager.Core.Implementations;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Core.UnitTests.Fakers;
using TrelloTenderManager.Domain.CsvClassMaps;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.UnitTests.Implementations
{
    public class TenderCsvParserTests
    {
        private readonly Mock<ICsvHelperWrapper> _csvHelperWrapper;
        private readonly TenderCsvParser _tenderCsvParser;

        public TenderCsvParserTests()
        {
            _csvHelperWrapper = new Mock<ICsvHelperWrapper>();
            _tenderCsvParser = new TenderCsvParser(_csvHelperWrapper.Object);
        }

        [Fact]
        public void GetRecords_ValidData()
        {
            // Arrange
            const int numberOfRecords = 3;

            var tenderFaker = new TenderFaker();
            var tenders = tenderFaker.Generate(numberOfRecords);

            _csvHelperWrapper.Setup(fileProcessorBl => fileProcessorBl.GetRecords<Tender>(It.IsAny<string>(), typeof(TenderMap))).Returns(tenders);

            // Act
            var csvRecords = _tenderCsvParser.Parse(It.IsAny<string>());

            // Assert
            Assert.NotNull(csvRecords);
            Assert.Equal(numberOfRecords, csvRecords.Count);
        }

        [Fact]
        public void GetRecords_1RecordBadData()
        {
            // Arrange
            _csvHelperWrapper.Setup(fileProcessorBl => fileProcessorBl.GetRecords<Tender>(It.IsAny<string>(), typeof(TenderMap))).Throws<Exception>();

            // Assert
            Assert.Throws<Exception>(Action);
            return;

            // Act
            void Action() => _tenderCsvParser.Parse(It.IsAny<string>());
        }
    }
}