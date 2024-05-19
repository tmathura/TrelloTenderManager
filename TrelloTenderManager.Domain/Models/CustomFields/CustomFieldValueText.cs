using Newtonsoft.Json;

namespace TrelloTenderManager.Domain.Models.CustomFields;

public class CustomFieldValueText : CustomFieldValue
{
    /// <summary>
    /// Gets or sets the text value.
    /// </summary>
    [JsonProperty("text")]
    public string? Text { get; set; }
}
