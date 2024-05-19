using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace TrelloTenderManager.Domain.CsvConverters;

/// <summary>
/// Custom converter for enumerations.
/// </summary>
/// <typeparam name="TEnum">The enumeration type.</typeparam>
public class CustomEnumConverter<TEnum>() : EnumConverter(typeof(TEnum))
{
    /// <summary>
    /// Converts a string representation of an enumeration value to its corresponding enumeration object.
    /// </summary>
    /// <param name="text">The string representation of the enumeration value.</param>
    /// <param name="row">The current CSV reader row.</param>
    /// <param name="memberMapData">The member map data.</param>
    /// <returns>The enumeration object.</returns>
    public override object? ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (!Enum.TryParse(typeof(TEnum), text, out _))
            return default(TEnum);

        try
        {
            return base.ConvertFromString(text, row, memberMapData);
        }
        catch (TypeConverterException)
        {
            return default(TEnum);
        }
    }
}
