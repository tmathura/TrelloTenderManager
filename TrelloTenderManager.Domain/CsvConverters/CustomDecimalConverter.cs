using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace TrelloTenderManager.Domain.CsvConverters;

/// <summary>
/// Custom decimal converter for CSVHelper library.
/// </summary>
public class CustomDecimalConverter : DecimalConverter
{
    /// <summary>
    /// Converts a string representation of a decimal to its decimal equivalent.
    /// </summary>
    /// <param name="text">The string to convert.</param>
    /// <param name="row">The current reader row.</param>
    /// <param name="memberMapData">The member map data.</param>
    /// <returns>The decimal equivalent of the input string, or the default decimal value if the conversion fails.</returns>
    public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (!decimal.TryParse(text, out _)) return default(decimal);

        try
        {
            return base.ConvertFromString(text, row, memberMapData);
        }
        catch (TypeConverterException)
        {
            return default(decimal);
        }
    }
}
