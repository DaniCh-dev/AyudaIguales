using AyudaIguales.Models;

namespace AyudaIguales.Services
{
    public interface IUserService
    {
        Task<RegistroResponse> RegisterUserAsync(RegistroRequest request);
        Task<LoginResponse> LoginUserAsync(LoginRequest request);
    }
}