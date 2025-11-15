CREATE FUNCTION fn_ObtenerVuelosDisponibles
(
    @Fecha DATE
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        V.VueloID,
        V.NumeroVuelo,
        A.Nombre AS Aerolinea,
        A.CodigoIATA,
        V.Origen,
        V.Destino,
        V.FechaSalida,
        V.FechaLlegada,
        V.Estado,
        P.Numero AS Puerta,
        P.Terminal,
        V.AsientosDisponibles,
        AE.CapacidadPasajeros AS CapacidadTotal,
        V.PrecioBase,
        dbo.fn_CalcularOcupacionVuelo(V.VueloID) AS PorcentajeOcupacion
    FROM Vuelos V
    INNER JOIN Aerolineas A ON V.AerolineaID = A.AerolineaID
    INNER JOIN Aeronaves AE ON V.AeronaveID = AE.AeronaveID
    LEFT JOIN Puertas P ON V.PuertaID = P.PuertaID
    WHERE CAST(V.FechaSalida AS DATE) = @Fecha
    AND V.Estado IN ('Programado', 'Retrasado')
    AND V.AsientosDisponibles > 0
);
GO

SELECT * FROM dbo.fn_ObtenerVuelosDisponibles('2025-11-15');
GO

SELECT 
    NumeroVuelo,
    Aerolinea,
    Origen,
    Destino,
    FechaSalida,
    AsientosDisponibles,
    PorcentajeOcupacion
FROM dbo.fn_ObtenerVuelosDisponibles('2025-11-15')
WHERE AsientosDisponibles > 50
ORDER BY FechaSalida;
GO