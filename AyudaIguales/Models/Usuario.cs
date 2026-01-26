using System.ComponentModel.DataAnnotations;

namespace AyudaIguales.Models
{
    public enum RolUsuario
    {
        Admin,
        Usuario
    }
    public class Usuario
    {
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string NombreUsuario { get; set; }

        [Required, StringLength(255)]
        public string Password { get; set; }

        [Required, StringLength(100)]
        public string Correo { get; set; }

        public RolUsuario Rol { get; set; } = RolUsuario.Usuario;
    }
}
