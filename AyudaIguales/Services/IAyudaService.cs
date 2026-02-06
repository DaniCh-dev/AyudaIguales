using AyudaIguales.Models;

namespace AyudaIguales.Services
{
    public interface IAyudaService
    {
        // Obtener todas las ayudas
        Task<ObtenerAyudasResponse> ObtenerAyudasAsync();

        // Obtener ayudas con filtros
        Task<ObtenerAyudasResponse> ObtenerAyudasConFiltrosAsync(FiltrosAyuda filtros);

        // Crear una nueva ayuda
        Task<CrearAyudaResponse> CrearAyudaAsync(CrearAyudaRequest request);

        // Obtener ayuda por ID
        Task<Ayuda> ObtenerAyudaPorIdAsync(int id);
    }
}