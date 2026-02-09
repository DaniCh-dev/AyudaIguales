using AyudaIguales.Models;

namespace AyudaIguales.Services
{
    public interface IChatService
    {
        // Obtener todos los chats del usuario
        Task<ObtenerChatsResponse> ObtenerChatsAsync(int id_usuario, int id_centro);

        // Obtener usuarios disponibles para chatear (usa el modelo de User)
        Task<ObtenerUsuariosResponse> ObtenerUsuariosParaChatAsync(int id_usuario, int id_centro);

        // Crear o obtener chat entre dos usuarios
        Task<CrearChatResponse> CrearChatAsync(int id_usuario1, int id_usuario2);
    }
}