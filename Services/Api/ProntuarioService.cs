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
        private const string apiEndpoint = "/api/prontuarios/";
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
                var caminho = $"search-prontuarios-paciente?PacienteID={PacienteID}";
                var apiUrl = apiEndpoint + caminho;

                var httpClient = _httpClientFactory.CreateClient("apiconsultorio");
                var result = await httpClient.GetFromJsonAsync<List<ProntuarioDTO>>(apiUrl);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar os prontuarios: {apiEndpoint} " + ex.Message);
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<ProntuarioDTO> UpdateProntuario(ProntuarioDTO prontuarioDTO)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("apiconsultorio");

                ProntuarioDTO prontuarioUpdate  = new();

                using (var response = await httpClient.PutAsJsonAsync(apiEndpoint, prontuarioDTO))
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
                        ErrorDto erro = await JsonSerializer
                                            .DeserializeAsync<ErrorDto>(apiResponse, _options);

                        foreach (var item in erro.Errors)
                        {
                            errorMessage = System.String.Concat(item.Message, Environment.NewLine);
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
