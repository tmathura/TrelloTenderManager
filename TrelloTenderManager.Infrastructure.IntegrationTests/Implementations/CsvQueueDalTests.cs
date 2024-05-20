using Microsoft.Extensions.Configuration;
using Moq;
using TrelloTenderManager.Domain.DataAccessObjects;
using TrelloTenderManager.Domain.Enums;
using TrelloTenderManager.Domain.Exceptions;
using TrelloTenderManager.Infrastructure.Implementations;

namespace TrelloTenderManager.Infrastructure.IntegrationTests.Implementations
{
    public class CsvQueueDalTests : IDisposable
    {
        private const string DatabasePath = "TrelloTenderManager.Infrastructure.IntegrationTests.SQLite.db3";
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly CsvQueueDal _csvQueueDal;

        public CsvQueueDalTests()
        {

            _configurationMock = new Mock<IConfiguration>();
            _configurationMock.Setup(c => c["Database:Name"]).Returns(DatabasePath);

            _csvQueueDal = new CsvQueueDal(_configurationMock.Object);
        }

        [Fact, Trait("Category", "IntegrationTests")]
        public async Task CsvQueue_Create()
        {
            // Arrange
            const string csvContent = "Test content";
            var csvQueueDao = new CsvQueueDao
            {
                CsvContent = csvContent,
            };

            // Act
            var csvQueueId = await _csvQueueDal.CreateCsvQueue(csvQueueDao);

            // Assert
            Assert.True(csvQueueId > 0);

            var readCsvQueueDaos = await _csvQueueDal.ReadCsvQueue();

            Assert.NotNull(readCsvQueueDaos);
            Assert.Single(readCsvQueueDaos);

            var readCsvQueueDao = readCsvQueueDaos.FirstOrDefault();

            Assert.NotNull(readCsvQueueDao);
            Assert.Equal(csvContent, readCsvQueueDao.CsvContent);
            Assert.Equal(readCsvQueueDao.Status, QueueStatus.Unprocessed);
        }

        [Fact, Trait("Category", "IntegrationTests")]
        public async Task CsvQueue_Read()
        {
            // Arrange
            const string csvContent = "Test content";
            var csvQueueDao = new CsvQueueDao
            {
                CsvContent = csvContent,
                Status = QueueStatus.Unprocessed
            };

            await _csvQueueDal.CreateCsvQueue(csvQueueDao);
            await _csvQueueDal.CreateCsvQueue(csvQueueDao);
            
            // Act
            var readCsvQueueDaos = await _csvQueueDal.ReadCsvQueue();

            Assert.NotNull(readCsvQueueDaos);
            Assert.Equal(2, readCsvQueueDaos.Count);
        }

        [Fact, Trait("Category", "IntegrationTests")]
        public async Task CsvQueue_Update()
        {
            // Arrange
            var csvContent = "Test content";
            var csvQueueDao = new CsvQueueDao
            {
                CsvContent = csvContent,
                Status = QueueStatus.Unprocessed
            };

            var csvQueueId = await _csvQueueDal.CreateCsvQueue(csvQueueDao);
            
            csvContent = "Test content updated";
            csvQueueDao.CsvContent = csvContent;

            // Act
            var updatedRows = await _csvQueueDal.UpdateCsvQueue(csvQueueDao);
            Assert.Equal(1, updatedRows);

            // Update Assert
            var readCsvQueueDaos = await _csvQueueDal.ReadCsvQueue(expression => expression.Id == csvQueueId);
            Assert.NotNull(readCsvQueueDaos);
            Assert.Single(readCsvQueueDaos);

            var readCsvQueueDao = readCsvQueueDaos.FirstOrDefault();

            Assert.NotNull(readCsvQueueDao);
            Assert.Equal(csvQueueId, readCsvQueueDao.Id);
            Assert.Equal(csvContent, readCsvQueueDao.CsvContent);
        }

        [Fact, Trait("Category", "IntegrationTests")]
        public void Constructor_Should_Throw_AppSettingsException_When_No_DatabasePath()
        {
            // Arrange
            _configurationMock.Setup(c => c["Database:Name"]).Returns(string.Empty);

            // Assert
            var exception = Assert.Throws<AppSettingsException>(Action);
            Assert.Equal("Error getting database name string from configuration.", exception.Message);
            return;

            // Act
            void Action() => _ = new CsvQueueDal(_configurationMock.Object);
        }

        public void Dispose()
        {
            _csvQueueDal.Dispose();

            if (File.Exists(DatabasePath))
            {
                File.Delete(DatabasePath);
            }
        }
    }
}