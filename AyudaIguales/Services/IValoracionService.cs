using AyudaIguales.Models;
using System.Threading.Tasks;

namespace AyudaIguales.Services
{
    public interface IValoracionService
    {
        // Crear una nueva valoracion
        Task<CrearValoracionResponse> CrearValoracionAsync(CrearValoracionRequest request);

       
    }
}