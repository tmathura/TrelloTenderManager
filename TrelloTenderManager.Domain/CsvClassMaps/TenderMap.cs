using CsvHelper.Configuration;
using TrelloTenderManager.Domain.CsvConverters;
using TrelloTenderManager.Domain.Enums;
using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Domain.CsvClassMaps;

public sealed class TenderMap : ClassMap<Tender>
{
    public TenderMap()
    {
        Map(tender => tender.Id).TypeConverter<CustomGuidConverter>();
        Map(tender => tender.TenderId).TypeConverter<CustomGuidConverter>();
        Map(tender => tender.LotNumber);
        Map(tender => tender.Deadline).TypeConverter<CustomDateTimeConverter>();
        Map(tender => tender.Name);
        Map(tender => tender.TenderName);
        Map(tender => tender.ExpirationDate).TypeConverter<CustomDateTimeConverter>();
        Map(tender => tender.HasDocuments).TypeConverter<CustomBooleanConverter>();
        Map(tender => tender.Location);
        Map(tender => tender.PublicationDate).TypeConverter<CustomDateTimeConverter>();
        Map(tender => tender.Status).TypeConverter<CustomEnumConverter<TenderStatus>>();
        Map(tender => tender.Currency);
        Map(tender => tender.Value).TypeConverter<CustomDecimalConverter>();
        Map(tender => tender.ImportData).Convert(convertFromStringArgs => convertFromStringArgs.Row.Context.Parser.RawRecord);
    }
}