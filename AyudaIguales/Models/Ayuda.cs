using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace AyudaIguales.Models
{
    // Modelo principal de Ayuda
    public class Ayuda
    {
        public int id { get; set; }
        public int id_usuario { get; set; }
        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string descripcion { get; set; } = string.Empty;
        [Required(ErrorMessage = "El contenido es obligatorio")]
        public string contenido { get; set; } = string.Empty;
        public bool activa { get; set; }
        public DateTime fecha { get; set; }
        // Propiedades adicionales para mostrar en la vista
        public string nombre_usuario { get; set; } = string.Empty;
        public int num_respuestas { get; set; }
        // Lista de rutas de imagenes asociadas
        public List<string> imagenes { get; set; } = new List<string>();
    }

    // Modelo para crear una nueva ayuda
    public class CrearAyudaRequest
    {
        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string descripcion { get; set; } = string.Empty;
        [Required(ErrorMessage = "El contenido es obligatorio")]
        public string contenido { get; set; } = string.Empty;
        public int id_usuario { get; set; }
        // Archivos de imagen para subir (maximo 5 imagenes)
        public List<IFormFile>? imagenes { get; set; }
    }

    // Modelo para la respuesta al obtener ayudas
    public class ObtenerAyudasResponse
    {
        public bool ok { get; set; }
        public string msg { get; set; } = string.Empty;
        public List<Ayuda> ayudas { get; set; } = new List<Ayuda>();
    }

    // Modelo para la respuesta al crear ayuda
    public class CrearAyudaResponse
    {
        public bool ok { get; set; }
        public string msg { get; set; } = string.Empty;
        public int? id { get; set; }
        // Lista de rutas de imagenes guardadas
        public List<string> imagenes { get; set; } = new List<string>();
    }

    // Modelo para filtros de búsqueda
    public class FiltrosAyuda
    {
        public string? busqueda { get; set; }
        public string? estado { get; set; }
        public int? id_usuario { get; set; }
        public string? fecha { get; set; }
        public string? respuestas { get; set; }
    }
    // Modelo para una respuesta con informacion completa
    public class RespuestaDetalle
    {
        public int id { get; set; }
        public int id_ayuda { get; set; }
        public int id_usuario { get; set; }
        public string contenido { get; set; } = string.Empty;
        public DateTime fecha { get; set; }
        public string nombre_usuario { get; set; } = string.Empty;
        public double nota_promedio { get; set; }
        public bool puede_valorar { get; set; }
        public bool ya_valorado { get; set; }
        public List<string> imagenes { get; set; } = new List<string>();
    }

    // Modelo para la respuesta al obtener respuestas de una ayuda
    public class ObtenerRespuestasResponse
    {
        public bool ok { get; set; }
        public string msg { get; set; } = string.Empty;
        public List<RespuestaDetalle> respuestas { get; set; } = new List<RespuestaDetalle>();
    }
}