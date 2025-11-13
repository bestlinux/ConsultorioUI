using ConsultorioUI.Models;
using ConsultorioUI.Models.DTOs;
using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection.Metadata;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Text.Json;
using static ConsultorioUI.Pages.Pagamentos.Pagamentos;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConsultorioUI.Services.Api
{
    public class PacienteService : IPacienteService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ILogger<PacienteService> _logger;
        private readonly JsonSerializerOptions _options;

        private PacienteDTO? paciente;
        public PacienteService(IHttpClientFactory httpClientFactory,
        ILogger<PacienteService> logger)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        }

        public async Task<PacienteDTO> CreatePaciente(PacienteDTO pacienteDTO)
        {
            var httpClient = _httpClientFactory.CreateClient("ConsultorioPsicoFunctions");

            var response = await httpClient.PostAsJsonAsync("api/CreatePaciente", pacienteDTO);

            var conteudo = await response.Content.ReadAsStringAsync();

            var paciente = JsonSerializer.Deserialize<PacienteDTO>
                             (await response.Content.ReadAsStringAsync(),
                             new JsonSerializerOptions
                             {
                                 PropertyNameCaseInsensitive = false
                             });

            if (response.IsSuccessStatusCode)
            {
                return paciente!;
            }
            else
            {
                var message = await response.Content.ReadAsStringAsync();
                _logger.LogError($"Erro ao salvar o paciente pelo nome= {pacienteDTO.Nome} - {message}");
                throw new Exception($"Status Code : {response.StatusCode} - {message}");
            }
        }

        public async Task<List<PacienteDTO>> GetPacientes()
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("ConsultorioPsicoFunctions");
                var response = await httpClient.GetAsync("api/GetAllPacientes");

                var conteudo = await response.Content.ReadAsStringAsync();

                if (conteudo != null && conteudo != "")
                {
                    var result = JsonSerializer.Deserialize<List<PacienteDTO>>
                                     (await response.Content.ReadAsStringAsync(),
                                     new JsonSerializerOptions
                                     {
                                         PropertyNameCaseInsensitive = false
                                     });

                    return result!;
                }
                else
                {
                    return new List<PacienteDTO>();
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"Erro ao acessar pacientes: " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> DeletePaciente(int id)
        {
            var httpClient = _httpClientFactory.CreateClient("ConsultorioPsicoFunctions");

            var response = await httpClient.DeleteAsync("api/DeletePaciente?Id=" + id);

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

        public async Task<PacienteDTO> UpdatePaciente(PacienteDTO pacienteDTO)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("ConsultorioPsicoFunctions");

                PacienteDTO? pacienteUpdated = new();

                using (var response = await httpClient.PutAsJsonAsync("api/UpdatePaciente", pacienteDTO))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponse = await response.Content.ReadAsStreamAsync();
                        pacienteUpdated = await JsonSerializer
                                            .DeserializeAsync<PacienteDTO>(apiResponse!, _options);
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

                    return pacienteUpdated!;
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
                var httpClient = _httpClientFactory.CreateClient("ConsultorioPsicoFunctions");
                var response = await httpClient.GetAsync("api/GetPacienteById?Id=" + id);

                if (response.IsSuccessStatusCode)
                {
                    var paciente = await response.Content.ReadFromJsonAsync<PacienteDTO>();
                    return paciente!;
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
