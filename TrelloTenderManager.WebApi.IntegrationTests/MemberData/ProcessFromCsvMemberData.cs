using TrelloTenderManager.Tests.Common.Helpers;

namespace TrelloTenderManager.WebApi.IntegrationTests.MemberData;

public class ProcessFromCsvMemberData
{
    /// <summary>
    /// Gets the data for the process from CSV member test cases.
    /// </summary>
    /// <returns>An enumerable collection of object arrays representing the test data.</returns>
    public static IEnumerable<object?[]> GetData()
    {
        // Scenario 1: explain???
        yield return
        [
            TestDataHelper.GetSampleFileFileContent(1), // The content of the file to test with
            1, // The expected cards created count
            0 // The expected cards updated count
        ];

        // Scenario 2: explain???
        yield return
        [
            TestDataHelper.GetSampleFileFileContent(2), // The content of the file to test with
            1, // The expected cards created count
            0 // The expected cards updated count
        ];

        //// Scenario 3: explain???
        //yield return
        //[
        //    TestDataHelper.GetSampleFileFileContent(3), // The content of the file to test with
        //    0, // The expected cards created count
        //    1 // The expected cards updated count
        //];
    }
}
