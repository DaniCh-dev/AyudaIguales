using AyudaIguales.Models;

namespace AyudaIguales.Services
{
    public class ChatService : IChatService
    {
        private readonly HttpClient _client;

        public ChatService(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient("ApiClient");
        }

        // Obtener todos los chats del usuario
        public async Task<ObtenerChatsResponse> ObtenerChatsAsync(int id_usuario, int id_centro)
        {
            try
            {
                var response = await _client.GetAsync($"getChats.php?id_usuario={id_usuario}&id_centro={id_centro}");
                var result = await response.Content.ReadFromJsonAsync<ObtenerChatsResponse>();

                return result ?? new ObtenerChatsResponse { ok = false, msg = "Error al procesar la respuesta" };
            }
            catch (Exception ex)
            {
                return new ObtenerChatsResponse { ok = false, msg = $"Error de conexión: {ex.Message}" };
            }
        }

        // Obtener usuarios disponibles para chatear
        public async Task<ObtenerUsuariosResponse> ObtenerUsuariosParaChatAsync(int id_usuario, int id_centro)
        {
            try
            {
                var response = await _client.GetAsync($"getUsuariosParaChat.php?id_usuario={id_usuario}&id_centro={id_centro}");
                var result = await response.Content.ReadFromJsonAsync<ObtenerUsuariosResponse>();

                return result ?? new ObtenerUsuariosResponse { ok = false, msg = "Error al procesar la respuesta" };
            }
            catch (Exception ex)
            {
                return new ObtenerUsuariosResponse { ok = false, msg = $"Error de conexión: {ex.Message}" };
            }
        }

        // Crear o obtener chat entre dos usuarios
        public async Task<CrearChatResponse> CrearChatAsync(int id_usuario1, int id_usuario2)
        {
            try
            {
                var data = new
                {
                    id_usuario1 = id_usuario1,
                    id_usuario2 = id_usuario2
                };

                var response = await _client.PostAsJsonAsync("createChat.php", data);
                var result = await response.Content.ReadFromJsonAsync<CrearChatResponse>();

                return result ?? new CrearChatResponse { ok = false, msg = "Error al procesar la respuesta" };
            }
            catch (Exception ex)
            {
                return new CrearChatResponse { ok = false, msg = $"Error de conexión: {ex.Message}" };
            }
        }
        // Obtener informacion del chat
        public async Task<ObtenerInfoChatResponse> ObtenerInfoChatAsync(int id_chat, int id_usuario)
        {
            try
            {
                var response = await _client.GetAsync($"getInfoChat.php?id_chat={id_chat}&id_usuario={id_usuario}");
                var result = await response.Content.ReadFromJsonAsync<ObtenerInfoChatResponse>();

                return result ?? new ObtenerInfoChatResponse { ok = false, msg = "Error al procesar la respuesta" };
            }
            catch (Exception ex)
            {
                return new ObtenerInfoChatResponse { ok = false, msg = $"Error de conexión: {ex.Message}" };
            }
        }
        // Obtener mensajes de un chat
        public async Task<ObtenerMensajesResponse> ObtenerMensajesAsync(int id_chat, int id_usuario)
        {
            try
            {
                var response = await _client.GetAsync($"getMensajes.php?id_chat={id_chat}&id_usuario={id_usuario}");
                var result = await response.Content.ReadFromJsonAsync<ObtenerMensajesResponse>();

                return result ?? new ObtenerMensajesResponse { ok = false, msg = "Error al procesar la respuesta" };
            }
            catch (Exception ex)
            {
                return new ObtenerMensajesResponse { ok = false, msg = $"Error de conexión: {ex.Message}" };
            }
        }

        // Enviar mensaje en un chat
        public async Task<EnviarMensajeResponse> EnviarMensajeAsync(EnviarMensajeRequest request)
        {
            // Validaciones
            if (request.id_chat <= 0)
                return new EnviarMensajeResponse { ok = false, msg = "Chat no válido" };

            if (request.id_usuario <= 0)
                return new EnviarMensajeResponse { ok = false, msg = "Usuario no válido" };

            if (string.IsNullOrWhiteSpace(request.contenido) && (request.imagenes == null || !request.imagenes.Any()))
                return new EnviarMensajeResponse { ok = false, msg = "El mensaje no puede estar vacío" };

            try
            {
                // Crear contenido multipart si hay imagenes, sino JSON
                if (request.imagenes != null && request.imagenes.Any())
                {
                    // Usar MultipartFormDataContent para enviar archivos
                    using var formData = new MultipartFormDataContent();

                    // Agregar datos basicos como campos de texto
                    formData.Add(new StringContent(request.id_chat.ToString()), "id_chat");
                    formData.Add(new StringContent(request.id_usuario.ToString()), "id_usuario");
                    formData.Add(new StringContent(request.contenido ?? ""), "contenido");

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
                    var response = await _client.PostAsync("sendMensaje.php", formData);
                    var result = await response.Content.ReadFromJsonAsync<EnviarMensajeResponse>();
                    return result ?? new EnviarMensajeResponse { ok = false, msg = "Error al procesar la respuesta" };
                }
                else
                {
                    // Si no hay imagenes, enviar JSON normal
                    var data = new
                    {
                        id_chat = request.id_chat,
                        id_usuario = request.id_usuario,
                        contenido = request.contenido
                    };

                    var response = await _client.PostAsJsonAsync("sendMensaje.php", data);
                    var result = await response.Content.ReadFromJsonAsync<EnviarMensajeResponse>();
                    return result ?? new EnviarMensajeResponse { ok = false, msg = "Error al procesar la respuesta" };
                }
            }
            catch (Exception ex)
            {
                return new EnviarMensajeResponse { ok = false, msg = $"Error de conexión: {ex.Message}" };
            }
        }





    }
}