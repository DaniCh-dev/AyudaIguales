using System.ComponentModel.DataAnnotations;

namespace AyudaIguales.Models
{
    public class Centro
    {

        public int id { get; set; }
        [Required(ErrorMessage = "El nombre del centro es obligatorio")]
        public string nombre { get; set; }
        [Required(ErrorMessage = "El CIF del centro es obligatorio")]
        public string cif { get; set; }

    }
}
