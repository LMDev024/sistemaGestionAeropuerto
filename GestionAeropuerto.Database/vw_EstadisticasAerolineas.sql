CREATE VIEW vw_EstadisticasAerolineas
AS
SELECT 
    A.AerolineaID,
    A.Nombre AS Aerolinea,
    A.CodigoIATA,
    A.Pais,
    COUNT(DISTINCT AE.AeronaveID) AS TotalAeronaves,
    COUNT(DISTINCT V.VueloID) AS TotalVuelos,
    COUNT(DISTINCT CASE WHEN V.Estado = 'Programado' THEN V.VueloID END) AS VuelosProgramados,
    COUNT(DISTINCT CASE WHEN V.Estado = 'En Vuelo' THEN V.VueloID END) AS VuelosEnAire,
    COUNT(DISTINCT CASE WHEN V.Estado = 'Aterrizado' THEN V.VueloID END) AS VuelosAterrizados,
    COUNT(DISTINCT CASE WHEN V.Estado = 'Cancelado' THEN V.VueloID END) AS VuelosCancelados,
    COUNT(DISTINCT R.ReservaID) AS TotalReservas,
    COUNT(DISTINCT CASE WHEN R.Estado = 'Confirmada' THEN R.ReservaID END) AS ReservasConfirmadas,
    COUNT(DISTINCT CASE WHEN R.Estado = 'Cancelada' THEN R.ReservaID END) AS ReservasCanceladas,
    SUM(CASE WHEN R.Estado != 'Cancelada' THEN R.Precio ELSE 0 END) AS IngresosTotal,
    AVG(CASE 
        WHEN V.VueloID IS NOT NULL 
        THEN dbo.fn_CalcularOcupacionVuelo(V.VueloID) 
    END) AS OcupacionPromedio,
    SUM(AE.CapacidadPasajeros - ISNULL(V.AsientosDisponibles, AE.CapacidadPasajeros)) AS TotalPasajerosTransportados
FROM Aerolineas A
LEFT JOIN Aeronaves AE ON A.AerolineaID = AE.AerolineaID
LEFT JOIN Vuelos V ON AE.AeronaveID = V.AeronaveID
LEFT JOIN Reservas R ON V.VueloID = R.VueloID
WHERE A.Activo = 1
GROUP BY A.AerolineaID, A.Nombre, A.CodigoIATA, A.Pais;
GO


SELECT * FROM vw_EstadisticasAerolineas
ORDER BY TotalVuelos DESC;
GO
SELECT TOP 3
    Aerolinea,
    CodigoIATA,
    TotalVuelos,
    TotalReservas,
    FORMAT(IngresosTotal, 'C', 'es-CO') AS Ingresos,
    CAST(OcupacionPromedio AS DECIMAL(5,2)) AS 'Ocupación %'
FROM vw_EstadisticasAerolineas
ORDER BY IngresosTotal DESC;
GO
SELECT 
    Aerolinea,
    TotalVuelos,
    TotalPasajerosTransportados,
    CAST(OcupacionPromedio AS DECIMAL(5,2)) AS 'Ocupación Promedio %'
FROM vw_EstadisticasAerolineas
WHERE TotalVuelos > 0
ORDER BY OcupacionPromedio DESC;
GO