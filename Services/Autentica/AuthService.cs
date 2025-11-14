using Blazored.LocalStorage;
using ConsultorioUI.Models;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace ConsultorioUI.Services.Autentica;

public class AuthService : IAuthService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly AuthenticationStateProvider _authenticationStateProvider;
    private readonly ILocalStorageService _localStorage;
    private readonly IConfiguration _configuration;

    public AuthService(IHttpClientFactory httpClientFactory, 
        AuthenticationStateProvider authenticationStateProvider,
        ILocalStorageService localStorage,
        IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _authenticationStateProvider = authenticationStateProvider;
        _localStorage = localStorage;
        _configuration = configuration;
    }

    public async Task<LoginResult> Login(LoginModel loginModel)
    {
        try
        {

            var key = _configuration["FunctionKey"];

            var httpClient = _httpClientFactory.CreateClient("ConsultorioPsicoFunctions");


            var response = await httpClient.PostAsJsonAsync("api/login?code=" + key, loginModel);
            //var response = await httpClient.PostAsJsonAsync("api/login", loginModel);


            var conteudo = await response.Content.ReadAsStringAsync();

            var loginResult = JsonSerializer.Deserialize<LoginResult>
                             (await response.Content.ReadAsStringAsync(),
                             new JsonSerializerOptions
                             {
                               PropertyNameCaseInsensitive = false
                             });

            if (!response.IsSuccessStatusCode)
            {
                return loginResult;
            }

            await _localStorage.SetItemAsync("authToken", loginResult.Token);
            await _localStorage.SetItemAsync("tokenExpiration", loginResult.Expiration);

            ((ApiAuthenticationStateProvider)_authenticationStateProvider)
                                .MarkUserAsAuthenticated(loginModel.Email);

            httpClient.DefaultRequestHeaders.Authorization = 
                        new AuthenticationHeaderValue("Bearer",
                                                         loginResult.Token);

            return loginResult;

        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public async Task Logout()
    {
        var httpClient = _httpClientFactory.CreateClient("apiconsultorio");
        await _localStorage.RemoveItemAsync("authToken");

        ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
        httpClient.DefaultRequestHeaders.Authorization = null;
    }
}
