using ConsultorioUI.Models.DTOs;

namespace ConsultorioUI.Services.Api
{
    public interface IPacienteService
    {
        Task<PacienteDTO> CreatePaciente(PacienteDTO pacienteDTO);
    }
}
