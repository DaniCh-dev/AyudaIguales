using AyudaIguales.Models;
using System.Text.Json;

namespace AyudaIguales.Services
{
    public class AyudaService : IAyudaService
    {
        private readonly HttpClient _client;

        public AyudaService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("ApiClient");
        }

        // Obtener todas las ayudas
        public async Task<ObtenerAyudasResponse> ObtenerAyudasAsync()
        {
            try
            {
                // Llamar al endpoint PHP
                var response = await _client.GetAsync("getAyudas.php");
                var result = await response.Content.ReadFromJsonAsync<ObtenerAyudasResponse>();

                return result ?? new ObtenerAyudasResponse { ok = false, msg = "Error al procesar la respuesta" };
            }
            catch (Exception ex)
            {
                return new ObtenerAyudasResponse { ok = false, msg = $"Error de conexión: {ex.Message}" };
            }
        }

        // Obtener ayudas con filtros
        public async Task<ObtenerAyudasResponse> ObtenerAyudasConFiltrosAsync(FiltrosAyuda filtros)
        {
            try
            {
                // Preparar datos para enviar al PHP
                var data = new
                {
                    busqueda = filtros.busqueda ?? "",
                    estado = filtros.estado ?? "",
                    id_usuario = filtros.id_usuario?.ToString() ?? "",
                    fecha = filtros.fecha ?? "",
                    respuestas = filtros.respuestas ?? ""
                };

                // Enviar petición al endpoint PHP
                var response = await _client.PostAsJsonAsync("filterAyudas.php", data);
                var result = await response.Content.ReadFromJsonAsync<ObtenerAyudasResponse>();

                return result ?? new ObtenerAyudasResponse { ok = false, msg = "Error al procesar la respuesta" };
            }
            catch (Exception ex)
            {
                return new ObtenerAyudasResponse { ok = false, msg = $"Error de conexión: {ex.Message}" };
            }
        }

        // Crear una nueva ayuda
        public async Task<CrearAyudaResponse> CrearAyudaAsync(CrearAyudaRequest request)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(request.descripcion))
                return new CrearAyudaResponse { ok = false, msg = "La descripción es obligatoria" };

            if (string.IsNullOrWhiteSpace(request.contenido))
                return new CrearAyudaResponse { ok = false, msg = "El contenido es obligatorio" };

            if (request.id_usuario <= 0)
                return new CrearAyudaResponse { ok = false, msg = "Usuario no válido" };

            try
            {
                // Preparar datos para enviar al PHP
                var data = new
                {
                    id_usuario = request.id_usuario.ToString(),
                    descripcion = request.descripcion,
                    contenido = request.contenido
                };

                // Enviar petición al endpoint PHP
                var response = await _client.PostAsJsonAsync("createAyuda.php", data);
                var result = await response.Content.ReadFromJsonAsync<CrearAyudaResponse>();

                return result ?? new CrearAyudaResponse { ok = false, msg = "Error al procesar la respuesta" };
            }
            catch (Exception ex)
            {
                return new CrearAyudaResponse { ok = false, msg = $"Error de conexión: {ex.Message}" };
            }
        }

        // Obtener ayuda por ID
        public async Task<Ayuda> ObtenerAyudaPorIdAsync(int id)
        {
            try
            {
                // Llamar al endpoint PHP
                var response = await _client.GetAsync($"getAyuda.php?id={id}");
                var jsonString = await response.Content.ReadAsStringAsync();

                using var jsonDoc = JsonDocument.Parse(jsonString);
                var root = jsonDoc.RootElement;

                if (!root.TryGetProperty("ok", out var okElement) || !okElement.GetBoolean())
                {
                    return null;
                }

                if (!root.TryGetProperty("ayuda", out var ayudaElement))
                {
                    return null;
                }

                // Crear objeto ayuda manualmente
                var ayuda = new Ayuda
                {
                    id = ayudaElement.GetProperty("id").GetInt32(),
                    id_usuario = ayudaElement.GetProperty("id_usuario").GetInt32(),
                    descripcion = ayudaElement.GetProperty("descripcion").GetString(),
                    contenido = ayudaElement.GetProperty("contenido").GetString(),
                    activa = ayudaElement.GetProperty("activa").GetBoolean(),
                    fecha = DateTime.Parse(ayudaElement.GetProperty("fecha").GetString()),
                    nombre_usuario = ayudaElement.TryGetProperty("nombre_usuario", out var nombreElement)
                        ? nombreElement.GetString()
                        : "",
                    num_respuestas = ayudaElement.TryGetProperty("num_respuestas", out var numRespElement)
                        ? numRespElement.GetInt32()
                        : 0
                };

                return ayuda;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}