using AyudaIguales.Models;
using System.Text.Json;
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

        public async Task<LoginResponse> LoginUserAsync(LoginRequest request)
        {
            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(request.nombre_usuario))
                return new LoginResponse { ok = false, msg = "El nombre de usuario es obligatorio" };

            if (string.IsNullOrWhiteSpace(request.password))
                return new LoginResponse { ok = false, msg = "La contraseña es obligatoria" };

            try
            {
                var data = new
                {
                    nombre_usuario = request.nombre_usuario,
                    password = request.password
                };

                // Enviar petición al endpoint PHP
                var response = await _client.PostAsJsonAsync("loginUser.php", data);
                var jsonString = await response.Content.ReadAsStringAsync();

                // Deserializar manualmente para manejar el enum
                using var jsonDoc = JsonDocument.Parse(jsonString);
                var root = jsonDoc.RootElement;

                // Verificar si la respuesta es exitosa
                if (!root.TryGetProperty("ok", out var okElement) || !okElement.GetBoolean())
                {
                    var msgElement = root.TryGetProperty("msg", out var msg) ? msg.GetString() : "Error desconocido";
                    return new LoginResponse { ok = false, msg = msgElement };
                }

                // Obtener el objeto usuario
                if (!root.TryGetProperty("usuario", out var usuarioElement))
                {
                    return new LoginResponse { ok = false, msg = "No se recibió información del usuario" };
                }

                // Convertir rol string a enum
                var rolString = usuarioElement.GetProperty("rol").GetString();
                RolUsuario rolEnum;

                // Intentar parsear el rol, si falla usar 'usuario' por defecto
                if (!Enum.TryParse<RolUsuario>(rolString, true, out rolEnum))
                {
                    rolEnum = RolUsuario.usuario;
                }

                // Crear objeto usuario manualmente
                var usuario = new Usuario
                {
                    id = usuarioElement.GetProperty("id").GetInt32(),
                    nombre_usuario = usuarioElement.GetProperty("nombre_usuario").GetString(),
                    correo = usuarioElement.GetProperty("correo").GetString(),
                    rol = rolEnum,
                    id_centro = usuarioElement.GetProperty("id_centro").GetInt32()
                };

                var mensajeResponse = root.TryGetProperty("msg", out var msgResp) ? msgResp.GetString() : "Login exitoso";

                return new LoginResponse
                {
                    ok = true,
                    msg = mensajeResponse,
                    usuario = usuario
                };
            }
            catch (Exception ex)
            {
                return new LoginResponse { ok = false, msg = $"Error de conexión: {ex.Message}" };
            }
        }
        // Obtener todos los usuarios (para filtros de admin)
        public async Task<ObtenerUsuariosResponse> ObtenerTodosUsuariosAsync()
        {
            try
            {
                // Llamar al endpoint PHP
                var response = await _client.GetAsync("getUsuarios.php");
                var result = await response.Content.ReadFromJsonAsync<ObtenerUsuariosResponse>();

                return result ?? new ObtenerUsuariosResponse { ok = false, msg = "Error al procesar la respuesta" };
            }
            catch (Exception ex)
            {
                return new ObtenerUsuariosResponse { ok = false, msg = $"Error de conexión: {ex.Message}" };
            }
        }
    }
}