using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace TrelloTenderManager.Domain.CsvConverters;

public class CustomIntConverter : Int32Converter
{
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