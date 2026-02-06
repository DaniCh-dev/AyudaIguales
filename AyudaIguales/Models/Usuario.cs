using System.ComponentModel.DataAnnotations;

namespace AyudaIguales.Models
{
    public enum RolUsuario
    {
        admin,
        usuario
    }

    public class Usuario
    {
        public int id { get; set; }
        public string nombre_usuario { get; set; }
        public string correo { get; set; }
        public RolUsuario rol { get; set; }
        public int id_centro { get; set; }
    }

    // Modelo para la petición de registro
    public class RegistroRequest
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string nombre_usuario { get; set; }

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "El formato del correo no es válido")]
        public string correo { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string password { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un centro")]
        public int id_centro { get; set; }

        // Checkbox para marcar si es administrador
        public bool es_admin { get; set; } = false;

        // Propiedad calculada que devuelve el rol según el checkbox
        public string rol => es_admin ? "admin" : "usuario";
    }

    // Modelo para la respuesta de registro
    public class RegistroResponse
    {
        public bool ok { get; set; }
        public string msg { get; set; }
        public int? id { get; set; }
    }

    // Modelo para la petición de login
    public class LoginRequest
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        public string nombre_usuario { get; set; }

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        public string password { get; set; }
    }

    // Modelo para la respuesta de login
    public class LoginResponse
    {
        public bool ok { get; set; }
        public string msg { get; set; }
        public Usuario usuario { get; set; }
    }
}