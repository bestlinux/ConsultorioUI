namespace ConsultorioUI.Models
{
    public class StatusPaciente(int Id, string status)
    {
        public int Id { get; set; } = Id;

        public string Status { get; set; } = status;
    }
}
