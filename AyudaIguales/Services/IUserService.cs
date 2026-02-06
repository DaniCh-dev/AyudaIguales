using AyudaIguales.Models;

namespace AyudaIguales.Services
{
    public interface IUserService
    {
        Task<RegistroResponse> RegisterUserAsync(RegistroRequest request);
        Task<LoginResponse> LoginUserAsync(LoginRequest request);
        // Nuevo método para obtener todos los usuarios
        Task<ObtenerUsuariosResponse> ObtenerTodosUsuariosAsync(int id_centro);
    }
}