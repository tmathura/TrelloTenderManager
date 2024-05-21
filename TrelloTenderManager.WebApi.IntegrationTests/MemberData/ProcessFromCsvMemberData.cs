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
        // Scenario 1: One line of data
        yield return
        [
            TestDataHelper.GetSampleFileFileContentLines(1), // The content of the file to test with
            1, // The expected cards created count
            0 // The expected cards updated count
        ];

        // Scenario 2: Two lines of data, same tender id and lot number - should call create once
        yield return
        [
            TestDataHelper.GetSampleFileFileContentLines(2), // The content of the file to test with
            1, // The expected cards created count
            0 // The expected cards updated count
        ];
    }
}
