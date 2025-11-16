using GestionAeropuerto.API.Models;

namespace GestionAeropuerto.API.Repository
{
    public interface IPuertaRepository
    {
        Task<IEnumerable<Puerta>> GetAllAsync();
        Task<IEnumerable<Puerta>> GetDisponiblesAsync();
        Task<Puerta?> GetByIdAsync(int id);
    }
}