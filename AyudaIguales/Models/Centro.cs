using System.ComponentModel.DataAnnotations;

namespace AyudaIguales.Models
{
    public class Centro
    {

        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Nombre { get; set; }

        [Required, StringLength(20)]
        public string CIF { get; set; }

        [Required, StringLength(20)]
        public string Codigo { get; set; }
    }
}
