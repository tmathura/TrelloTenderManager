using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace TrelloTenderManager.Domain.CsvConverters;

/// <summary>
/// Custom converter for Guid values in CSV files.
/// </summary>
public class CustomGuidConverter : GuidConverter
{
    /// <summary>
    /// Converts a string representation of a Guid to a Guid object.
    /// </summary>
    /// <param name="text">The string representation of the Guid.</param>
    /// <param name="row">The current CSV reader row.</param>
    /// <param name="memberMapData">The member map data for the Guid property.</param>
    /// <returns>The Guid object.</returns>
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (!Guid.TryParse(text, out _)) return default(Guid);

        try
        {
            return base.ConvertFromString(text, row, memberMapData);
        }
        catch (TypeConverterException)
        {
            return default(Guid);
        }
    }
}
