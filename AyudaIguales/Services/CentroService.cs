using System.Text.Json;

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

                // Intentar extraer el mensaje del JSON de error
                try
                {
                    var errorJson = JsonSerializer.Deserialize<JsonElement>(errorContent);

                    // Si hay un campo "msg" en el JSON, usarlo
                    if (errorJson.TryGetProperty("msg", out JsonElement msgElement))
                    {
                        throw new Exception(msgElement.GetString());
                    }
                }
                catch (JsonException)
                {
                    // Si no es JSON válido, mostrar el contenido completo
                }

                // Si no se pudo extraer el mensaje, mostrar el error completo
                throw new Exception(errorContent);
            }

            return true;
        }
    }
}