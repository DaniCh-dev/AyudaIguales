using AyudaIguales.Models;

namespace AyudaIguales.Services
{
    public interface ICentroService
    {
        Task<bool> CreateCentro(string nombre, string cif);
        Task<List<Centro>> ObtenerCentrosAsync();
    }
}