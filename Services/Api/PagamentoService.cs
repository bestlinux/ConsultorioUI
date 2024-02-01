using ConsultorioUI.Models.DTOs;
using ConsultorioUI.Pages.Pacientes;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ConsultorioUI.Services.Api
{
    public class PagamentoService : IPagamentoService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ILogger<PagamentoService> _logger;
        private const string apiEndpoint = "/api/pagamentos/";
        private readonly JsonSerializerOptions _options;

        private PagamentoDTO? pagamento;

        public PagamentoService(IHttpClientFactory httpClientFactory,
        ILogger<PagamentoService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }
        public async Task<PagamentoDTO> CreatePagamento(PagamentoDTO pagamentoDTO)
        {
            var httpClient = _httpClientFactory.CreateClient("apiconsultorio");

            StringContent content = new(JsonSerializer.Serialize(pagamentoDTO),
                                                      Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync(apiEndpoint, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();

                    pagamento = await JsonSerializer
                               .DeserializeAsync<PagamentoDTO>(apiResponse, _options);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Erro ao salvar o pagamento para o paciente = {pagamentoDTO.PacienteId} - {message}");
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
            return pagamento;
        }

        public async Task<List<PagamentoDTO>> GetPagamentos()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("apiconsultorio");
                var result = await httpClient.GetFromJsonAsync<List<PagamentoDTO>>(apiEndpoint);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar os pagamentos: {apiEndpoint} " + ex.Message);
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<List<PagamentoDTO>> GetPagamentosByPacienteMesAno(int PacienteID, int Mes, int Ano)
        {
            try
            {
                var caminho = $"search-pagamentos-paciente-mes-ano?PacienteID={PacienteID}&Mes={Mes}&Ano={Ano}";
                var apiUrl = apiEndpoint + caminho;

                var httpClient = _httpClientFactory.CreateClient("apiconsultorio");
                var result = await httpClient.GetFromJsonAsync<List<PagamentoDTO>>(apiUrl);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar os pagamentos: {apiEndpoint} " + ex.Message);
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<PagamentoDTO> UpdatePagamento(PagamentoDTO pagamentoDTO)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("apiconsultorio");

                PagamentoDTO pagamentoUpdate = new();

                using (var response = await httpClient.PutAsJsonAsync(apiEndpoint, pagamentoDTO))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        pagamentoUpdate = await JsonSerializer
                                            .DeserializeAsync<PagamentoDTO>(apiResponse, _options);
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

                    return pagamentoUpdate;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeletePagamento(int id)
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
