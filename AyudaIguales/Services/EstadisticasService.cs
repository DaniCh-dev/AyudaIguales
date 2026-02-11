using AyudaIguales.Models;

namespace AyudaIguales.Services
{
    public class EstadisticasService : IEstadisticasService
    {
        private readonly HttpClient _client;

        public EstadisticasService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("ApiClient");
        }

        // Obtener estadisticas
        public async Task<ObtenerEstadisticasResponse> ObtenerEstadisticasAsync(int id_usuario, int id_centro, string rol)
        {
            try
            {
                var response = await _client.GetAsync($"getEstadisticas.php?id_usuario={id_usuario}&id_centro={id_centro}&rol={rol}");
                var result = await response.Content.ReadFromJsonAsync<ObtenerEstadisticasResponse>();

                return result ?? new ObtenerEstadisticasResponse { ok = false, msg = "Error al procesar la respuesta" };
            }
            catch (Exception ex)
            {
                return new ObtenerEstadisticasResponse { ok = false, msg = $"Error de conexión: {ex.Message}" };
            }
        }
    }
}