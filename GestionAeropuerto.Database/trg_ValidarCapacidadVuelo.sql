CREATE TRIGGER trg_ValidarCapacidadVuelo
ON Reservas
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @VueloID INT;
    DECLARE @AsientosDisponibles INT;
    DECLARE @NumeroAsiento NVARCHAR(5);
    SELECT 
        @VueloID = VueloID,
        @NumeroAsiento = NumeroAsiento
    FROM inserted;
    SELECT @AsientosDisponibles = AsientosDisponibles
    FROM Vuelos
    WHERE VueloID = @VueloID;
    IF @AsientosDisponibles <= 0
    BEGIN
        RAISERROR('No hay asientos disponibles en este vuelo', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
    IF EXISTS (
        SELECT 1 
        FROM Reservas 
        WHERE VueloID = @VueloID 
        AND NumeroAsiento = @NumeroAsiento 
        AND Estado != 'Cancelada'
    )
    BEGIN
        RAISERROR('El asiento ya está ocupado', 16, 1);
        ROLLBACK TRANSACTION;
        RETURN;
    END
    INSERT INTO Reservas (CodigoReserva, VueloID, PasajeroID, NumeroAsiento, 
                         Clase, Estado, Precio, Equipaje, FechaReserva)
    SELECT 
        CodigoReserva, VueloID, PasajeroID, NumeroAsiento, 
        Clase, Estado, Precio, Equipaje, FechaReserva
    FROM inserted;
    UPDATE Vuelos
    SET AsientosDisponibles = AsientosDisponibles - 1
    WHERE VueloID = @VueloID;
    
    PRINT 'Reserva creada exitosamente. Trigger de validación ejecutado.';
END
GO


INSERT INTO Reservas (CodigoReserva, VueloID, PasajeroID, NumeroAsiento, Clase, Estado, Precio, Equipaje)
VALUES ('TEST001', 7, 2, '25A', 'Economica', 'Confirmada', 180000, 1);
GO

SELECT * FROM Reservas WHERE CodigoReserva = 'TEST001';
GO

BEGIN TRY
    INSERT INTO Reservas (CodigoReserva, VueloID, PasajeroID, NumeroAsiento, Clase, Estado, Precio, Equipaje)
    VALUES ('TEST002', 7, 3, '25A', 'Economica', 'Confirmada', 180000, 1);
END TRY
BEGIN CATCH
    PRINT 'Error capturado (esperado): ' + ERROR_MESSAGE();
END CATCH
GO

SELECT 
    VueloID,
    NumeroVuelo,
    AsientosDisponibles
FROM Vuelos
WHERE VueloID = 7;
GO