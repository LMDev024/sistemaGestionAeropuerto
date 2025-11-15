CREATE FUNCTION fn_CalcularTiempoVuelo
(
    @VueloID INT
)
RETURNS INT
AS
BEGIN
    DECLARE @TiempoMinutos INT;
    
    SELECT @TiempoMinutos = DATEDIFF(MINUTE, FechaSalida, FechaLlegada)
    FROM Vuelos
    WHERE VueloID = @VueloID;
    
    RETURN ISNULL(@TiempoMinutos, 0);
END
GO

SELECT 
    V.NumeroVuelo,
    V.Origen,
    V.Destino,
    V.FechaSalida,
    V.FechaLlegada,
    dbo.fn_CalcularTiempoVuelo(V.VueloID) AS 'Duración (minutos)',
    dbo.fn_CalcularTiempoVuelo(V.VueloID) / 60 AS 'Duración (horas)',
    dbo.fn_CalcularTiempoVuelo(V.VueloID) % 60 AS 'Minutos adicionales'
FROM Vuelos V
ORDER BY V.FechaSalida;
GO