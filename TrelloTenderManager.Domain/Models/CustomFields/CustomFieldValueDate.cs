using Newtonsoft.Json;

namespace TrelloTenderManager.Domain.Models.CustomFields;

public class CustomFieldValueDate : CustomFieldValue
{
    /// <summary>
    /// Gets or sets the date value.
    /// </summary>
    [JsonProperty("date")]
    public string? Date { get; set; }
}
