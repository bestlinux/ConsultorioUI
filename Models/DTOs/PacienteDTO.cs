using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ConsultorioUI.Models.DTOs
{
    public class PacienteDTO
    {
        public int Id { get; set; }
        public string? Nome { get; set; }

        public int Sexo { get; set; }

        public DateTime DataNascimento { get; set; }

        public string Telefone { get; set; }

        public string Email { get; set; }

        public string CPF { get; set; }

        public int? Pais { get; set; }

        public string? CEP { get; set; }

        public string? Cidade { get; set; }

        public string? Estado { get; set; }

        public string? Bairro { get; set; }

        public string? Logradouro { get; set; }
        public string? NumeroLogradouro { get; set; }

        public string? Complemento { get; set; }

        //MENSAL
        //AVULSO
        //GRATUITO
        public int? TipoPagamento { get; set; }

        public double? ValorSessao { get; set; }

        public int? DiaVencimento { get; set; }

        public int? StatusPagamento { get; set; }

        public int? Ativo { get; set; }
        /*[Required(ErrorMessage = "O campo Cidade é obrigatório")]
        public string Cidade { get; set; }*/

    }
}
