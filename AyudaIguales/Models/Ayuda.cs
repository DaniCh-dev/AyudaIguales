using System.ComponentModel.DataAnnotations;

namespace AyudaIguales.Models
{
    public class Ayuda
    {
        public int Id { get; set; }

        public int IdUsuario { get; set; }
        public virtual Usuario Usuario { get; set; }

        [Required, StringLength(255)]
        public string Descripcion { get; set; }

        [Required]
        public string Contenido { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
