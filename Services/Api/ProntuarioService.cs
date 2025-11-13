using ConsultorioUI.Models.DTOs;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using static ConsultorioUI.Pages.Pagamentos.Pagamentos;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsultorioUI.Services.Api
{
    public class ProntuarioService : IProntuarioService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ILogger<ProntuarioService> _logger;
        private readonly JsonSerializerOptions _options;

        private ProntuarioDTO? pronturario;

        public ProntuarioService(IHttpClientFactory httpClientFactory,
        ILogger<ProntuarioService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<List<ProntuarioDTO>> GetProntuariosByPaciente(int PacienteID)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("ConsultorioPsicoFunctions");
                var response = await httpClient.GetAsync("api/GetProntuarioByPaciente?Id=" + PacienteID);

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<List<ProntuarioDTO>>
                                    (await response.Content.ReadAsStringAsync(),
                                    new JsonSerializerOptions
                                    {
                                        PropertyNameCaseInsensitive = false
                                    });

                    return result!;
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Erro ao obter o pronturario pelo id= {PacienteID} - {message}");
                    throw new Exception($"Status Code : {response.StatusCode} - {message}");
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter o pronturario pelo id={PacienteID} \n\n {ex.Message}");
                throw;
            }
        }

        public async Task<ProntuarioDTO> UpdateProntuario(ProntuarioDTO prontuarioDTO)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("ConsultorioPsicoFunctions");

                ProntuarioDTO prontuarioUpdate = new();

                using (var response = await httpClient.PutAsJsonAsync("api/UpdateProntuario", prontuarioDTO))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        prontuarioUpdate = await JsonSerializer
                                            .DeserializeAsync<ProntuarioDTO>(apiResponse, _options);
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

                    return prontuarioUpdate;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
