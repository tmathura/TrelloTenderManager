using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using TrelloTenderManager.WebApp.AutomationTests.Common.Models;
using Xunit.Abstractions;

namespace TrelloTenderManager.WebApp.AutomationTests.Common.Helpers;

public abstract class DriverHelper : IDriverHelper
{
    protected readonly int DefaultWaitTimeoutSeconds = 60;
    protected readonly string Url;
    protected readonly ITestOutputHelper Output;

    protected DriverHelper(ITestOutputHelper output)
    {
        this.Output = output;

        var config = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        var options = new DriverHelperOptions();
        config.Bind(options);
        Driver = CreateWebDriver(options.WebDriversOptions!);
        Wait = CreateWebDriverWait(Driver, options.WebDriversOptions!);
        Url = options.WebAppOptions?.Url!;
    }

    public IWebDriver Driver { get; }

    public WebDriverWait Wait { get; }

    protected abstract IWebDriver CreateWebDriver(WebDriversOptions options);

    protected abstract WebDriverWait CreateWebDriverWait(IWebDriver driver, WebDriversOptions options);

    public void GoToStartPage()
    {
        Driver.Navigate().GoToUrl(Url);
    }

    public IWebElement FindElement(By by, TimeSpan timeout)
    {
        if (timeout == default)
        {
            return Driver.FindElement(by);
        }

        return Wait.Until(driver => driver.FindElement(by));
    }

    public void Dispose()
    {
        Driver.Close();
    }
}