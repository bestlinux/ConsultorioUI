using System.Text.Json.Serialization;

namespace ConsultorioUI.Models.DTOs
{
    public class ErrorResponse
    {
        public bool? Success { get; set; }
        public ErrorDTO Error { get; set; }
    }
}
