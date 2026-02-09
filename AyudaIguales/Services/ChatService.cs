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
    }
}