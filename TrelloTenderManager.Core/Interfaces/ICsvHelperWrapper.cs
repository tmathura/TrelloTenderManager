namespace TrelloTenderManager.Core.Interfaces;

public interface ICsvHelperWrapper
{
    /// <summary>
    /// Gets the records from a CSV file.
    /// </summary>
    /// <typeparam name="T">The type of the records.</typeparam>
    /// <param name="fileContent">The content of the CSV file.</param>
    /// <param name="classMapType">The type of the class map to be used for mapping the CSV fields to the object properties.</param>
    /// <returns>A list of records of type T.</returns>
    List<T> GetRecords<T>(string fileContent, Type? classMapType);
}