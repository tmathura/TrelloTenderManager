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

    [Theory, Trait("Category", "IntegrationTests")]
    [MemberData(nameof(ProcessFromCsvMemberData.GetData), MemberType = typeof(ProcessFromCsvMemberData))]
    public async Task ProcessFromCsv(string[] fileContent, int expectedCardsCreatedCount, int expectedCardsUpdatedCount)
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

    [Fact, Trait("Category", "IntegrationTests")]
    public async Task ProcessFromCsv_FileTooBig()
    {
        // Arrange
        var fileContent = TestDataHelper.GetSampleFileFileContentLines(3);
        var processFromCsvRequest = new
        {
            fileContent
        };

        // Act
        var response = await _commonHelper.CallEndPoint("api/card/process-from-csv", null, Method.Post, processFromCsvRequest);

        // Assert
        Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);

        dynamic responseContent = JsonConvert.DeserializeObject(response.Content);
        Assert.Equal("Csv file content is too large, rather queue this file.", responseContent);
    }
}