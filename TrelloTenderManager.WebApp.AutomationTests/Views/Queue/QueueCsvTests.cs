using OpenQA.Selenium;
using TrelloTenderManager.Tests.Common.Helpers;
using TrelloTenderManager.WebApp.AutomationTests.Common;
using TrelloTenderManager.WebApp.AutomationTests.Common.Enums;
using Xunit.Abstractions;

namespace TrelloTenderManager.WebApp.AutomationTests.Views.Queue;

public class QueueCsvTests(ITestOutputHelper output) : SeleniumWebDriverTest(output)
{
    private readonly ITestOutputHelper _output = output;

    [Fact, Trait("Category", "AutomationTests")]
    public void QueueCsv()
    {
        // Arrange
        var sampleFilename = TestDataHelper.GetSampleFilename(1);
        var sampleFilePath = TestDataHelper.GetSampleFilePath(1);

        using var helper = GetWebDriverHelper(WebDriverType.Chrome);

        // Act
        helper.GoToStartPage();

        var linkQueueCsv = helper.Driver.FindElement(By.Id("linkQueueCsv"));
        linkQueueCsv.Click();

        var uploadFile = helper.Driver.FindElement(By.Id("UploadFile"));

        uploadFile.SendKeys(sampleFilePath);

        var btnSubmit = helper.Driver.FindElement(By.Id("btnSubmit"));
        btnSubmit.Click();

        var preSuccessMessage = helper.Driver.FindElement(By.Id("preSuccessMessage"));
        var successMessage = preSuccessMessage.Text;

        _output.WriteLine(successMessage);

        Assert.Equal($"The csv file '{sampleFilename}' was queued successfully.", successMessage);
    }
}