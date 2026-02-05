using AyudaIguales.Models;
using System.Text.RegularExpressions;

namespace AyudaIguales.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;

        public UserService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<RegistroResponse> RegisterUserAsync(RegistroRequest request)
        {
            // Validaciones en el servidor ASP.NET
            if (string.IsNullOrWhiteSpace(request.nombre_usuario))
                return new RegistroResponse { ok = false, msg = "El nombre de usuario es obligatorio" };

            if (string.IsNullOrWhiteSpace(request.correo))
                return new RegistroResponse { ok = false, msg = "El correo es obligatorio" };

            if (string.IsNullOrWhiteSpace(request.password))
                return new RegistroResponse { ok = false, msg = "La contraseña es obligatoria" };

            if (request.id_centro <= 0)
                return new RegistroResponse { ok = false, msg = "Debe seleccionar un centro" };

            // Validar formato de correo
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            if (!emailRegex.IsMatch(request.correo))
                return new RegistroResponse { ok = false, msg = "El formato del correo electrónico no es válido" };

            // Validar formato de contraseña
            var passwordRegex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
            if (!passwordRegex.IsMatch(request.password))
                return new RegistroResponse { ok = false, msg = "La contraseña debe tener mínimo 8 caracteres, una mayúscula, una minúscula, un número y un carácter especial (@$!%*?&)" };

            try
            {
                // Preparar datos para enviar al PHP
                var data = new
                {
                    nombre_usuario = request.nombre_usuario,
                    correo = request.correo,
                    password = request.password,
                    id_centro = request.id_centro.ToString(),
                    rol = request.rol
                };

                // Enviar petición al endpoint PHP
                var response = await _client.PostAsJsonAsync("registUser.php", data);

                // Leer respuesta
                var result = await response.Content.ReadFromJsonAsync<RegistroResponse>();

                return result ?? new RegistroResponse { ok = false, msg = "Error al procesar la respuesta" };
            }
            catch (Exception ex)
            {
                return new RegistroResponse { ok = false, msg = $"Error de conexión: {ex.Message}" };
            }
        }

        public async Task<Usuario?> LoginUserAsync(string nombre_usuario, string password)
        {
            var data = new
            {
                nombre_usuario,
                password
            };

            var response = await _client.PostAsJsonAsync("api/usuarios/login", data);

            if (!response.IsSuccessStatusCode)
            {
                // Recoger el texto de error que devuelve PHP
                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception(errorText);
            }

            return await response.Content.ReadFromJsonAsync<Usuario>();
        }
    }
}