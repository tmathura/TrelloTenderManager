using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using TrelloTenderManager.Core.Interfaces;

namespace TrelloTenderManager.Core.Implementations;

/// <summary>
/// Wrapper class for CsvHelper library.
/// </summary>
public class CsvHelperWrapper : ICsvHelperWrapper
{
    /// <inheritdoc />
    public List<T> GetRecords<T>(string? fileContent, Type? classMapType)
    {
        using var stringReader = new StringReader(fileContent);
        var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
        {
            MissingFieldFound = null
        };
        using var csvReader = new CsvReader(stringReader, csvConfiguration);

        if (classMapType is not null)
        {
            csvReader.Context.RegisterClassMap(classMapType);
        }

        var records = csvReader.GetRecords<T>().ToList();

        return records;
    }
}
