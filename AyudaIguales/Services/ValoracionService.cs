using AyudaIguales.Models;

namespace AyudaIguales.Services
{
    public class ValoracionService : IValoracionService
    {
        private readonly HttpClient _client;

        public ValoracionService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("ApiClient");
        }

        // Crear una nueva valoracion
        public async Task<CrearValoracionResponse> CrearValoracionAsync(CrearValoracionRequest request)
        {
            // Validaciones del lado del cliente
            if (request.id_respuesta <= 0)
                return new CrearValoracionResponse { ok = false, msg = "Respuesta no válida" };

            if (request.id_usuario <= 0)
                return new CrearValoracionResponse { ok = false, msg = "Usuario no válido" };

            if (request.nota < 1 || request.nota > 5)
                return new CrearValoracionResponse { ok = false, msg = "La nota debe estar entre 1 y 5" };

            try
            {
                var data = new
                {
                    id_respuesta = request.id_respuesta,
                    id_usuario = request.id_usuario,
                    nota = request.nota
                };

                var response = await _client.PostAsJsonAsync("createValoracion.php", data);
                var result = await response.Content.ReadFromJsonAsync<CrearValoracionResponse>();

                return result ?? new CrearValoracionResponse { ok = false, msg = "Error al procesar la respuesta" };
            }
            catch (Exception ex)
            {
                return new CrearValoracionResponse { ok = false, msg = $"Error de conexión: {ex.Message}" };
            }
        }

    }
}
