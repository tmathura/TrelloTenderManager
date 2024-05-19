using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace TrelloTenderManager.Domain.CsvConverters;

/// <summary>
/// Custom boolean converter for CSV parsing.
/// </summary>
public class CustomBooleanConverter : BooleanConverter
{
    /// <summary>
    /// Converts a string representation of a boolean value to a boolean object.
    /// </summary>
    /// <param name="text">The string representation of the boolean value.</param>
    /// <param name="row">The current CSV reader row.</param>
    /// <param name="memberMapData">The member map data for the current property.</param>
    /// <returns>The converted boolean object.</returns>
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (!bool.TryParse(text, out _)) return default(bool);

        try
        {
            return base.ConvertFromString(text, row, memberMapData);
        }
        catch (TypeConverterException)
        {
            return default(bool);
        }
    }
}
