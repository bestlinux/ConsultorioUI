namespace ConsultorioUI.Models
{
    public class TipoPagamento(string Id, string pagamento)
    {
        public string Id { get; set; } = Id;

        public string Pagamento { get; set; } = pagamento;
    }
}
