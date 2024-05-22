namespace TrelloTenderManager.WebApp.AutomationTests.Common.Models;

public class WebDriverOptions
{
    public bool Headless { get; set; }
    public bool AcceptInsecureCertificates { get; set; }
    public int WaitTimeoutSeconds { get; set; }
    public string? WindowSize { get; set; }
    public bool DisableGpu { get; set; }
    public bool DisableExtensions { get; set; }
}