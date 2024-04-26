using ConsultorioUI.Models.DTOs;

namespace ConsultorioUI.Services.Api
{
    public interface IProntuarioService
    {
        Task<List<ProntuarioDTO>> GetProntuariosByPaciente(int PacienteID);

        Task<ProntuarioDTO> UpdateProntuario(ProntuarioDTO prontuario);
    }
}
