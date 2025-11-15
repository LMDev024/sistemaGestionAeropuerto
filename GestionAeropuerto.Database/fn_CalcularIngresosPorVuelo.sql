CREATE FUNCTION fn_CalcularIngresosPorVuelo
(
    @VueloID INT
)
RETURNS DECIMAL(12,2)
AS
BEGIN
    DECLARE @IngresoTotal DECIMAL(12,2);
    
    SELECT @IngresoTotal = SUM(Precio)
    FROM Reservas
    WHERE VueloID = @VueloID
    AND Estado IN ('Confirmada', 'CheckIn', 'Abordado');
    
    RETURN ISNULL(@IngresoTotal, 0);
END
GO

SELECT 
    V.NumeroVuelo,
    V.Origen,
    V.Destino,
    COUNT(R.ReservaID) AS TotalReservas,
    dbo.fn_CalcularIngresosPorVuelo(V.VueloID) AS 'Ingresos Totales',
    V.PrecioBase,
    dbo.fn_CalcularOcupacionVuelo(V.VueloID) AS 'Ocupación %'
FROM Vuelos V
LEFT JOIN Reservas R ON V.VueloID = R.VueloID AND R.Estado != 'Cancelada'
GROUP BY V.VueloID, V.NumeroVuelo, V.Origen, V.Destino, V.PrecioBase
ORDER BY dbo.fn_CalcularIngresosPorVuelo(V.VueloID) DESC;
GO