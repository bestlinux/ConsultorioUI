namespace ConsultorioUI.Models
{
    public class StatusPagamento(int Id, string status)
    {
        public int Id { get; set; } = Id;

        public string Status { get; set; } = status;
    }
}
