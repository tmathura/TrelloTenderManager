using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using SQLite;
using System.Net;
using TrelloTenderManager.Domain.DataAccessObjects;
using TrelloTenderManager.Tests.Common.Helpers;
using TrelloTenderManager.WebApi.IntegrationTests.Common.Helpers;
using Xunit.Abstractions;

namespace TrelloTenderManager.WebApi.IntegrationTests.Controllers;

public class QueueControllerTests : IClassFixture<CommonHelper>
{
    private readonly CommonHelper _commonHelper;

    public QueueControllerTests(ITestOutputHelper outputHelper, CommonHelper commonHelper)
    {
        commonHelper.OutputHelper = outputHelper;
        _commonHelper = commonHelper;

        var databasePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TrelloTenderManager.Infrastructure.SQLite.db3");
        databasePath = databasePath.Replace("TrelloTenderManager.WebApi.IntegrationTests", "TrelloTenderManager.WebApi");

        if (File.Exists(databasePath))
        {
            var database = new SQLiteAsyncConnection(databasePath);

            database.DropTableAsync<CsvQueueDao>().Wait();
            database.CreateTableAsync<CsvQueueDao>().Wait();
        }
    }

    [Fact, Trait("Category", "IntegrationTests")]
    public async Task QueueFromCsv()
    {
        // Arrange
        var fileContent = TestDataHelper.GetSampleFileFileContent(1);
        var processFromCsvRequest = new
        {
            fileContent
        };

        await _commonHelper.CallEndPoint("api/card/queue-from-csv", null, Method.Post, processFromCsvRequest);
        await _commonHelper.CallEndPoint("api/card/queue-from-csv", null, Method.Post, processFromCsvRequest);

        // Act
        var response = await _commonHelper.CallEndPoint("api/queue", null, Method.Get, null);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        dynamic responseContent = JsonConvert.DeserializeObject(response.Content);

        Assert.NotNull(responseContent);
        Assert.Equal(2, ((JArray)responseContent).Count);
    }
}