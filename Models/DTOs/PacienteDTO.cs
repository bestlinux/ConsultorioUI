using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ConsultorioUI.Models.DTOs
{
    public class PacienteDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O campo nome é obrigatório")]
        [MinLength(3)]
        [MaxLength(200)]
        [DisplayName("Nome")]
        public string? Nome { get; set; }

    }
}
