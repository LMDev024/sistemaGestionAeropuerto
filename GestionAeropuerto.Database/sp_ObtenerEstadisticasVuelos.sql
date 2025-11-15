
CREATE PROCEDURE sp_ObtenerEstadisticasVuelos
    @FechaInicio DATE = NULL,
    @FechaFin DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;
    IF @FechaInicio IS NULL
        SET @FechaInicio = CAST(GETDATE() AS DATE);
    
    IF @FechaFin IS NULL
        SET @FechaFin = CAST(GETDATE() AS DATE);

    SELECT 
        V.Estado,
        COUNT(*) AS TotalVuelos,
        SUM(AE.CapacidadPasajeros - V.AsientosDisponibles) AS TotalPasajeros
    FROM Vuelos V
    INNER JOIN Aeronaves AE ON V.AeronaveID = AE.AeronaveID
    WHERE CAST(V.FechaSalida AS DATE) BETWEEN @FechaInicio AND @FechaFin
    GROUP BY V.Estado
    ORDER BY TotalVuelos DESC;
    SELECT 
        A.Nombre AS Aerolinea,
        COUNT(V.VueloID) AS TotalVuelos,
        SUM(AE.CapacidadPasajeros - V.AsientosDisponibles) AS TotalPasajeros,
        AVG(CAST((AE.CapacidadPasajeros - V.AsientosDisponibles) * 100.0 / AE.CapacidadPasajeros AS DECIMAL(5,2))) AS PorcentajeOcupacion
    FROM Vuelos V
    INNER JOIN Aerolineas A ON V.AerolineaID = A.AerolineaID
    INNER JOIN Aeronaves AE ON V.AeronaveID = AE.AeronaveID
    WHERE CAST(V.FechaSalida AS DATE) BETWEEN @FechaInicio AND @FechaFin
    GROUP BY A.Nombre
    ORDER BY TotalVuelos DESC;
END
GO

EXEC sp_ObtenerEstadisticasVuelos;
GO

EXEC sp_ObtenerEstadisticasVuelos 
    @FechaInicio = '2025-11-15',
    @FechaFin = '2025-11-17';
GO

