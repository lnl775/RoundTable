using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using UmbracoBridge.ValidationAttributes;

namespace UmbracoBridge.Models
{
    public class DocumentType
    {
        [JsonPropertyName("alias")]
        [Required(ErrorMessage = "Alias is required.")]
        public string Alias { get; set; }

        [JsonPropertyName("name")]
        [Required(ErrorMessage = "Name is required.")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [JsonPropertyName("icon")]
        [IconStartsWith(ErrorMessage = "Icon must start with 'icon-'.")]
        public string Icon { get; set; }

        [JsonPropertyName("allowedAsRoot")]
        public bool AllowedAsRoot { get; set; }

        [JsonPropertyName("variesByCulture")]
        public bool VariesByCulture { get; set; }

        [JsonPropertyName("variesBySegment")]
        public bool VariesBySegment { get; set; }

        [JsonPropertyName("collection")]
        public object? Collection { get; set; }

        [JsonPropertyName("isElement")]
        public bool IsElement { get; set; }
    }
}
