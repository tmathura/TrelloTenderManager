using Newtonsoft.Json;

namespace TrelloTenderManager.Domain.Models.CustomFields;

public class CustomFieldValueChecked : CustomFieldValue
{
    /// <summary>
    /// Gets or sets the checked value.
    /// </summary>
    [JsonProperty("checked")]
    public string? Checked { get; set; }
}
