using System.Text.Json.Serialization;

namespace UmbracoBridge.Models
{
    public class HealthCheckGroup
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }
        [JsonPropertyName("items")]
        public Item[]? Items { get; set; }
    }
    public class Item
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
    }
}
