using Newtonsoft.Json;
using RestSharp;
using System.Net;
using TrelloTenderManager.Tests.Common.Helpers;
using TrelloTenderManager.WebApi.IntegrationTests.Common.Helpers;
using TrelloTenderManager.WebApi.IntegrationTests.MemberData;
using Xunit.Abstractions;

namespace TrelloTenderManager.WebApi.IntegrationTests.Controllers;

public class CardControllerTests : IClassFixture<CommonHelper>
{
    private readonly CommonHelper _commonHelper;

    public CardControllerTests(ITestOutputHelper outputHelper, CommonHelper commonHelper)
    {
        commonHelper.OutputHelper = outputHelper;
        _commonHelper = commonHelper;
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

        // Act
        var response = await _commonHelper.CallEndPoint("api/card/queue-from-csv", null, Method.Post, processFromCsvRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Theory, Trait("Category", "IntegrationTests")]
    [MemberData(nameof(ProcessFromCsvMemberData.GetData), MemberType = typeof(ProcessFromCsvMemberData))]
    public async Task ProcessFromCsv(string fileContent, int expectedCardsCreatedCount, int expectedCardsUpdatedCount)
    {
        // Arrange
        var processFromCsvRequest = new
        {
            fileContent
        };

        // Act
        var response = await _commonHelper.CallEndPoint("api/card/process-from-csv", null, Method.Post, processFromCsvRequest);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        dynamic responseContent = JsonConvert.DeserializeObject(response.Content);

        Assert.NotNull(responseContent);

        var processFromCsvResult = responseContent?.processFromCsvResult;
        var actualCardsCreatedCount = processFromCsvResult?.createdCount.Value;
        var actualCardsUpdatedCount = processFromCsvResult?.updatedCount.Value;

        Assert.Equal(expectedCardsCreatedCount, actualCardsCreatedCount);
        Assert.Equal(expectedCardsUpdatedCount, actualCardsUpdatedCount);
    }
}