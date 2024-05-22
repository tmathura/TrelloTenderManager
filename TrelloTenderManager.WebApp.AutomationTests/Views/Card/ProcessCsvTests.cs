using OpenQA.Selenium;
using TrelloTenderManager.Tests.Common.Helpers;
using TrelloTenderManager.WebApp.AutomationTests.Common;
using TrelloTenderManager.WebApp.AutomationTests.Common.Enums;
using TrelloTenderManager.WebApp.AutomationTests.MemberData;
using Xunit.Abstractions;

namespace TrelloTenderManager.WebApp.AutomationTests.Views.Card;

public class ProcessCsvTests(ITestOutputHelper output) : SeleniumWebDriverTest(output)
{
    private readonly ITestOutputHelper _output = output;

    [Theory, Trait("Category", "AutomationTests")]
    [MemberData(nameof(ProcessFromCsvMemberData.GetData), MemberType = typeof(ProcessFromCsvMemberData))]
    public void ProcessCsv(string sampleFilename, string sampleFilePath, int expectedCardsCreatedCount, int expectedCardsUpdatedCount)
    {
        // Arrange
        using var helper = GetWebDriverHelper(WebDriverType.Chrome);

        // Act
        helper.GoToStartPage();

        var uploadFile = helper.Driver.FindElement(By.Id("UploadFile"));

        uploadFile.SendKeys(sampleFilePath);

        var btnSubmit = helper.Driver.FindElement(By.Id("btnSubmit"));
        btnSubmit.Click();

        var preSuccessMessage = helper.Driver.FindElement(By.Id("preSuccessMessage"));
        var successMessage = preSuccessMessage.Text;

        _output.WriteLine(successMessage);

        var expectedSuccessMessage = $"The csv file '{sampleFilename}' was processed successfully. {expectedCardsCreatedCount} card/s were created and {expectedCardsUpdatedCount} card/s where updated";
        Assert.Equal(expectedSuccessMessage, successMessage);
    }

    [Fact, Trait("Category", "AutomationTests")]
    public void ProcessCsv_FileTooBig()
    {
        // Arrange
        var sampleFilePath = TestDataHelper.GetSampleFilePath(3);

        using var helper = GetWebDriverHelper(WebDriverType.Chrome);

        // Act
        helper.GoToStartPage();

        var uploadFile = helper.Driver.FindElement(By.Id("UploadFile"));

        uploadFile.SendKeys(sampleFilePath);

        var btnSubmit = helper.Driver.FindElement(By.Id("btnSubmit"));
        btnSubmit.Click();

        var preErrorMessage = helper.Driver.FindElement(By.Id("post-error"));
        var errorMessage = preErrorMessage.Text;

        _output.WriteLine(errorMessage);

        Assert.Equal("\"Csv file content is too large, rather queue this file.\"", errorMessage);
    }
}