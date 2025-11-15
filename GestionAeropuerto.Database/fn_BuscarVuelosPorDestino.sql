CREATE FUNCTION fn_BuscarVuelosPorDestino
(
    @Destino NVARCHAR(100)
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        V.VueloID,
        V.NumeroVuelo,
        A.Nombre AS Aerolinea,
        AE.Modelo AS Aeronave,
        V.Origen,
        V.Destino,
        V.FechaSalida,
        V.FechaLlegada,
        V.Estado,
        V.AsientosDisponibles,
        AE.CapacidadPasajeros,
        V.PrecioBase,
        dbo.fn_CalcularOcupacionVuelo(V.VueloID) AS PorcentajeOcupacion,
        dbo.fn_CalcularTiempoVuelo(V.VueloID) AS DuracionMinutos
    FROM Vuelos V
    INNER JOIN Aerolineas A ON V.AerolineaID = A.AerolineaID
    INNER JOIN Aeronaves AE ON V.AeronaveID = AE.AeronaveID
    WHERE V.Destino LIKE '%' + @Destino + '%'
);
GO


SELECT * FROM dbo.fn_BuscarVuelosPorDestino('Miami');
GO
SELECT 
    NumeroVuelo,
    Aerolinea,
    Origen,
    Destino,
    FechaSalida,
    AsientosDisponibles,
    PorcentajeOcupacion,
    DuracionMinutos / 60 AS 'Horas de vuelo'
FROM dbo.fn_BuscarVuelosPorDestino('BOG')
WHERE Estado = 'Programado'
ORDER BY FechaSalida;
GO