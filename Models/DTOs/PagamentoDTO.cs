namespace ConsultorioUI.Models.DTOs
{
    public class PagamentoDTO
    {
        public int Id { get; set; }

        //1 - PAGO
        //2 - ABERTO
        public int StatusPagamento { get; set; }

        public double? Valor { get; set; }

        public string? Observacao { get; set; }

        public int? Mes { get; set; }

        public int? PacienteId { get; set; }

        public int Ano { get; set; }

        public PacienteDTO? Paciente { get; set; }
    }
}
