using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace TrelloTenderManager.Domain.CsvConverters;

public class CustomDateTimeConverter : DateTimeConverter
{
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