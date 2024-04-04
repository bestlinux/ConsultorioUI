using ConsultorioUI.Models.DTOs;

namespace ConsultorioUI.Services.Api
{
    public interface IAgendaService
    {
        Task<AgendaDTO> CreateAgenda(AgendaDTO pagamento);
        
        Task<AgendaDTO> UpdateAgenda(AgendaDTO agendaDTO);

        Task<List<AgendaDTO>> GetAgendas();

        Task<bool> DeleteAgenda(int id);
    }
}
