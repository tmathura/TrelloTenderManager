using Newtonsoft.Json;

namespace TrelloTenderManager.Domain.Models.CustomFields;

/// <summary>
/// Represents a custom field item.
/// </summary>
public class CustomFieldItem
{
    /// <summary>
    /// Gets or sets the ID of the custom field.
    /// </summary>
    [JsonProperty("idCustomField")]
    public string? IdCustomField { get; set; }

    /// <summary>
    /// Gets or sets the value of the custom field.
    /// </summary>
    [JsonProperty("value")]
    public CustomFieldValue? Value { get; set; }
}
