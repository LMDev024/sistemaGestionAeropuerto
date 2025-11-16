using Dapper;
using GestionAeropuerto.API.Data;
using GestionAeropuerto.API.Models;

namespace GestionAeropuerto.API.Repository
{
    public class PuertaRepository : IPuertaRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public PuertaRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Puerta>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = "SELECT * FROM Puertas ORDER BY Terminal, Numero";
            return await connection.QueryAsync<Puerta>(query);
        }

        public async Task<IEnumerable<Puerta>> GetDisponiblesAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = "SELECT * FROM Puertas WHERE Estado = 'Disponible' ORDER BY Terminal, Numero";
            return await connection.QueryAsync<Puerta>(query);
        }

        public async Task<Puerta?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = "SELECT * FROM Puertas WHERE PuertaID = @Id";
            return await connection.QueryFirstOrDefaultAsync<Puerta>(query, new { Id = id });
        }
    }
}