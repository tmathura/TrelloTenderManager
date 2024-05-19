using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace TrelloTenderManager.Domain.CsvConverters;

/// <summary>
/// Custom converter for converting string to int.
/// </summary>
public class CustomIntConverter : Int32Converter
{
    /// <summary>
    /// Converts a string to an int.
    /// </summary>
    /// <param name="text">The string to convert.</param>
    /// <param name="row">The current reader row.</param>
    /// <param name="memberMapData">The member map data.</param>
    /// <returns>The converted int value.</returns>
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (!int.TryParse(text, out _)) return default(int);

        try
        {
            return base.ConvertFromString(text, row, memberMapData);
        }
        catch (TypeConverterException)
        {
            return default(int);
        }
    }
}
