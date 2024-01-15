namespace ConsultorioUI.Models.DTOs
{
    public class PagamentoDTO
    {
        public int? Status { get; set; }

        public double? Valor { get; set; }

        public string? Observacao { get; set; }

        public int? Mes { get; set; }

        public int? PacienteId { get; set; }

        public int Ano { get; set; }
    }
}
