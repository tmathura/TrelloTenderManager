using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Interfaces
{
    public interface ITenderCsvParser
    {
        List<Tender> Parse(string fileContent);
    }
}