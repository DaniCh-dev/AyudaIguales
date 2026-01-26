using System.ComponentModel.DataAnnotations;

namespace AyudaIguales.Models
{
    public class Respuesta
    {
        public int Id { get; set; }

        public int IdAyuda { get; set; }

        public int IdUsuario { get; set; }

        [Required]
        public string Contenido { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
