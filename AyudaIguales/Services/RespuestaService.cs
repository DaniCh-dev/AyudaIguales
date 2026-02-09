using AyudaIguales.Models;

namespace AyudaIguales.Services
{
    public class RespuestaService : IRespuestaService
    {
        private readonly HttpClient _client;

        public RespuestaService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("ApiClient");
        }

        // Crear una nueva respuesta con soporte de imagenes
        public async Task<CrearRespuestaResponse> CrearRespuestaAsync(CrearRespuestaRequest request)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(request.contenido))
                return new CrearRespuestaResponse { ok = false, msg = "El contenido es obligatorio" };

            if (request.id_ayuda <= 0)
                return new CrearRespuestaResponse { ok = false, msg = "Ayuda no válida" };

            if (request.id_usuario <= 0)
                return new CrearRespuestaResponse { ok = false, msg = "Usuario no válido" };

            try
            {
                // Crear contenido multipart si hay imagenes, sino JSON
                if (request.imagenes != null && request.imagenes.Any())
                {
                    // Usar MultipartFormDataContent para enviar archivos
                    using var formData = new MultipartFormDataContent();

                    // Agregar datos basicos como campos de texto
                    formData.Add(new StringContent(request.id_ayuda.ToString()), "id_ayuda");
                    formData.Add(new StringContent(request.id_usuario.ToString()), "id_usuario");
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
                    var response = await _client.PostAsync("createRespuesta.php", formData);
                    var result = await response.Content.ReadFromJsonAsync<CrearRespuestaResponse>();
                    return result ?? new CrearRespuestaResponse { ok = false, msg = "Error al procesar la respuesta" };
                }
                else
                {
                    // Si no hay imagenes, enviar JSON normal
                    var data = new
                    {
                        id_ayuda = request.id_ayuda,
                        id_usuario = request.id_usuario,
                        contenido = request.contenido
                    };

                    var response = await _client.PostAsJsonAsync("createRespuesta.php", data);
                    var result = await response.Content.ReadFromJsonAsync<CrearRespuestaResponse>();
                    return result ?? new CrearRespuestaResponse { ok = false, msg = "Error al procesar la respuesta" };
                }
            }
            catch (Exception ex)
            {
                return new CrearRespuestaResponse { ok = false, msg = $"Error de conexión: {ex.Message}" };
            }
        }


        // Eliminar respuesta (solo admin)
        public async Task<EliminarRespuestaResponse> EliminarRespuestaAsync(int id, int id_usuario, string rol)
        {
            try
            {
                var data = new
                {
                    id = id,
                    id_usuario = id_usuario,
                    rol = rol
                };

                var response = await _client.PostAsJsonAsync("deleteRespuesta.php", data);
                var result = await response.Content.ReadFromJsonAsync<EliminarRespuestaResponse>();

                return result ?? new EliminarRespuestaResponse { ok = false, msg = "Error al procesar la respuesta" };
            }
            catch (Exception ex)
            {
                return new EliminarRespuestaResponse { ok = false, msg = $"Error de conexión: {ex.Message}" };
            }
        }
    }
}