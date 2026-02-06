using AyudaIguales.Models;

namespace AyudaIguales.Services
{
    public interface IAyudaService
    {
        // Obtener todas las ayudas del centro
        Task<ObtenerAyudasResponse> ObtenerAyudasAsync(int id_centro);

        // Obtener ayudas con filtros del centro
        Task<ObtenerAyudasResponse> ObtenerAyudasConFiltrosAsync(FiltrosAyuda filtros, int id_centro);

        // Crear una nueva ayuda
        Task<CrearAyudaResponse> CrearAyudaAsync(CrearAyudaRequest request);

        // Obtener ayuda por ID del centro (puede retornar null)
        Task<Ayuda?> ObtenerAyudaPorIdAsync(int id, int id_centro);
    }
}