
CREATE TABLE LogNotificaciones (
    LogID INT PRIMARY KEY IDENTITY(1,1),
    VueloID INT NOT NULL,
    TipoNotificacion NVARCHAR(50) NOT NULL,
    Mensaje NVARCHAR(500) NOT NULL,
    FechaNotificacion DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (VueloID) REFERENCES Vuelos(VueloID)
);
GO

CREATE TRIGGER trg_NotificarCambioPuerta
ON Vuelos
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    IF UPDATE(PuertaID)
    BEGIN
        INSERT INTO LogNotificaciones (VueloID, TipoNotificacion, Mensaje)
        SELECT 
            i.VueloID,
            'Cambio de Puerta',
            'Vuelo ' + i.NumeroVuelo + 
            ': Puerta cambiada de ' + 
            ISNULL(Pd.Numero, 'Sin asignar') + 
            ' a ' + 
            ISNULL(Pn.Numero, 'Sin asignar')
        FROM inserted i
        INNER JOIN deleted d ON i.VueloID = d.VueloID
        LEFT JOIN Puertas Pd ON d.PuertaID = Pd.PuertaID
        LEFT JOIN Puertas Pn ON i.PuertaID = Pn.PuertaID
        WHERE ISNULL(i.PuertaID, 0) != ISNULL(d.PuertaID, 0);
    END
END
GO


SELECT * FROM LogNotificaciones;
GO
UPDATE Vuelos
SET PuertaID = 9
WHERE VueloID = 4;
GO
SELECT 
    L.LogID,
    L.VueloID,
    V.NumeroVuelo,
    L.TipoNotificacion,
    L.Mensaje,
    L.FechaNotificacion
FROM LogNotificaciones L
INNER JOIN Vuelos V ON L.VueloID = V.VueloID
ORDER BY L.FechaNotificacion DESC;
GO
UPDATE Vuelos
SET PuertaID = 10
WHERE VueloID = 4;
GO
SELECT * FROM LogNotificaciones ORDER BY FechaNotificacion DESC;
GO