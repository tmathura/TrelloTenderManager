using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using TrelloTenderManager.Core.Interfaces;

namespace TrelloTenderManager.Core.Implementations
{
    public class CsvHelperWrapper : ICsvHelperWrapper
    {
        public List<T> GetRecords<T>(string fileContent, Type? classMapType)
        {
            using var stringReader = new StringReader(fileContent);
            var csvConfiguration = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null
            };
            using var csvReader = new CsvReader(stringReader, csvConfiguration);

            if (classMapType != null)
            {
                csvReader.Context.RegisterClassMap(classMapType);
            }

            var records = csvReader.GetRecords<T>().ToList();
            
            return records;
        }
    }
}
