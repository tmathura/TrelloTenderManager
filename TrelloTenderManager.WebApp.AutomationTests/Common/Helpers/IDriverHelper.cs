using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TrelloTenderManager.WebApp.AutomationTests.Common.Helpers;

public interface IDriverHelper : IDisposable
{
    IWebDriver Driver { get; }
    WebDriverWait Wait { get; }
    void GoToStartPage();
    IWebElement FindElement(By by, TimeSpan timeout);
}