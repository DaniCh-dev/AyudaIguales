namespace AyudaIguales.Services
{
    public class CentroService
    {
        private readonly HttpClient _client;

        public CentroService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("ApiClient");
        }

        public async Task<bool> CreateCentro(string nombre, string cif)
        {
            var data = new
            {
                nombre,
                cif
            };
            var response = await _client.PostAsJsonAsync("registCentro.php", data);
            if (!response.IsSuccessStatusCode)
            {
                // Recoger el texto de error que devuelve PHP
                var errorText = await response.Content.ReadAsStringAsync();
                throw new Exception(errorText);
            }
            return response.IsSuccessStatusCode;
        }
    }
}
