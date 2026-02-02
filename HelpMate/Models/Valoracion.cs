using System.ComponentModel.DataAnnotations;

namespace HelpMate.Models
{
    public class Valoracion
    {
        public int Id { get; set; }

        [Required]
        public int OfertaId { get; set; }
        public Oferta? Oferta { get; set; }

        [Required]
        public int EmisorUsuarioId { get; set; }
        public Usuario? Emisor { get; set; }

        [Required]
        public int ReceptorUsuarioId { get; set; }
        public Usuario? Receptor { get; set; }

        [Range(1, 5)]
        public int Puntuacion { get; set; }

        [StringLength(300)]
        public string? Comentario { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
