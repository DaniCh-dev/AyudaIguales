namespace AyudaIguales.Services
{
    public interface ICentroService
    {
        Task<bool> CreateCentro(string nombre, string cif);
    }
}
