using ConsultorioUI.Models.DTOs;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ConsultorioUI.Services.Api
{
    public class AgendaService : IAgendaService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ILogger<AgendaService> _logger;
        private const string apiEndpoint = "/api/agendas/";
        private readonly JsonSerializerOptions _options;

        private AgendaDTO? agenda;

        public AgendaService(IHttpClientFactory httpClientFactory,
        ILogger<AgendaService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<AgendaDTO>> GetAgendas()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("apiconsultorio");
                var result = await httpClient.GetFromJsonAsync<List<AgendaDTO>>(apiEndpoint);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar as agendas: {apiEndpoint} " + ex.Message);
                throw new UnauthorizedAccessException();
            }
        }
        public async Task<AgendaDTO> UpdateAgenda(AgendaDTO agendaDTO)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("apiconsultorio");

                AgendaDTO agendaUpdate = new();

                using (var response = await httpClient.PutAsJsonAsync(apiEndpoint, agendaDTO))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        agendaUpdate = await JsonSerializer
                                            .DeserializeAsync<AgendaDTO>(apiResponse, _options);
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException();
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var errorMessage = string.Empty;
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        ErrorDto erro = await JsonSerializer
                                            .DeserializeAsync<ErrorDto>(apiResponse, _options);

                        foreach (var item in erro.Errors)
                        {
                            errorMessage = String.Concat(item.Message, Environment.NewLine);
                        }
                        throw new Exception(errorMessage);
                    }

                    return agendaUpdate;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<AgendaDTO> CreateAgenda(AgendaDTO agenda)
        {
            var httpClient = _httpClientFactory.CreateClient("apiconsultorio");

            StringContent content = new(JsonSerializer.Serialize(agenda),
                                                      Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync(apiEndpoint, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();

                    agenda = await JsonSerializer
                               .DeserializeAsync<AgendaDTO>(apiResponse, _options);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Erro ao salvar o agendamento para o paciente = {agenda.PacienteNome} - {message}");
                    throw new Exception($"Status Code : {response.StatusCode} - {message}");
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException();
                }
                else
                {
                    return null;
                }
            }
            return agenda;
        }

        public async Task<bool> DeleteAgenda(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("apiconsultorio");

            using (var response = await httpClient.DeleteAsync(apiEndpoint + id))
            {
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    throw new UnauthorizedAccessException();
                }
            }
            return false;
        }
    }
}
