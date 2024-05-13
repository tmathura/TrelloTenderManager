using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Implementations;

public static class TenderValidator
{
    public static TenderValidationResult Validate(List<Tender> tenders)
    {
        List<Tender> validTenders = [];
        List<Tender> invalidTenders = [];

        foreach (var tender in tenders)
        {
            if (Validate(tender))
            {
                validTenders.Add(tender);
            }
            else
            {
                invalidTenders.Add(tender);
            }
        }

        var tenderValidationResult = new TenderValidationResult(validTenders, invalidTenders);

        return tenderValidationResult;
    }

    private static bool Validate(Tender? tender)
    {
        if (tender is null)
        {
            return false;
        }

        if (tender.Id == Guid.Empty)
        {
            return false;
        }

        if (tender.TenderId == Guid.Empty)
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(tender.Name))
        {
            return false;
        }

        if (string.IsNullOrWhiteSpace(tender.TenderName))
        {
            return false;
        }

        return true;
    }
}