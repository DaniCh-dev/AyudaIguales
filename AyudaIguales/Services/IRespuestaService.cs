using AyudaIguales.Models;

namespace AyudaIguales.Services
{
    public interface IRespuestaService
    {
        // Crear una nueva respuesta con soporte de imagenes
        Task<CrearRespuestaResponse> CrearRespuestaAsync(CrearRespuestaRequest request);
    }
}
