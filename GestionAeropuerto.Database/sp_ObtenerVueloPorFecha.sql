
CREATE PROCEDURE sp_ObtenerVuelosPorFecha
    @Fecha DATE
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        V.VueloID,
        V.NumeroVuelo,
        A.Nombre AS Aerolinea,
        A.CodigoIATA,
        AE.Modelo AS Aeronave,
        V.Origen,
        V.Destino,
        V.FechaSalida,
        V.FechaLlegada,
        V.Estado,
        P.Numero AS Puerta,
        P.Terminal,
        V.AsientosDisponibles,
        AE.CapacidadPasajeros AS CapacidadTotal,
        V.PrecioBase
    FROM Vuelos V
    INNER JOIN Aerolineas A ON V.AerolineaID = A.AerolineaID
    INNER JOIN Aeronaves AE ON V.AeronaveID = AE.AeronaveID
    LEFT JOIN Puertas P ON V.PuertaID = P.PuertaID
    WHERE CAST(V.FechaSalida AS DATE) = @Fecha
    ORDER BY V.FechaSalida;
END
GO

EXEC sp_ObtenerVuelosPorFecha @Fecha = '2025-11-15';
GO