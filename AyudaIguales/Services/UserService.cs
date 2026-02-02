using AyudaIguales.Models;

namespace AyudaIguales.Services
{
    public class UserService:IUserService
    {
        private readonly HttpClient _client;

        public UserService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<bool> RegisterUserAsync(string nombre_usuario, string password, string correo,int id_centro, string rol)
        {
            var data = new
            {
                nombre_usuario,
                password,
                correo,
                id_centro,
                rol
            };
            var response = await _client.PostAsJsonAsync("registUser.php", data);
            if (!response.IsSuccessStatusCode)
            {
                // Recoger el texto de error que devuelve PHP
                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception(errorText);
            }
            return response.IsSuccessStatusCode;
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
