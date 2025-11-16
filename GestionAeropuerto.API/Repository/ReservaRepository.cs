using Dapper;
using GestionAeropuerto.API.Data;
using GestionAeropuerto.API.Models;
using System.Data;

namespace GestionAeropuerto.API.Repository
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public ReservaRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Reserva>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = @"
                SELECT 
                    R.ReservaID, R.CodigoReserva, R.VueloID, R.PasajeroID,
                    R.NumeroAsiento, R.Clase, R.Estado, R.Precio, R.Equipaje,
                    R.FechaReserva,
                    P.Nombre + ' ' + P.Apellido AS NombrePasajero,
                    V.NumeroVuelo
                FROM Reservas R
                INNER JOIN Pasajeros P ON R.PasajeroID = P.PasajeroID
                INNER JOIN Vuelos V ON R.VueloID = V.VueloID
                WHERE R.Estado != 'Cancelada'
                ORDER BY R.FechaReserva DESC";

            return await connection.QueryAsync<Reserva>(query);
        }

        public async Task<Reserva?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = @"
                SELECT 
                    R.ReservaID, R.CodigoReserva, R.VueloID, R.PasajeroID,
                    R.NumeroAsiento, R.Clase, R.Estado, R.Precio, R.Equipaje,
                    R.FechaReserva,
                    P.Nombre + ' ' + P.Apellido AS NombrePasajero,
                    V.NumeroVuelo
                FROM Reservas R
                INNER JOIN Pasajeros P ON R.PasajeroID = P.PasajeroID
                INNER JOIN Vuelos V ON R.VueloID = V.VueloID
                WHERE R.ReservaID = @Id";

            return await connection.QueryFirstOrDefaultAsync<Reserva>(query, new { Id = id });
        }

        public async Task<Reserva?> GetByCodigoAsync(string codigo)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = @"
                SELECT 
                    R.ReservaID, R.CodigoReserva, R.VueloID, R.PasajeroID,
                    R.NumeroAsiento, R.Clase, R.Estado, R.Precio, R.Equipaje,
                    R.FechaReserva,
                    P.Nombre + ' ' + P.Apellido AS NombrePasajero,
                    V.NumeroVuelo
                FROM Reservas R
                INNER JOIN Pasajeros P ON R.PasajeroID = P.PasajeroID
                INNER JOIN Vuelos V ON R.VueloID = V.VueloID
                WHERE R.CodigoReserva = @Codigo";

            return await connection.QueryFirstOrDefaultAsync<Reserva>(query, new { Codigo = codigo });
        }

        public async Task<IEnumerable<Reserva>> GetByVueloAsync(int vueloId)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@VueloID", vueloId);

            return await connection.QueryAsync<Reserva>(
                "sp_ObtenerPasajerosPorVuelo",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<IEnumerable<Reserva>> GetByPasajeroAsync(int pasajeroId)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = @"
                SELECT * FROM fn_ObtenerReservasPorPasajero(@PasajeroID)";

            return await connection.QueryAsync<Reserva>(query, new { PasajeroID = pasajeroId });
        }

        public async Task<int> CreateAsync(Reserva reserva)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@VueloID", reserva.VueloID);
            parameters.Add("@PasajeroID", reserva.PasajeroID);
            parameters.Add("@NumeroAsiento", reserva.NumeroAsiento);
            parameters.Add("@Clase", reserva.Clase);
            parameters.Add("@Equipaje", reserva.Equipaje);
            parameters.Add("@ReservaID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "sp_RealizarReserva",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@ReservaID");
        }

        public async Task<bool> CancelarAsync(int reservaId, string? motivo)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@ReservaID", reservaId);
            parameters.Add("@Motivo", motivo);

            try
            {
                await connection.ExecuteAsync(
                    "sp_CancelarReserva",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}