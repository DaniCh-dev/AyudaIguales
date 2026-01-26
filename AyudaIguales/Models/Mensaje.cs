namespace AyudaIguales.Models
{
    public class Mensaje
    {
        public int Id { get; set; }

        public int IdChat { get; set; }

        public int IdUsuario { get; set; }

        public string Contenido { get; set; }


        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}
