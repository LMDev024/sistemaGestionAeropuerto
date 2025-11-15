CREATE TRIGGER trg_AuditarCambiosVuelo
ON Vuelos
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF UPDATE(Estado)
    BEGIN
        INSERT INTO HistorialEstadosVuelo (VueloID, EstadoAnterior, EstadoNuevo, Observaciones)
        SELECT 
            i.VueloID,
            d.Estado AS EstadoAnterior,
            i.Estado AS EstadoNuevo,
            'Cambio automático registrado por trigger'
        FROM inserted i
        INNER JOIN deleted d ON i.VueloID = d.VueloID
        WHERE i.Estado != d.Estado;
    END
END
GO

SELECT * FROM HistorialEstadosVuelo WHERE VueloID = 2;
GO

UPDATE Vuelos
SET Estado = 'En Vuelo'
WHERE VueloID = 2;
GO
SELECT * FROM HistorialEstadosVuelo WHERE VueloID = 2 ORDER BY FechaCambio DESC;
GO
UPDATE Vuelos
SET Estado = 'Aterrizado'
WHERE VueloID = 2;
GO
SELECT 
    H.HistorialID,
    H.VueloID,
    V.NumeroVuelo,
    H.EstadoAnterior,
    H.EstadoNuevo,
    H.FechaCambio,
    H.Observaciones
FROM HistorialEstadosVuelo H
INNER JOIN Vuelos V ON H.VueloID = V.VueloID
WHERE H.VueloID = 2
ORDER BY H.FechaCambio;
GO