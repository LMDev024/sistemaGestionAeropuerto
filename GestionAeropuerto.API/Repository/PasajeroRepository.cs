using Dapper;
using GestionAeropuerto.API.Data;
using GestionAeropuerto.API.Models;

namespace GestionAeropuerto.API.Repository
{
    public class PasajeroRepository : IPasajeroRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public PasajeroRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Pasajero>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = "SELECT * FROM Pasajeros ORDER BY FechaRegistro DESC";
            return await connection.QueryAsync<Pasajero>(query);
        }

        public async Task<Pasajero?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = "SELECT * FROM Pasajeros WHERE PasajeroID = @Id";
            return await connection.QueryFirstOrDefaultAsync<Pasajero>(query, new { Id = id });
        }

        public async Task<Pasajero?> GetByDocumentoAsync(string tipoDocumento, string numeroDocumento)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = @"
                SELECT * FROM Pasajeros 
                WHERE TipoDocumento = @TipoDocumento 
                AND NumeroDocumento = @NumeroDocumento";

            return await connection.QueryFirstOrDefaultAsync<Pasajero>(
                query,
                new { TipoDocumento = tipoDocumento, NumeroDocumento = numeroDocumento }
            );
        }

        public async Task<int> CreateAsync(Pasajero pasajero)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = @"
                INSERT INTO Pasajeros 
                (TipoDocumento, NumeroDocumento, Nombre, Apellido, FechaNacimiento, 
                 Nacionalidad, Email, Telefono)
                VALUES 
                (@TipoDocumento, @NumeroDocumento, @Nombre, @Apellido, @FechaNacimiento,
                 @Nacionalidad, @Email, @Telefono);
                SELECT CAST(SCOPE_IDENTITY() as int)";

            return await connection.ExecuteScalarAsync<int>(query, pasajero);
        }

        public async Task<bool> UpdateAsync(Pasajero pasajero)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = @"
                UPDATE Pasajeros SET
                    Nombre = @Nombre,
                    Apellido = @Apellido,
                    FechaNacimiento = @FechaNacimiento,
                    Nacionalidad = @Nacionalidad,
                    Email = @Email,
                    Telefono = @Telefono
                WHERE PasajeroID = @PasajeroID";

            var rowsAffected = await connection.ExecuteAsync(query, pasajero);
            return rowsAffected > 0;
        }
    }
}