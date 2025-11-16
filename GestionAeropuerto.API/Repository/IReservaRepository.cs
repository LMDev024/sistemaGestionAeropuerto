using GestionAeropuerto.API.Models;

namespace GestionAeropuerto.API.Repository
{
    public interface IReservaRepository
    {
        Task<IEnumerable<Reserva>> GetAllAsync();
        Task<Reserva?> GetByIdAsync(int id);
        Task<Reserva?> GetByCodigoAsync(string codigo);
        Task<IEnumerable<Reserva>> GetByVueloAsync(int vueloId);
        Task<IEnumerable<Reserva>> GetByPasajeroAsync(int pasajeroId);
        Task<int> CreateAsync(Reserva reserva);
        Task<bool> CancelarAsync(int reservaId, string? motivo);
    }
}