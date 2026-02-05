using System.Text.Json;
using AyudaIguales.Models;

namespace AyudaIguales.Services
{
    public class CentroService : ICentroService
    {
        private readonly HttpClient _client;

        public CentroService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<bool> CreateCentro(string nombre, string cif)
        {
            var data = new { nombre, cif };
            var response = await _client.PostAsJsonAsync("createCentro.php", data);

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();

                try
                {
                    var errorJson = JsonSerializer.Deserialize<JsonElement>(errorContent);

                    if (errorJson.TryGetProperty("msg", out JsonElement msgElement))
                    {
                        throw new Exception(msgElement.GetString());
                    }
                }
                catch (JsonException)
                {
                    // Si no es JSON válido, mostrar el contenido completo
                }

                throw new Exception(errorContent);
            }

            return true;
        }

        public async Task<List<Centro>> ObtenerCentrosAsync()
        {
            try
            {
                // Obtener centros desde el endpoint PHP
                var response = await _client.GetAsync("getCentros.php");

                if (!response.IsSuccessStatusCode)
                {
                    return new List<Centro>();
                }

                var content = await response.Content.ReadAsStringAsync();

                // Deserializar respuesta
                var result = JsonSerializer.Deserialize<CentrosResponse>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return result?.centros ?? new List<Centro>();
            }
            catch (Exception)
            {
                return new List<Centro>();
            }
        }
    }

    // Clase auxiliar para deserializar la respuesta de centros
    public class CentrosResponse
    {
        public bool ok { get; set; }
        public List<Centro> centros { get; set; }
    }
}