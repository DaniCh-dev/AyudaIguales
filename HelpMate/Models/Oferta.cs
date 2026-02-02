using System.ComponentModel.DataAnnotations;

namespace HelpMate.Models
{
    public class Oferta
    {
        public int Id { get; set; }

        [Required]
        public int SolicitudId { get; set; }
        public Solicitud? Solicitud { get; set; }

        [Required]
        public int TutorUsuarioId { get; set; }
        public Usuario? Tutor { get; set; }

        [Required, StringLength(500)]
        public string MensajeInicial { get; set; } = "";

        [Required, StringLength(20)]
        public string Estado { get; set; } = "Ofrecida";
        // Ofrecida | Aceptada | Rechazada | Finalizada

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
