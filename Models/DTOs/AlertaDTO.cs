namespace ConsultorioUI.Models.DTOs
{
    public class AlertaDTO
    {
        public int Id { get; set; }

        public string? Descricao { get; set; }

        //1 - ANIVERSARIO
        //2 - PAGAMENTOS
        public int? Categoria { get; set; }
    }
}
