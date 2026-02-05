using AyudaIguales.Models;

namespace AyudaIguales.Services
{
    public interface IUserService
    {
        Task<RegistroResponse> RegisterUserAsync(RegistroRequest request);
        Task<Usuario?> LoginUserAsync(string nombre_usuario, string password);
    }
}