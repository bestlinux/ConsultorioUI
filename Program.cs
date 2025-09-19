using Blazored.LocalStorage;
using ConsultorioUI;
using ConsultorioUI.Services.Api;
using ConsultorioUI.Services.Autentica;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var settings = new Settings();

builder.Configuration.Bind(settings);
builder.Services.AddSingleton(settings);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddHttpClient("apiconsultorio", options =>
{
    //options.BaseAddress = new Uri("https://localhost:7020/");
    options.BaseAddress = new Uri("https://consultoriopsicoapi-cfffcrh2e9dbfuf4.canadacentral-01.azurewebsites.net"); //APIGateway - Ocelot
}).AddHttpMessageHandler<CustomHttpHandler>();

builder.Services.AddScoped<CustomHttpHandler>();

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();
builder.Services.AddRadzenComponents();

var supportedCultures = new[]
{
    new System.Globalization.CultureInfo("pt-BR"),
};

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("pt-BR");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});

//AUTORIZACAO
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddScoped<IAuthService, AuthService>();

//SERVICOS NEGOCIO
builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IPagamentoService, PagamentoService>();
builder.Services.AddScoped<IAgendaService, AgendaService>();
builder.Services.AddScoped<IProntuarioService, ProntuarioService>();
builder.Services.AddScoped<IAlertaService, AlertaService>();

CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");
CultureInfo.DefaultThreadCurrentUICulture = new CultureInfo("pt-BR");

var app = builder.Build().RunAsync();
