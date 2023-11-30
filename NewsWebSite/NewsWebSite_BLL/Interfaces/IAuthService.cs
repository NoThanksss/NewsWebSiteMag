using NewsWebSite_BLL.Models;

namespace NewsWebSite_BLL.Interfaces
{
    public interface IAuthService
    {
        Task<AuthResponse> LoginAsync(LoginModel model);
        Task<AuthResponse> RegisterAsync(RegisterModel model);

    }
}
