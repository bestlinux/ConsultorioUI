namespace ConsultorioUI.Models.DTOs
{
    public class ProntuarioDTO
    {
        public int Id { get; set; }

        public PacienteDTO? Paciente { get; set; }

        public int? PacienteId { get; set; }

        public string? Pagina { get; set; }

        public string? Conteudo { get; set; }
    }
}
