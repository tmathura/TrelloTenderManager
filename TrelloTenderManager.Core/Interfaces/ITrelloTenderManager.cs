namespace TrelloTenderManager.Core.Interfaces;

public interface ITrelloTenderManager
{
    /// <summary>
    /// Populates Trello cards from the content of a CSV file.
    /// </summary>
    /// <param name="fileContent">The content of the CSV file.</param>
    Task PopulateCardsFromCsvFileContent(string fileContent);
}