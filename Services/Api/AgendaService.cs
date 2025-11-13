using ConsultorioUI.Models.DTOs;
using ConsultorioUI.Pages.Pacientes;
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
        private readonly JsonSerializerOptions _options;

        private AgendaDTO? agenda;

        public AgendaService(IHttpClientFactory httpClientFactory,
        ILogger<AgendaService> logger,
        Settings settings)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<AgendaDTO>> GetAgendas()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("ConsultorioPsicoFunctions");

                var response = await httpClient.GetAsync("api/GetAllAgenda");


                var conteudo = await response.Content.ReadAsStringAsync();

                if (conteudo != null && conteudo != "")
                {
                    var result = JsonSerializer.Deserialize<List<AgendaDTO>>
                                     (await response.Content.ReadAsStringAsync(),
                                     new JsonSerializerOptions
                                     {
                                         PropertyNameCaseInsensitive = false
                                     });

                    return result!;
                }
                else
                {
                    return new List<AgendaDTO>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar as agendas: " + ex.Message);
                throw new UnauthorizedAccessException();
            }
        }
        public async Task<AgendaDTO> UpdateAgenda(AgendaDTO agendaDTO)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("ConsultorioPsicoFunctions");

                AgendaDTO? agendaUpdated = new();

                using (var response = await httpClient.PutAsJsonAsync("api/UpdateAgenda", agendaDTO))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        agendaUpdated = await JsonSerializer
                                            .DeserializeAsync<AgendaDTO>(apiResponse!, _options);
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        throw new UnauthorizedAccessException();
                    }
                    else if (response.StatusCode == HttpStatusCode.BadRequest)
                    {
                        var errorMessage = string.Empty;
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        var erro = JsonSerializer.Deserialize<ErrorResponse>
                               (await response.Content.ReadAsStringAsync(),
                               new JsonSerializerOptions
                               {
                                   PropertyNameCaseInsensitive = false
                               });

                        if (erro != null)
                        {
                            foreach (var item in erro.Error.Errors)
                            {
                                errorMessage = System.String.Concat(item.Message, Environment.NewLine);
                            }
                        }

                        throw new Exception(errorMessage);
                    }

                    return agendaUpdated!;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<AgendaDTO> CreateAgenda(AgendaDTO agendaDTO)
        {
            var httpClient = _httpClientFactory.CreateClient("ConsultorioPsicoFunctions");

            var response = await httpClient.PostAsJsonAsync("api/CreateAgenda", agendaDTO);

            var conteudo = await response.Content.ReadAsStringAsync();

            var agenda = JsonSerializer.Deserialize<AgendaDTO>
                             (await response.Content.ReadAsStringAsync(),
                             new JsonSerializerOptions
                             {
                                 PropertyNameCaseInsensitive = false
                             });

            if (response.IsSuccessStatusCode)
            {
                return agenda!;
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Erro ao salvar a agenda");
                throw new Exception($"Status Code : {response.StatusCode} - {message}");
            }
        }

        public async Task<bool> DeleteAgenda(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ConsultorioPsicoFunctions");

            var response = await httpClient.DeleteAsync("api/DeleteAgenda?Id=" + id);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorMessage = string.Empty;

                var erro = JsonSerializer.Deserialize<ErrorResponse>
                             (await response.Content.ReadAsStringAsync(),
                             new JsonSerializerOptions
                             {
                                 PropertyNameCaseInsensitive = false
                             });

                if (erro != null)
                {
                    foreach (var item in erro.Error.Errors)
                    {
                        errorMessage = System.String.Concat(item.Message, Environment.NewLine);
                    }
                }
                throw new Exception(errorMessage);
            }

            return true;
        }

        public async Task<bool> DeleteAgendaRecorrencia(int? pacienteID)
        {
            var httpClient = _httpClientFactory.CreateClient("ConsultorioPsicoFunctions");

            var response = await httpClient.DeleteAsync("api/DeleteAgendaByRecorrencia?Id=" + pacienteID);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorMessage = string.Empty;

                var erro = JsonSerializer.Deserialize<ErrorResponse>
                             (await response.Content.ReadAsStringAsync(),
                             new JsonSerializerOptions
                             {
                                 PropertyNameCaseInsensitive = false
                             });

                if (erro != null)
                {
                    foreach (var item in erro.Error.Errors)
                    {
                        errorMessage = System.String.Concat(item.Message, Environment.NewLine);
                    }
                }
                throw new Exception(errorMessage);
            }

            return true;
        }

        public async Task<bool> DeleteAgendaPessoalRecorrencia(int? categoriaAgendamento)
        {
            var httpClient = _httpClientFactory.CreateClient("ConsultorioPsicoFunctions");

            var response = await httpClient.DeleteAsync("api/DeleteAgendaPessoalByRecorrencia?Id=" + categoriaAgendamento);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorMessage = string.Empty;

                var erro = JsonSerializer.Deserialize<ErrorResponse>
                             (await response.Content.ReadAsStringAsync(),
                             new JsonSerializerOptions
                             {
                                 PropertyNameCaseInsensitive = false
                             });

                if (erro != null)
                {
                    foreach (var item in erro.Error.Errors)
                    {
                        errorMessage = System.String.Concat(item.Message, Environment.NewLine);
                    }
                }
                throw new Exception(errorMessage);
            }

            return true;
        }
    }
}
