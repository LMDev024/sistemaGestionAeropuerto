CREATE FUNCTION fn_ObtenerReservasPorPasajero
(
    @PasajeroID INT
)
RETURNS TABLE
AS
RETURN
(
    SELECT 
        R.ReservaID,
        R.CodigoReserva,
        V.NumeroVuelo,
        A.Nombre AS Aerolinea,
        V.Origen,
        V.Destino,
        V.FechaSalida,
        V.FechaLlegada,
        V.Estado AS EstadoVuelo,
        R.NumeroAsiento,
        R.Clase,
        R.Estado AS EstadoReserva,
        R.Precio,
        R.Equipaje,
        R.FechaReserva
    FROM Reservas R
    INNER JOIN Vuelos V ON R.VueloID = V.VueloID
    INNER JOIN Aerolineas A ON V.AerolineaID = A.AerolineaID
    WHERE R.PasajeroID = @PasajeroID
);
GO


SELECT * FROM dbo.fn_ObtenerReservasPorPasajero(1);
GO

SELECT 
    P.Nombre + ' ' + P.Apellido AS Pasajero,
    P.NumeroDocumento,
    R.*
FROM Pasajeros P
CROSS APPLY dbo.fn_ObtenerReservasPorPasajero(P.PasajeroID) R
WHERE P.PasajeroID = 1;
GO