CREATE PROCEDURE sp_ObtenerPasajerosPorVuelo
    @VueloID INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        R.ReservaID,
        R.CodigoReserva,
        P.TipoDocumento,
        P.NumeroDocumento,
        P.Nombre + ' ' + P.Apellido AS NombreCompleto,
        P.Nacionalidad,
        R.NumeroAsiento,
        R.Clase,
        R.Estado AS EstadoReserva,
        R.Equipaje,
        R.Precio,
        R.FechaReserva
    FROM Reservas R
    INNER JOIN Pasajeros P ON R.PasajeroID = P.PasajeroID
    WHERE R.VueloID = @VueloID
    AND R.Estado != 'Cancelada'
    ORDER BY R.NumeroAsiento;
END
GO

EXEC sp_ObtenerPasajerosPorVuelo @VueloID = 1;
GO