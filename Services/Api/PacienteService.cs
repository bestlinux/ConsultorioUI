using ConsultorioUI.Models.DTOs;
using Correios.NET;
using System.Net.Http.Json;
using System.Net.Http;
using System.Net;
using System.Text;
using System;
using System.Reflection.Metadata;
using System.Text.Json;
using AngleSharp.Io;
using System.Runtime.ConstrainedExecution;

namespace ConsultorioUI.Services.Api
{
    public class PacienteService : IPacienteService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ILogger<PacienteService> _logger;
        private const string apiEndpoint = "/api/pacientes/";
        private readonly JsonSerializerOptions _options;

        private PacienteDTO? paciente;
        private List<PacienteDTO>? pacientes;

        public PacienteService(IHttpClientFactory httpClientFactory,
        ILogger<PacienteService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<EnderecoDTO> BuscaCEP(string CEP)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("apiconsultorio");
                var response = await httpClient.GetAsync(apiEndpoint + "buscacep/" + CEP);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<EnderecoDTO>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Erro ao obter o CEP pelo cep= {CEP} - {message}");
                    throw new Exception($"Status Code : {response.StatusCode} - {message}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao buscar o CEP: {apiEndpoint} " + ex.Message);
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<PacienteDTO> CreatePaciente(PacienteDTO pacienteDTO)
        {

            var httpClient = _httpClientFactory.CreateClient("apiconsultorio");

            StringContent content = new(JsonSerializer.Serialize(pacienteDTO),
                                                      Encoding.UTF8, "application/json");

            using (var response = await httpClient.PostAsync(apiEndpoint, content))
            {
                if (response.IsSuccessStatusCode)
                {
                    var apiResponse = await response.Content.ReadAsStreamAsync();

                    paciente = await JsonSerializer
                               .DeserializeAsync<PacienteDTO>(apiResponse, _options);
                }
                else if (response.StatusCode == HttpStatusCode.BadRequest)
                {
                    var message = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Erro ao salvar o paciente pelo cep= {pacienteDTO.Nome} - {message}");
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
            return paciente;
        }

        public async Task<List<PacienteDTO>> GetPacientes()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("apiconsultorio");
                var result = await httpClient.GetFromJsonAsync<List<PacienteDTO>>(apiEndpoint);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar pacientes: {apiEndpoint} " + ex.Message);
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<bool> DeletePaciente(int id)
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

        public async Task<PacienteDTO> UpdatePaciente(PacienteDTO pacienteDTO)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("apiconsultorio");

                PacienteDTO pacienteUpdated = new();

                using (var response = await httpClient.PutAsJsonAsync(apiEndpoint, pacienteDTO))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        pacienteUpdated = await JsonSerializer
                                            .DeserializeAsync<PacienteDTO>(apiResponse, _options);
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

                    return pacienteUpdated;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PacienteDTO> GetPaciente(int id)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("apiconsultorio");
                var response = await httpClient.GetAsync(apiEndpoint + id);

                if (response.IsSuccessStatusCode)
                {
                    var paciente = await response.Content.ReadFromJsonAsync<PacienteDTO>();
                    return paciente;
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Erro ao obter o paciente pelo id= {id} - {message}");
                    throw new Exception($"Status Code : {response.StatusCode} - {message}");
                }
            }
            catch (UnauthorizedAccessException)
            {
                throw new UnauthorizedAccessException();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao obter o paciente pelo id={id} \n\n {ex.Message}");
                throw;
            }
        }
    }
}
