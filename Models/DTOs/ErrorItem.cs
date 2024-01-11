using System.Text.Json.Serialization;

namespace ConsultorioUI.Models.DTOs
{
    public class ErrorItem
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("tag")]
        public string Tag { get; set; }
    }
}
