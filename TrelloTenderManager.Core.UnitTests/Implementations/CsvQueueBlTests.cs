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
        public async Task CreateCsvQueue_Should_Call_CreateCsvQueue_On_CsvQueueDal()
        {
            // Arrange
            const string csvFileContent = "Sample CSV content";

            _csvQueueDalMock.Setup(dal => dal.CreateCsvQueue(It.Is<CsvQueueDao>(dao => dao.CsvContent == csvFileContent))).ReturnsAsync(1);

            // Act
            var result = await _csvQueueBl.CreateCsvQueue(It.IsAny<string>(), csvFileContent);

            // Assert
            _csvQueueDalMock.Verify(dal => dal.CreateCsvQueue(It.Is<CsvQueueDao>(dao => dao.CsvContent == csvFileContent)), Times.Once);
            Assert.Equal(1, result);
        }


        [Fact, Trait("Category", "UnitTests")]
        public async Task ReadCsvQueue_Should_Call_ReadCsvQueue_On_CsvQueueDal()
        {
            // Arrange
            const string csvFileContent = "Sample CSV content";
            var csvQueue = new CsvQueueDao
            {
                CsvContent = csvFileContent
            };

            _csvQueueDalMock.Setup(dal => dal.ReadCsvQueue(It.IsAny<Expression<Func<CsvQueueDao, bool>>?>())).ReturnsAsync([csvQueue]);

            // Act
            var result = await _csvQueueBl.ReadCsvQueue(It.IsAny<Expression<Func<CsvQueueDao, bool>>?>());

            // Assert
            _csvQueueDalMock.Verify(dal => dal.ReadCsvQueue(It.IsAny<Expression<Func<CsvQueueDao, bool>>?>()), Times.Once);
            Assert.NotNull(result);
            Assert.Single(result);
        }

        [Fact, Trait("Category", "UnitTests")]
        public async Task UpdateCsvQueue_Should_Call_UpdateCsvQueue_On_CsvQueueDal()
        {
            // Arrange
            var csvQueue = new CsvQueueDao
            {
                CsvContent = "Sample CSV content"
            };

            _csvQueueDalMock.Setup(dal => dal.UpdateCsvQueue(csvQueue)).ReturnsAsync(1);

            // Act
            var result = await _csvQueueBl.UpdateCsvQueue(csvQueue);

            // Assert
            _csvQueueDalMock.Verify(dal => dal.UpdateCsvQueue(csvQueue), Times.Once);
            Assert.Equal(1, result);
        }
    }
}
