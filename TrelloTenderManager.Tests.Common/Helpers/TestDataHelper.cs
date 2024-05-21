namespace TrelloTenderManager.Tests.Common.Helpers;

public static class TestDataHelper
{
    private static readonly DirectoryInfo CurrentDirectory = new(Environment.CurrentDirectory);

    public static string GetSampleFilename(int testScenario)
    {
        var path = Path.Combine($@"{CurrentDirectory}\Test Files\Sample Csv Files", $"Sample File - Scenario{testScenario}.csv");

        return Path.GetFileName(path);
    }

    /// <summary>
    /// Gets the content of the sample file for the specified test scenario.
    /// </summary>
    /// <param name="testScenario">The test scenario number.</param>
    /// <returns>The content of the sample file.</returns>
    public static string GetSampleFileFileContent(int testScenario)
    {
        return File.ReadAllText(Path.Combine($@"{CurrentDirectory}\Test Files\Sample Csv Files", $"Sample File - Scenario{testScenario}.csv"));
    }

    /// <summary>
    /// Gets the expected valid accumulated data.
    /// </summary>
    /// <param name="testScenario">The test scenario to get the data for.</param>
    /// <returns>A string of the expected valid accumulated data </returns>
    public static string[] GetSampleFileFileContentLines(int testScenario)
    {
        return File.ReadAllLines(Path.Combine($@"{CurrentDirectory}\Test Files\Sample Csv Files", $"Sample File - Scenario{testScenario}.csv"));
    }
}