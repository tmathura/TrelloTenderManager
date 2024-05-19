using TrelloTenderManager.Domain.Models;

namespace TrelloTenderManager.Core.Implementations;

/// <summary>
/// Provides methods to validate a list of tenders.
/// </summary>
public static class TenderParser
{

    /// <summary>
    /// Filters a list of tenders to remove duplicates.
    /// </summary>
    /// <param name="tenders">The list of tenders to filter.</param>
    public static List<Tender> Filter(List<Tender> tenders)
    {
        var filteredTenders = tenders.GroupBy(t => new { t.TenderId, t.LotNumber })
            .Select(g => g.Last())
            .ToList();

        return filteredTenders;

    }

    /// <summary>
    /// Validates a list of tenders and returns the validation result.
    /// </summary>
    /// <param name="tenders">The list of tenders to validate.</param>
    /// <returns>The validation result containing the valid and invalid tenders.</returns>
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

    /// <summary>
    /// Validates a single tender.
    /// </summary>
    /// <param name="tender">The tender to validate.</param>
    /// <returns><c>true</c> if the tender is valid; otherwise, <c>false</c>.</returns>
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
