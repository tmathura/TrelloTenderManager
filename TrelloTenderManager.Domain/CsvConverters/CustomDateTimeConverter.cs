using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace TrelloTenderManager.Domain.CsvConverters;

/// <summary>
/// Custom converter for DateTime values in CSV files.
/// </summary>
public class CustomDateTimeConverter : DateTimeConverter
{
    /// <summary>
    /// Converts a string representation of a DateTime value to a DateTime object.
    /// </summary>
    /// <param name="text">The string representation of the DateTime value.</param>
    /// <param name="row">The current CSV reader row.</param>
    /// <param name="memberMapData">The member map data for the current property.</param>
    /// <returns>The converted DateTime object.</returns>
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (!DateTime.TryParse(text, out _)) return default(DateTime);

        try
        {
            return base.ConvertFromString(text, row, memberMapData);
        }
        catch (TypeConverterException)
        {
            return default(DateTime);
        }
    }
}
