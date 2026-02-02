using System.ComponentModel.DataAnnotations;

namespace HelpMate.Models
{
    public class Asignatura
    {
        public int Id { get; set; }

        [Required, StringLength(80)]
        public string Nombre { get; set; } = "";
    }
}
