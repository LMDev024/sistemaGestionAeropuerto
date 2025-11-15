CREATE TRIGGER trg_CancelarReservasVueloCancelado
ON Vuelos
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF UPDATE(Estado)
    BEGIN
        UPDATE R
        SET R.Estado = 'Cancelada'
        FROM Reservas R
        INNER JOIN inserted i ON R.VueloID = i.VueloID
        WHERE i.Estado = 'Cancelado'
        AND R.Estado NOT IN ('Cancelada', 'Abordado');
        INSERT INTO LogNotificaciones (VueloID, TipoNotificacion, Mensaje)
        SELECT 
            i.VueloID,
            'Vuelo Cancelado',
            'Vuelo ' + i.NumeroVuelo + ' cancelado. ' + 
            CAST(COUNT(R.ReservaID) AS NVARCHAR) + ' reservas fueron canceladas automáticamente.'
        FROM inserted i
        INNER JOIN deleted d ON i.VueloID = d.VueloID
        LEFT JOIN Reservas R ON i.VueloID = R.VueloID 
            AND R.Estado = 'Cancelada'
        WHERE i.Estado = 'Cancelado' 
        AND d.Estado != 'Cancelado'
        GROUP BY i.VueloID, i.NumeroVuelo;
    END
END
GO

SELECT 
    R.ReservaID,
    R.CodigoReserva,
    V.NumeroVuelo,
    P.Nombre + ' ' + P.Apellido AS Pasajero,
    R.Estado
FROM Reservas R
INNER JOIN Vuelos V ON R.VueloID = V.VueloID
INNER JOIN Pasajeros P ON R.PasajeroID = P.PasajeroID
WHERE V.VueloID = 5;
GO

UPDATE Vuelos
SET Estado = 'Cancelado'
WHERE VueloID = 5;
GO

SELECT 
    R.ReservaID,
    R.CodigoReserva,
    V.NumeroVuelo,
    P.Nombre + ' ' + P.Apellido AS Pasajero,
    R.Estado
FROM Reservas R
INNER JOIN Vuelos V ON R.VueloID = V.VueloID
INNER JOIN Pasajeros P ON R.PasajeroID = P.PasajeroID
WHERE V.VueloID = 5;
GO

SELECT * FROM LogNotificaciones 
WHERE TipoNotificacion = 'Vuelo Cancelado'
ORDER BY FechaNotificacion DESC;
GO