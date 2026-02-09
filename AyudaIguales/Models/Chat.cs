using System.ComponentModel.DataAnnotations;

namespace AyudaIguales.Models
{
    // Modelo de chat
    public class Chat
    {
        public int id_chat { get; set; }
        public DateTime fecha_creacion { get; set; }
        public int id_otro_usuario { get; set; }
        public string nombre_otro_usuario { get; set; } = string.Empty;
        public string? ultimo_mensaje { get; set; }
        public DateTime? fecha_ultimo_mensaje { get; set; }
        public int? id_emisor_ultimo_mensaje { get; set; }
        public bool mensaje_propio { get; set; }
    }

    // Request para crear chat
    public class CrearChatRequest
    {
        [Required]
        public int id_usuario1 { get; set; }

        [Required]
        public int id_usuario2 { get; set; }
    }

    // Response para obtener chats
    public class ObtenerChatsResponse
    {
        public bool ok { get; set; }
        public string msg { get; set; } = string.Empty;
        public List<Chat> chats { get; set; } = new List<Chat>();
    }

    // Response para crear chat
    public class CrearChatResponse
    {
        public bool ok { get; set; }
        public string msg { get; set; } = string.Empty;
        public int id_chat { get; set; }
        public bool nuevo { get; set; }
    }
}