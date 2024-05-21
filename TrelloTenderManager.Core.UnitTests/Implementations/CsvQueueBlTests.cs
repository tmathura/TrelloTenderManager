using Moq;
using System.Linq.Expressions;
using TrelloTenderManager.Core.Implementations;
using TrelloTenderManager.Core.Interfaces;
using TrelloTenderManager.Domain.DataAccessObjects;
using TrelloTenderManager.Infrastructure.Interfaces;

namespace TrelloTenderManager.Core.UnitTests.Implementations
{
    public class CsvQueueBlTests
    {
        private readonly Mock<ICsvQueueDal> _csvQueueDalMock;
        private readonly CsvQueueBl _csvQueueBl;

        public CsvQueueBlTests()
        {
            _csvQueueDalMock = new Mock<ICsvQueueDal>();
            var cardManager = new Mock<ICardManager>();

            _csvQueueBl = new CsvQueueBl(_csvQueueDalMock.Object, cardManager.Object);
        }

        [Fact, Trait("Category", "UnitTests")]
        public async Task BlCreate_Should_Call_DalCreate()
        {
            // Arrange
            const string? csvFileContent = "Sample CSV content";

            _csvQueueDalMock.Setup(dal => dal.Create(It.Is<CsvQueueDao>(dao => dao.CsvContent == csvFileContent))).ReturnsAsync(1);

            // Act
            var result = await _csvQueueBl.CreateCsvQueue(It.IsAny<string>(), csvFileContent);

            // Assert
            _csvQueueDalMock.Verify(dal => dal.Create(It.Is<CsvQueueDao>(dao => dao.CsvContent == csvFileContent)), Times.Once);
            Assert.Equal(1, result);
        }


        [Fact, Trait("Category", "UnitTests")]
        public async Task BlRead_Should_Call_DalRead()
        {
            // Arrange
            const string csvFileContent = "Sample CSV content";
            var csvQueue = new CsvQueueDao
            {
                CsvContent = csvFileContent
            };

            _csvQueueDalMock.Setup(dal => dal.Read(It.IsAny<Expression<Func<CsvQueueDao, bool>>?>())).ReturnsAsync([csvQueue]);

            // Act
            var result = await _csvQueueBl.ReadCsvQueue(It.IsAny<Expression<Func<CsvQueueDao, bool>>?>());

            // Assert
            _csvQueueDalMock.Verify(dal => dal.Read(It.IsAny<Expression<Func<CsvQueueDao, bool>>?>()), Times.Once);
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact, Trait("Category", "UnitTests")]
        public async Task BlUpdate_Should_Call_DalUpdate()
        {
            // Arrange
            var csvQueue = new CsvQueueDao
            {
                CsvContent = "Sample CSV content"
            };

            _csvQueueDalMock.Setup(dal => dal.Update(csvQueue)).ReturnsAsync(1);

            // Act
            var result = await _csvQueueBl.UpdateCsvQueue(csvQueue);

            // Assert
            _csvQueueDalMock.Verify(dal => dal.Update(csvQueue), Times.Once);
            Assert.Equal(1, result);
        }
    }
}
