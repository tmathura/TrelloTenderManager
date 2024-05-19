namespace TrelloTenderManager.Tests.Common.Helpers;

public static class TestDataHelper
{
    private static readonly DirectoryInfo CurrentDirectory = new(Environment.CurrentDirectory);

    /// <summary>
    /// Gets the content of the sample file for the specified test scenario.
    /// </summary>
    /// <param name="testScenario">The test scenario number.</param>
    /// <returns>The content of the sample file.</returns>
    public static string GetSampleFileFileContent(int testScenario)
    {
        return File.ReadAllText(Path.Combine($@"{CurrentDirectory}\Test Files\Sample Csv Files", $"Sample File - Scenario{testScenario}.csv"));
    }
}