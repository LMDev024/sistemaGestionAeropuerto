using GestionAeropuerto.API.Models;

namespace GestionAeropuerto.API.Repository
{
    public interface IVueloRepository
    {
        Task<IEnumerable<Vuelo>> GetAllAsync();
        Task<Vuelo?> GetByIdAsync(int id);
        Task<IEnumerable<Vuelo>> GetByFechaAsync(DateTime fecha);
        Task<int> CreateAsync(Vuelo vuelo);
        Task<bool> UpdateEstadoAsync(int id, string nuevoEstado, string? observaciones);
        Task<bool> AsignarPuertaAsync(int vueloId, int puertaId);
    }
}