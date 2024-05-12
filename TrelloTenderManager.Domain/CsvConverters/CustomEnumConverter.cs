using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace TrelloTenderManager.Domain.CsvConverters;

public class CustomEnumConverter<TEnum>() : EnumConverter(typeof(TEnum))
{

    public override object? ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
    {
        if (!Enum.TryParse(typeof(TEnum), text, out _)) return default(TEnum);

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