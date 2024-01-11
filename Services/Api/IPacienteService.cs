using ConsultorioUI.Models.DTOs;

namespace ConsultorioUI.Services.Api
{
    public interface IPacienteService
    {
        Task<PacienteDTO> CreatePaciente(PacienteDTO pacienteDTO);

        Task<EnderecoDTO> BuscaCEP(string CEP);

        Task<List<PacienteDTO>> GetPacientes();

        Task<PacienteDTO> UpdatePaciente(PacienteDTO pacienteDTO);

        Task<PacienteDTO> GetPaciente(int id);

        Task<bool> DeletePaciente(int id);
    }
}
