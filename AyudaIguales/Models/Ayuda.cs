using System.ComponentModel.DataAnnotations;

namespace AyudaIguales.Models
{
    // Modelo principal de Ayuda
    public class Ayuda
    {
        public int id { get; set; }
        public int id_usuario { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string descripcion { get; set; }

        [Required(ErrorMessage = "El contenido es obligatorio")]
        public string contenido { get; set; }

        public bool activa { get; set; }
        public DateTime fecha { get; set; }

        // Propiedades adicionales para mostrar en la vista
        public string nombre_usuario { get; set; }
        public int num_respuestas { get; set; }
    }

    // Modelo para crear una nueva ayuda
    public class CrearAyudaRequest
    {
        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string descripcion { get; set; }

        [Required(ErrorMessage = "El contenido es obligatorio")]
        public string contenido { get; set; }

        public int id_usuario { get; set; }
    }

    // Modelo para la respuesta al obtener ayudas
    public class ObtenerAyudasResponse
    {
        public bool ok { get; set; }
        public string msg { get; set; }
        public List<Ayuda> ayudas { get; set; }
    }

    // Modelo para la respuesta al crear ayuda
    public class CrearAyudaResponse
    {
        public bool ok { get; set; }
        public string msg { get; set; }
        public int? id { get; set; }
    }

    // Modelo para filtros de búsqueda
    public class FiltrosAyuda
    {
        public string busqueda { get; set; }
        public string estado { get; set; }
        public int? id_usuario { get; set; }
        public string fecha { get; set; }
        public string respuestas { get; set; }
    }
}