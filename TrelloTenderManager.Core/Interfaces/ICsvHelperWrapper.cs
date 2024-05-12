namespace TrelloTenderManager.Core.Interfaces;

public interface ICsvHelperWrapper
{
    List<T> GetRecords<T>(string fileContent, Type? classMapType);
}