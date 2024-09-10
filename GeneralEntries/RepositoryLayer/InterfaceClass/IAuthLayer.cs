using GeneralEntries.DTOs;
using GeneralEntries.Helpers.Response;
using GeneralEntries.ViewModel;

namespace GeneralEntries.RepositoryLayer.InterfaceClass;

public interface IAuthLayer
{
    Task<ServiceResponse<string>> Login(string email, string password);
    Task<ServiceResponse<LoginDto>> LoginModel(LoginViewModel login);

    Task<TokenViewModel> Auth(LoginViewModel loginModel);

    Task<ServiceResponse<RegisterDto>> RegisterUser(RegisterViewModel model);

    Task LogoutAsync();

    Task<ServiceResponse<string>> ChangePasswordAsync(ChangePasswordViewModel model, string email);

    Task<TokenViewModel> GetRefreshToken(GetRefreshTokenViewModel model);
}
