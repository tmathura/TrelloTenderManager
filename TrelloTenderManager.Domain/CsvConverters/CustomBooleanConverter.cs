using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace TrelloTenderManager.Domain.CsvConverters;

public class CustomBooleanConverter : BooleanConverter
{
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