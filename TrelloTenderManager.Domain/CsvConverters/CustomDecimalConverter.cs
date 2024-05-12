using CsvHelper;
using CsvHelper.Configuration;
using CsvHelper.TypeConversion;

namespace TrelloTenderManager.Domain.CsvConverters;

public class CustomDecimalConverter : DecimalConverter
{
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