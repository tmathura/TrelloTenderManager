using TrelloTenderManager.WebApp.AutomationTests.Common.Enums;
using TrelloTenderManager.WebApp.AutomationTests.Common.Helpers;
using Xunit.Abstractions;

namespace TrelloTenderManager.WebApp.AutomationTests.Common;

public class SeleniumWebDriverTest(ITestOutputHelper output)
{
    protected readonly ITestOutputHelper Output = output;

    protected IDriverHelper GetWebDriverHelper(WebDriverType type)
    {
        return type switch
        {
            WebDriverType.Chrome => new ChromeDriverHelper(Output),
            _ => throw new Exception("Unsupported WebDriverType")
        };
    }
}