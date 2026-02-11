using AyudaIguales.Models;

namespace AyudaIguales.Services
{
    public interface IEstadisticasService
    {
        // Obtener estadisticas
        Task<ObtenerEstadisticasResponse> ObtenerEstadisticasAsync(int id_usuario, int id_centro, string rol);
    }
}