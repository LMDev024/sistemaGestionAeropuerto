CREATE FUNCTION fn_CalcularOcupacionVuelo
(
    @VueloID INT
)
RETURNS DECIMAL(5,2)
AS
BEGIN
    DECLARE @PorcentajeOcupacion DECIMAL(5,2);
    DECLARE @CapacidadTotal INT;
    DECLARE @AsientosDisponibles INT;
    SELECT 
        @CapacidadTotal = AE.CapacidadPasajeros,
        @AsientosDisponibles = V.AsientosDisponibles
    FROM Vuelos V
    INNER JOIN Aeronaves AE ON V.AeronaveID = AE.AeronaveID
    WHERE V.VueloID = @VueloID;
    IF @CapacidadTotal > 0
    BEGIN
        SET @PorcentajeOcupacion = ((@CapacidadTotal - @AsientosDisponibles) * 100.0) / @CapacidadTotal;
    END
    ELSE
    BEGIN
        SET @PorcentajeOcupacion = 0;
    END
    
    RETURN @PorcentajeOcupacion;
END
GO


SELECT 
    V.VueloID,
    V.NumeroVuelo,
    AE.CapacidadPasajeros AS Capacidad,
    V.AsientosDisponibles,
    (AE.CapacidadPasajeros - V.AsientosDisponibles) AS AsientosOcupados,
    dbo.fn_CalcularOcupacionVuelo(V.VueloID) AS PorcentajeOcupacion
FROM Vuelos V
INNER JOIN Aeronaves AE ON V.AeronaveID = AE.AeronaveID
WHERE V.VueloID = 1;
GO
SELECT 
    V.NumeroVuelo,
    V.Origen,
    V.Destino,
    dbo.fn_CalcularOcupacionVuelo(V.VueloID) AS 'Ocupación %'
FROM Vuelos V
ORDER BY V.FechaSalida;
GO

