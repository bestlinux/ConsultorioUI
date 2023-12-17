using ConsultorioUI.Models;

namespace ConsultorioUI.Services.Autentica;

public interface IAuthService
{
    Task<LoginResult> Login(LoginModel loginModel);

    Task Logout();
}
