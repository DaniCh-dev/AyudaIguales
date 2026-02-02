using System.ComponentModel.DataAnnotations;

namespace HelpMate.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required, StringLength(60)]
        public string Nombre { get; set; } = "";

        [Required, EmailAddress, StringLength(120)]
        public string Email { get; set; } = "";

        [Required, StringLength(255)]
        public string PasswordHash { get; set; } = "";

        [Required, StringLength(20)]
        public string Rol { get; set; } = "Alumno"; // Alumno | Administrador

        public bool Activo { get; set; } = true;
    }
}
