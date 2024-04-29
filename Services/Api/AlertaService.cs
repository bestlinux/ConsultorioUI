using ConsultorioUI.Models.DTOs;
using ConsultorioUI.Pages.Pacientes;
using System.Net.Http.Json;
using System.Text.Json;

namespace ConsultorioUI.Services.Api
{
    public class AlertaService : IAlertaService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ILogger<AlertaService> _logger;
        private const string apiEndpoint = "/api/alertas/";
        private readonly JsonSerializerOptions _options;

        private AlertaDTO? alerta;

        public AlertaService(IHttpClientFactory httpClientFactory,
        ILogger<AlertaService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<AlertaDTO>> GetAlertasByMesAno(int Mes, int Ano)
        {
            try
            {
                var caminho = $"search-alertas-mes-ano?Mes={Mes}&Ano={Ano}";
                var apiUrl = apiEndpoint + caminho;

                var httpClient = _httpClientFactory.CreateClient("apiconsultorio");
                var result = await httpClient.GetFromJsonAsync<List<AlertaDTO>>(apiUrl);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar os alertas: {apiEndpoint} " + ex.Message);
                throw new UnauthorizedAccessException();
            }
        }
    }
}
