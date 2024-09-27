namespace ConsultorioUI.Models.DTOs
{
    public class EmailAgendaDTO
    {
        public string? PacienteNome { get; set; }

        public string? PacienteEmail { get; set; }

        public DateTime? InicioSessao { get; set; }

        public DateTime? FimSessao { get; set; }

        public int? NumeroRecorrencias { get; set; }

        public int? TipoRecorrencia { get; set; }
    }
}
