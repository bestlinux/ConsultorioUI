using System.Text.Json.Serialization;

namespace ConsultorioUI.Models
{
    public class ErrorItem
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("tag")]
        public string Tag { get; set; }
    }
}
