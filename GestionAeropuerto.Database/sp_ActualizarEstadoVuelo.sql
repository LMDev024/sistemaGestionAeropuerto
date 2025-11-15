CREATE PROCEDURE sp_ActualizarEstadoVuelo
    @VueloID INT,
    @NuevoEstado NVARCHAR(20),
    @Observaciones NVARCHAR(500) = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        IF NOT EXISTS (SELECT 1 FROM Vuelos WHERE VueloID = @VueloID)
        BEGIN
            RAISERROR('El vuelo no existe', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        DECLARE @EstadoActual NVARCHAR(20);
        SELECT @EstadoActual = Estado FROM Vuelos WHERE VueloID = @VueloID;
        IF @EstadoActual = 'Cancelado'
        BEGIN
            RAISERROR('No se puede cambiar el estado de un vuelo cancelado', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        UPDATE Vuelos
        SET Estado = @NuevoEstado
        WHERE VueloID = @VueloID;
        INSERT INTO HistorialEstadosVuelo (VueloID, EstadoAnterior, EstadoNuevo, Observaciones)
        VALUES (@VueloID, @EstadoActual, @NuevoEstado, @Observaciones);
        
        COMMIT TRANSACTION;
        
        SELECT 'Estado actualizado exitosamente' AS Mensaje, 
               @EstadoActual AS EstadoAnterior, 
               @NuevoEstado AS EstadoNuevo;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

EXEC sp_ActualizarEstadoVuelo 
    @VueloID = 1,
    @NuevoEstado = 'Abordando',
    @Observaciones = 'Abordaje iniciado en puerta A1';
GO