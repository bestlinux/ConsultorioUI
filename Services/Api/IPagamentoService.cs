using ConsultorioUI.Models.DTOs;

namespace ConsultorioUI.Services.Api
{
    public interface IPagamentoService
    {
        Task<PagamentoDTO> CreatePaciente(PagamentoDTO pagamento);
    }
}
