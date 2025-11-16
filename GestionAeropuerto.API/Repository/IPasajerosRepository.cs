using GestionAeropuerto.API.Models;

namespace GestionAeropuerto.API.Repository
{
    public interface IPasajeroRepository
    {
        Task<IEnumerable<Pasajero>> GetAllAsync();
        Task<Pasajero?> GetByIdAsync(int id);
        Task<Pasajero?> GetByDocumentoAsync(string tipoDocumento, string numeroDocumento);
        Task<int> CreateAsync(Pasajero pasajero);
        Task<bool> UpdateAsync(Pasajero pasajero);
    }
}