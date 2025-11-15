CREATE PROCEDURE sp_RealizarReserva
    @VueloID INT,
    @PasajeroID INT,
    @NumeroAsiento NVARCHAR(5),
    @Clase NVARCHAR(20),
    @Equipaje INT = 1,
    @ReservaID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @AsientosDisponibles INT, @PrecioBase DECIMAL(10,2), @EstadoVuelo NVARCHAR(20);
        
        SELECT @AsientosDisponibles = AsientosDisponibles, 
               @PrecioBase = PrecioBase,
               @EstadoVuelo = Estado
        FROM Vuelos 
        WHERE VueloID = @VueloID;
        
        IF @AsientosDisponibles IS NULL
        BEGIN
            RAISERROR('El vuelo no existe', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @EstadoVuelo NOT IN ('Programado', 'Retrasado')
        BEGIN
            RAISERROR('No se pueden hacer reservas para este vuelo', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @AsientosDisponibles <= 0
        BEGIN
            RAISERROR('No hay asientos disponibles en este vuelo', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        IF EXISTS (SELECT 1 FROM Reservas 
                   WHERE VueloID = @VueloID 
                   AND NumeroAsiento = @NumeroAsiento 
                   AND Estado != 'Cancelada')
        BEGIN
            RAISERROR('El asiento ya está ocupado', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        IF NOT EXISTS (SELECT 1 FROM Pasajeros WHERE PasajeroID = @PasajeroID)
        BEGIN
            RAISERROR('El pasajero no existe', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        DECLARE @Precio DECIMAL(10,2);
        SET @Precio = CASE @Clase
            WHEN 'Economica' THEN @PrecioBase
            WHEN 'Ejecutiva' THEN @PrecioBase * 1.5
            WHEN 'Primera' THEN @PrecioBase * 2.5
            ELSE @PrecioBase
        END;
        DECLARE @CodigoReserva NVARCHAR(10);
        SET @CodigoReserva = 'RES' + RIGHT('000000' + CAST(ABS(CHECKSUM(NEWID())) % 1000000 AS VARCHAR), 6);
        INSERT INTO Reservas (CodigoReserva, VueloID, PasajeroID, NumeroAsiento, 
                             Clase, Estado, Precio, Equipaje)
        VALUES (@CodigoReserva, @VueloID, @PasajeroID, @NumeroAsiento, 
                @Clase, 'Confirmada', @Precio, @Equipaje);
        
        SET @ReservaID = SCOPE_IDENTITY();
        UPDATE Vuelos
        SET AsientosDisponibles = AsientosDisponibles - 1
        WHERE VueloID = @VueloID;
        
        COMMIT TRANSACTION;
        
        SELECT 'Reserva realizada exitosamente' AS Mensaje, 
               @ReservaID AS ReservaID, 
               @CodigoReserva AS CodigoReserva,
               @Precio AS Precio;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

DECLARE @NuevaReservaID INT;

EXEC sp_RealizarReserva 
    @VueloID = 7,
    @PasajeroID = 1,
    @NumeroAsiento = '15A',
    @Clase = 'Economica',
    @Equipaje = 1,
    @ReservaID = @NuevaReservaID OUTPUT;

SELECT @NuevaReservaID AS 'ID de reserva creada';
GO