using System.ComponentModel.DataAnnotations;
namespace AyudaIguales.Models
{
    // Request para crear respuesta
    public class CrearRespuestaRequest
    {
        [Required(ErrorMessage = "El contenido es obligatorio")]
        public string contenido { get; set; } = string.Empty;

        public int id_ayuda { get; set; }
        public int id_usuario { get; set; }

        // Archivos de imagen para subir (maximo 5 imagenes)
        public List<IFormFile>? imagenes { get; set; }
    }

    // Response para crear respuesta
    public class CrearRespuestaResponse
    {
        public bool ok { get; set; }
        public string msg { get; set; } = string.Empty;
        public int? id { get; set; }
        public List<string> imagenes { get; set; } = new List<string>();
    }

    // Response al eliminar respuesta
    public class EliminarRespuestaResponse
    {
        public bool ok { get; set; }
        public string msg { get; set; } = string.Empty;
        public int id_ayuda { get; set; }
    }
}