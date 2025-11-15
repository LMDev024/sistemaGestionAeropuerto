CREATE PROCEDURE sp_AsignarPuerta
    @VueloID INT,
    @PuertaID INT
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
        DECLARE @EstadoPuerta NVARCHAR(20);
        SELECT @EstadoPuerta = Estado FROM Puertas WHERE PuertaID = @PuertaID;
        
        IF @EstadoPuerta IS NULL
        BEGIN
            RAISERROR('La puerta no existe', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        
        IF @EstadoPuerta != 'Disponible'
        BEGIN
            RAISERROR('La puerta no está disponible', 16, 1);
            ROLLBACK TRANSACTION;
            RETURN;
        END
        DECLARE @PuertaAnterior INT;
        SELECT @PuertaAnterior = PuertaID FROM Vuelos WHERE VueloID = @VueloID;
        
        IF @PuertaAnterior IS NOT NULL
        BEGIN
            UPDATE Puertas
            SET Estado = 'Disponible'
            WHERE PuertaID = @PuertaAnterior;
        END
        UPDATE Vuelos
        SET PuertaID = @PuertaID
        WHERE VueloID = @VueloID;
        UPDATE Puertas
        SET Estado = 'Ocupada'
        WHERE PuertaID = @PuertaID;
        
        COMMIT TRANSACTION;
        
        SELECT 'Puerta asignada exitosamente' AS Mensaje;
        
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
            
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

EXEC sp_AsignarPuerta 
    @VueloID = 7,
    @PuertaID = 7;
GO