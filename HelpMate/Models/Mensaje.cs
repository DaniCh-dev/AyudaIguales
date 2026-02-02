using System.ComponentModel.DataAnnotations;

namespace HelpMate.Models
{
    public class Mensaje
    {
        public int Id { get; set; }

        [Required]
        public int OfertaId { get; set; }
        public Oferta? Oferta { get; set; }

        [Required]
        public int EmisorUsuarioId { get; set; }
        public Usuario? Emisor { get; set; }

        [Required]
        public string Contenido { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
