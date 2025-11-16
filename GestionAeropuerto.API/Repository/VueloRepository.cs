using Dapper;
using GestionAeropuerto.API.Data;
using GestionAeropuerto.API.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GestionAeropuerto.API.Repository
{
    public class VueloRepository : IVueloRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public VueloRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Vuelo>> GetAllAsync()
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = @"
                SELECT 
                    V.VueloID, V.NumeroVuelo, V.AerolineaID, V.AeronaveID,
                    V.Origen, V.Destino, V.FechaSalida, V.FechaLlegada,
                    V.Estado, V.PuertaID, V.AsientosDisponibles, V.PrecioBase,
                    V.FechaRegistro,
                    A.Nombre AS NombreAerolinea,
                    AE.Modelo AS ModeloAeronave,
                    P.Numero AS NumeroPuerta
                FROM Vuelos V
                INNER JOIN Aerolineas A ON V.AerolineaID = A.AerolineaID
                INNER JOIN Aeronaves AE ON V.AeronaveID = AE.AeronaveID
                LEFT JOIN Puertas P ON V.PuertaID = P.PuertaID
                ORDER BY V.FechaSalida DESC";

            return await connection.QueryAsync<Vuelo>(query);
        }

        public async Task<Vuelo?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var query = @"
                SELECT 
                    V.VueloID, V.NumeroVuelo, V.AerolineaID, V.AeronaveID,
                    V.Origen, V.Destino, V.FechaSalida, V.FechaLlegada,
                    V.Estado, V.PuertaID, V.AsientosDisponibles, V.PrecioBase,
                    V.FechaRegistro,
                    A.Nombre AS NombreAerolinea,
                    AE.Modelo AS ModeloAeronave,
                    P.Numero AS NumeroPuerta
                FROM Vuelos V
                INNER JOIN Aerolineas A ON V.AerolineaID = A.AerolineaID
                INNER JOIN Aeronaves AE ON V.AeronaveID = AE.AeronaveID
                LEFT JOIN Puertas P ON V.PuertaID = P.PuertaID
                WHERE V.VueloID = @Id";

            return await connection.QueryFirstOrDefaultAsync<Vuelo>(query, new { Id = id });
        }

        public async Task<IEnumerable<Vuelo>> GetByFechaAsync(DateTime fecha)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Fecha", fecha.Date, DbType.Date);

            return await connection.QueryAsync<Vuelo>(
                "sp_ObtenerVuelosPorFecha",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<int> CreateAsync(Vuelo vuelo)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@NumeroVuelo", vuelo.NumeroVuelo);
            parameters.Add("@AerolineaID", vuelo.AerolineaID);
            parameters.Add("@AeronaveID", vuelo.AeronaveID);
            parameters.Add("@Origen", vuelo.Origen);
            parameters.Add("@Destino", vuelo.Destino);
            parameters.Add("@FechaSalida", vuelo.FechaSalida);
            parameters.Add("@FechaLlegada", vuelo.FechaLlegada);
            parameters.Add("@PrecioBase", vuelo.PrecioBase);
            parameters.Add("@VueloID", dbType: DbType.Int32, direction: ParameterDirection.Output);

            await connection.ExecuteAsync(
                "sp_CrearVuelo",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return parameters.Get<int>("@VueloID");
        }

        public async Task<bool> UpdateEstadoAsync(int id, string nuevoEstado, string? observaciones)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@VueloID", id);
            parameters.Add("@NuevoEstado", nuevoEstado);
            parameters.Add("@Observaciones", observaciones);

            try
            {
                await connection.ExecuteAsync(
                    "sp_ActualizarEstadoVuelo",
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

        public async Task<bool> AsignarPuertaAsync(int vueloId, int puertaId)
        {
            using var connection = _connectionFactory.CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@VueloID", vueloId);
            parameters.Add("@PuertaID", puertaId);

            try
            {
                await connection.ExecuteAsync(
                    "sp_AsignarPuerta",
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