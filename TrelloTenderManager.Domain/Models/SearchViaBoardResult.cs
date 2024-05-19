using System.Text.Json.Serialization;

namespace TrelloTenderManager.Domain.Models;

public class SearchViaBoardResult
{
    /// <summary>
    /// Gets or sets the ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string? Id { get; set; }

    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    [JsonPropertyName("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Gets or sets the description.
    /// </summary>
    [JsonPropertyName("desc")]
    public string? Desc { get; set; }
}
