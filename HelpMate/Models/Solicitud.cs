using System.ComponentModel.DataAnnotations;

namespace HelpMate.Models
{
    public class Solicitud
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Titulo { get; set; } = "";

        [Required]
        public string Descripcion { get; set; } = "";

        [Required, StringLength(20)]
        public string Estado { get; set; } = "Abierta"; // Abierta | EnProceso | Cerrada

        // Relaciones mínimas
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }

        public int AsignaturaId { get; set; }
        public Asignatura? Asignatura { get; set; }
    }
}
