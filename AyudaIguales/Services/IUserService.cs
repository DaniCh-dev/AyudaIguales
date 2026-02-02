using AyudaIguales.Models;

namespace AyudaIguales.Services
{
    public interface IUserService
    {
        Task<bool>  RegisterUserAsync(string nombre_usuario, string password, string correo, int id_centro, string rol);
        Task<Usuario?> LoginUserAsync(string nombre_usuario, string password);


    }
}
