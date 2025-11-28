using ConsultorioUI.Models;
using ConsultorioUI.Models.DTOs;
using ConsultorioUI.Pages.Pacientes;
using ConsultorioUI.Services.Autentica;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace ConsultorioUI.Services.Api
{
    public class AlertaService : IAlertaService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ILogger<AlertaService> _logger;
        private readonly JsonSerializerOptions _options;
        private readonly IConfiguration _configuration;
        public AlertaService(IHttpClientFactory httpClientFactory,
        ILogger<AlertaService> logger,
        IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            _configuration = configuration;
        }

        public async Task<List<AlertaDTO>> GetAlertasByMesAno(int Mes, int Ano)
        {
            try
            {
                var key = _configuration["FunctionKey"];

                var httpClient = _httpClientFactory.CreateClient("ConsultorioPsicoFunctions");

                var response = await httpClient.GetAsync("api/GetAllAlertasByMesAno?code=" + key + "&Mes=" + Mes + "&Ano=" + Ano);

                var conteudo = await response.Content.ReadAsStringAsync();

                if (conteudo != null && conteudo != "")
                {
                    var result = JsonSerializer.Deserialize<List<AlertaDTO>>
                                     (await response.Content.ReadAsStringAsync(),
                                     new JsonSerializerOptions
                                     {
                                         PropertyNameCaseInsensitive = false
                                     });

                    return result!;
                }
                else
                {
                    return new List<AlertaDTO>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar os alertas: " + ex.Message);
                throw new UnauthorizedAccessException();
            }
        }
    }
}
