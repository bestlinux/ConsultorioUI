namespace ConsultorioUI.Models.DTOs
{
    public class ErrorDTO
    {
        public string Title { get; set; }

        public List<ErrorItem> Errors { get; set; } = new();
    }
}
