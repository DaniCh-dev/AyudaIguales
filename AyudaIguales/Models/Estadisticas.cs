namespace AyudaIguales.Models
{
    // Estadisticas generales
    public class Estadisticas
    {
        // Estadisticas de admin
        public int total_usuarios { get; set; }
        public int total_ayudas { get; set; }
        public int ayudas_activas { get; set; }
        public int ayudas_inactivas { get; set; }
        public int total_respuestas { get; set; }
        public int total_valoraciones { get; set; }
        public double promedio_valoraciones { get; set; }
        public List<DatoMes> ayudas_por_mes { get; set; } = new List<DatoMes>();
        public List<UsuarioTop> top_usuarios_ayudas { get; set; } = new List<UsuarioTop>();
        public List<UsuarioTop> top_usuarios_respuestas { get; set; } = new List<UsuarioTop>();
        public int ayudas_sin_respuestas { get; set; }
        public int ayudas_con_respuestas { get; set; }

        // Estadisticas de usuario
        public int mis_ayudas { get; set; }
        public int mis_ayudas_activas { get; set; }
        public int mis_respuestas { get; set; }
        public int respuestas_recibidas { get; set; }
        public double mi_promedio_valoraciones { get; set; }
        public int total_valoraciones_recibidas { get; set; }
        public List<DatoMes> mis_ayudas_por_mes { get; set; } = new List<DatoMes>();
        public List<DatoMes> mis_respuestas_por_mes { get; set; } = new List<DatoMes>();
        public List<DistribucionNota> distribucion_valoraciones { get; set; } = new List<DistribucionNota>();
        public double tasa_respuesta { get; set; }
    }

    // Dato por mes para graficos
    public class DatoMes
    {
        public string mes { get; set; } = string.Empty;
        public int total { get; set; }
    }

    // Usuario top para rankings
    public class UsuarioTop
    {
        public string nombre_usuario { get; set; } = string.Empty;
        public int total_ayudas { get; set; }
        public int total_respuestas { get; set; }
    }

    // Distribucion de notas
    public class DistribucionNota
    {
        public int nota { get; set; }
        public int cantidad { get; set; }
    }

    // Response para obtener estadisticas
    public class ObtenerEstadisticasResponse
    {
        public bool ok { get; set; }
        public string msg { get; set; } = string.Empty;
        public Estadisticas? estadisticas { get; set; }
    }
}