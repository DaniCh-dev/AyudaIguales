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
}
