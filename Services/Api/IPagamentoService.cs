using ConsultorioUI.Models.DTOs;

namespace ConsultorioUI.Services.Api
{
    public interface IPagamentoService
    {
        Task<PagamentoDTO> CreatePagamento(PagamentoDTO pagamento);

        Task<List<PagamentoDTO>> GetPagamentos();

        Task<List<PagamentoDTO>> GetPagamentosByPacienteMesAno(int PacienteID, int Mes, int Ano);

        Task<PagamentoDTO> UpdatePagamento(PagamentoDTO pagamento);

        Task<bool> DeletePagamento (int PagamentoID);
    }
}
