using ConsultorioUI.Models.DTOs;

namespace ConsultorioUI.Services.Api
{
    public interface IAlertaService
    {
        Task<List<AlertaDTO>> GetAlertasByMesAno(int Mes, int Ano);
    }
}
