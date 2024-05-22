using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using TrelloTenderManager.WebApp.AutomationTests.Common.Models;
using Xunit.Abstractions;

namespace TrelloTenderManager.WebApp.AutomationTests.Common.Helpers;

public class ChromeDriverHelper(ITestOutputHelper output) : DriverHelper(output)
{
    private static readonly DirectoryInfo CurrentDirectory = new(Environment.CurrentDirectory);
    public static string DownloadLocation { get; } = Path.Combine($"{CurrentDirectory}", @"Test Files\Downloads");

    /// <summary>
    /// Create the web driver with the supplied options.
    /// </summary>
    /// <param name="options">The web driver options.</param>
    /// <returns>A <see cref="ChromeDriver"/></returns>
    protected override IWebDriver CreateWebDriver(WebDriversOptions options)
    {
        var chromeOptions = new ChromeOptions();

        if (options?.Chrome?.AcceptInsecureCertificates ?? false)
        {
            chromeOptions.AcceptInsecureCertificates = options?.Chrome?.AcceptInsecureCertificates;
        }

        if (!string.IsNullOrWhiteSpace(options?.Chrome?.WindowSize))
        {
            chromeOptions.AddArgument($"window-size={options?.Chrome?.WindowSize}");
        }

        if (options?.Chrome?.DisableGpu ?? false)
        {
            chromeOptions.AddArgument("disable-gpu");
        }

        if (options?.Chrome?.DisableExtensions ?? false)
        {
            chromeOptions.AddArgument("disable-extensions");
        }

        chromeOptions.AddArgument("start-maximized");

        if (options?.Chrome?.Headless ?? false)
        {
            chromeOptions.AddArgument("headless");
        }

        chromeOptions.AddUserProfilePreference("download.default_directory", DownloadLocation);

        return new ChromeDriver(chromeOptions);
    }

    /// <summary>
    /// Wait for a specific amount of time.
    /// </summary>
    /// <param name="driver">The driver to use.</param>
    /// <param name="options">The web driver options.</param>
    /// <returns></returns>
    protected override WebDriverWait CreateWebDriverWait(IWebDriver driver, WebDriversOptions options)
    {
        return new WebDriverWait(Driver, TimeSpan.FromSeconds(options?.Chrome?.WaitTimeoutSeconds ?? DefaultWaitTimeoutSeconds));
    }
}