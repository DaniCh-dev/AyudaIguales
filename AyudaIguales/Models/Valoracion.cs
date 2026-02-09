using System.ComponentModel.DataAnnotations;

namespace AyudaIguales.Models
{
    
    // Request para crear valoracion
    public class CrearValoracionRequest
    {
        [Required(ErrorMessage = "La respuesta es obligatoria")]
        public int id_respuesta { get; set; }

        [Required(ErrorMessage = "El usuario es obligatorio")]
        public int id_usuario { get; set; }

        [Required(ErrorMessage = "La nota es obligatoria")]
        [Range(1, 5, ErrorMessage = "La nota debe estar entre 1 y 5")]
        public int nota { get; set; }
    }

    // Response para crear valoracion
    public class CrearValoracionResponse
    {
        public bool ok { get; set; }
        public string msg { get; set; } = string.Empty;
        public int? id { get; set; }
    }

}