using Newtonsoft.Json;

namespace TrelloTenderManager.Domain.Models.CustomFields;

public class CustomFieldValueNumber : CustomFieldValue
{
    /// <summary>
    /// Gets or sets the number value.
    /// </summary>
    [JsonProperty("number")]
    public string? Number { get; set; }
}
