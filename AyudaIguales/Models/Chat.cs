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
    // Modelo de mensaje
    public class Mensaje
    {
        public int id { get; set; }
        public int id_chat { get; set; }
        public int id_usuario { get; set; }
        public string? contenido { get; set; }
        public DateTime fecha { get; set; }
        public string nombre_usuario { get; set; } = string.Empty;
        public bool es_propio { get; set; }
        public List<string> imagenes { get; set; } = new List<string>();
    }

    // Informacion del chat
    public class InfoChat
    {
        public int id_chat { get; set; }
        public int id_otro_usuario { get; set; }
        public string nombre_otro_usuario { get; set; } = string.Empty;
    }
    // Request para enviar mensaje
    public class EnviarMensajeRequest
    {
        [Required]
        public int id_chat { get; set; }

        [Required]
        public int id_usuario { get; set; }

        public string? contenido { get; set; }

        // Archivos de imagen para subir (maximo 5 imagenes)
        public List<IFormFile>? imagenes { get; set; }
    }
    // Response para enviar mensaje
    public class EnviarMensajeResponse
    {
        public bool ok { get; set; }
        public string msg { get; set; } = string.Empty;
        public int id { get; set; }
        public List<string> imagenes { get; set; } = new List<string>();
    }

    // Response para obtener info del chat
    public class ObtenerInfoChatResponse
    {
        public bool ok { get; set; }
        public string msg { get; set; } = string.Empty;
        public InfoChat? info { get; set; }
    }
    // Response para obtener mensajes
    public class ObtenerMensajesResponse
    {
        public bool ok { get; set; }
        public string msg { get; set; } = string.Empty;
        public List<Mensaje> mensajes { get; set; } = new List<Mensaje>();
    }
}