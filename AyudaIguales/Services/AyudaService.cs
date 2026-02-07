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

        // Obtener todas las ayudas del centro
        public async Task<ObtenerAyudasResponse> ObtenerAyudasAsync(int id_centro)
        {
            try
            {
                var response = await _client.GetAsync($"getAyudas.php?id_centro={id_centro}");
                var result = await response.Content.ReadFromJsonAsync<ObtenerAyudasResponse>();

                return result ?? new ObtenerAyudasResponse { ok = false, msg = "Error al procesar la respuesta" };
            }
            catch (Exception ex)
            {
                return new ObtenerAyudasResponse { ok = false, msg = $"Error de conexión: {ex.Message}" };
            }
        }

        // Obtener ayudas con filtros del centro
        public async Task<ObtenerAyudasResponse> ObtenerAyudasConFiltrosAsync(FiltrosAyuda filtros, int id_centro)
        {
            try
            {
                var data = new
                {
                    busqueda = filtros.busqueda ?? "",
                    estado = filtros.estado ?? "",
                    id_usuario = filtros.id_usuario?.ToString() ?? "",
                    fecha = filtros.fecha ?? "",
                    respuestas = filtros.respuestas ?? "",
                    id_centro = id_centro
                };

                var response = await _client.PostAsJsonAsync("filterAyudas.php", data);
                var result = await response.Content.ReadFromJsonAsync<ObtenerAyudasResponse>();

                return result ?? new ObtenerAyudasResponse { ok = false, msg = "Error al procesar la respuesta" };
            }
            catch (Exception ex)
            {
                return new ObtenerAyudasResponse { ok = false, msg = $"Error de conexión: {ex.Message}" };
            }
        }

        // Crear una nueva ayuda con soporte de imagenes
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
                // Crear contenido multipart si hay imagenes, sino JSON
                if (request.imagenes != null && request.imagenes.Any())
                {
                    // Usar MultipartFormDataContent para enviar archivos
                    using var formData = new MultipartFormDataContent();

                    // Agregar datos basicos como campos de texto
                    formData.Add(new StringContent(request.id_usuario.ToString()), "id_usuario");
                    formData.Add(new StringContent(request.descripcion), "descripcion");
                    formData.Add(new StringContent(request.contenido), "contenido");

                    // Agregar cada imagen
                    foreach (var imagen in request.imagenes)
                    {
                        if (imagen.Length > 0)
                        {
                            var streamContent = new StreamContent(imagen.OpenReadStream());
                            streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(imagen.ContentType);
                            formData.Add(streamContent, "imagenes[]", imagen.FileName);
                        }
                    }

                    // Enviar peticion con archivos
                    var response = await _client.PostAsync("createAyuda.php", formData);

                    // Leer la respuesta como string primero para depurar
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Intentar parsear como JSON
                    try
                    {
                        var result = await response.Content.ReadFromJsonAsync<CrearAyudaResponse>();
                        return result ?? new CrearAyudaResponse { ok = false, msg = "Error al procesar la respuesta" };
                    }
                    catch
                    {
                        // Si falla el parseo, devolver el contenido crudo para debug
                        return new CrearAyudaResponse { ok = false, msg = $"Respuesta inválida del servidor: {responseContent}" };
                    }
                }
                else
                {
                    // Si no hay imagenes, enviar JSON normal - CAMBIO AQUI: id_usuario como numero
                    var data = new
                    {
                        id_usuario = request.id_usuario, // Cambio: quitar .ToString()
                        descripcion = request.descripcion,
                        contenido = request.contenido
                    };

                    var response = await _client.PostAsJsonAsync("createAyuda.php", data);

                    // Leer la respuesta como string primero para depurar
                    var responseContent = await response.Content.ReadAsStringAsync();

                    // Intentar parsear como JSON
                    try
                    {
                        var result = await response.Content.ReadFromJsonAsync<CrearAyudaResponse>();
                        return result ?? new CrearAyudaResponse { ok = false, msg = "Error al procesar la respuesta" };
                    }
                    catch
                    {
                        // Si falla el parseo, devolver el contenido crudo para debug
                        return new CrearAyudaResponse { ok = false, msg = $"Respuesta inválida del servidor: {responseContent}" };
                    }
                }
            }
            catch (Exception ex)
            {
                return new CrearAyudaResponse { ok = false, msg = $"Error de conexión: {ex.Message}" };
            }
        }

        // Obtener ayuda por ID del centro
        public async Task<Ayuda?> ObtenerAyudaPorIdAsync(int id, int id_centro)
        {
            try
            {
                var response = await _client.GetAsync($"getAyuda.php?id={id}&id_centro={id_centro}");
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

                var fechaString = ayudaElement.GetProperty("fecha").GetString();
                if (string.IsNullOrEmpty(fechaString))
                {
                    return null;
                }

                // Crear objeto ayuda con imagenes
                var ayuda = new Ayuda
                {
                    id = ayudaElement.GetProperty("id").GetInt32(),
                    id_usuario = ayudaElement.GetProperty("id_usuario").GetInt32(),
                    descripcion = ayudaElement.GetProperty("descripcion").GetString() ?? "",
                    contenido = ayudaElement.GetProperty("contenido").GetString() ?? "",
                    activa = ayudaElement.GetProperty("activa").GetBoolean(),
                    fecha = DateTime.Parse(fechaString),
                    nombre_usuario = ayudaElement.TryGetProperty("nombre_usuario", out var nombreElement)
                        ? nombreElement.GetString() ?? ""
                        : "",
                    num_respuestas = ayudaElement.TryGetProperty("num_respuestas", out var numRespElement)
                        ? numRespElement.GetInt32()
                        : 0
                };

                // Cargar imagenes si existen
                if (ayudaElement.TryGetProperty("imagenes", out var imagenesElement) && imagenesElement.ValueKind == JsonValueKind.Array)
                {
                    ayuda.imagenes = new List<string>();
                    foreach (var img in imagenesElement.EnumerateArray())
                    {
                        var ruta = img.GetString();
                        if (!string.IsNullOrEmpty(ruta))
                        {
                            ayuda.imagenes.Add(ruta);
                        }
                    }
                }

                return ayuda;
            }
            catch (Exception)
            {
                return null;
            }
        }

        // Obtener respuestas de una ayuda con valoraciones y permisos del usuario actual
        public async Task<ObtenerRespuestasResponse> ObtenerRespuestasAsync(int id_ayuda, int id_usuario_actual)
        {
            try
            {
                var response = await _client.GetAsync($"getRespuestas.php?id_ayuda={id_ayuda}&id_usuario_actual={id_usuario_actual}");
                var result = await response.Content.ReadFromJsonAsync<ObtenerRespuestasResponse>();

                return result ?? new ObtenerRespuestasResponse { ok = false, msg = "Error al procesar la respuesta" };
            }
            catch (Exception ex)
            {
                return new ObtenerRespuestasResponse { ok = false, msg = $"Error de conexión: {ex.Message}" };
            }
        }
    }
}