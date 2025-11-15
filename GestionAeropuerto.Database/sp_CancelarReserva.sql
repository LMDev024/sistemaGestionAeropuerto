CREATE PROCEDURE sp_CancelarReserva
    @ReservaID INT,
    @Motivo NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        DECLARE @VueloID INT, @EstadoReserva NVARCHAR(20);
        
        SELECT @VueloID = VueloID, @EstadoReserva = Estado
        FROM Reservas
        WHERE ReservaID = @ReservaID;
        
        IF @VueloID IS NULL
        BEGIN
            RAISERROR('La reserva no existe', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @EstadoReserva = 'Cancelada'
        BEGIN
            RAISERROR('La reserva ya está cancelada', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @EstadoReserva = 'Abordado'
        BEGIN
            RAISERROR('No se puede cancelar una reserva de un pasajero que ya abordó', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        UPDATE Reservas
        SET Estado = 'Cancelada'
        WHERE ReservaID = @ReservaID;
        
        UPDATE Vuelos
        SET AsientosDisponibles = AsientosDisponibles + 1
        WHERE VueloID = @VueloID;
        
        COMMIT TRANSACTION;
        
        SELECT 'Reserva cancelada exitosamente' AS Mensaje;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

EXEC sp_CancelarReserva 
    @ReservaID = 1,
    @Motivo = 'Cambio de planes del cliente';
GO