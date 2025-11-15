
USE GestionAeropuerto
GO

CREATE PROCEDURE sp_CrearVuelo
    @NumeroVuelo NVARCHAR(10),
    @AerolineaID INT,
    @AeronaveID INT,
    @Origen NVARCHAR(100),
    @Destino NVARCHAR(100),
    @FechaSalida DATETIME,
    @FechaLlegada DATETIME,
    @PrecioBase DECIMAL(10,2),
    @VueloID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        IF @FechaSalida >= @FechaLlegada
        BEGIN
            RAISERROR('La fecha de llegada debe ser posterior a la fecha de salida', 16, 1);
            RETURN;
        END      
        IF NOT EXISTS (SELECT 1 FROM Aeronaves WHERE AeronaveID = @AeronaveID AND Estado = 'Operativo')
        BEGIN
            RAISERROR('La aeronave no existe o no está operativa', 16, 1);
            RETURN;
        END
        DECLARE @Capacidad INT;
        SELECT @Capacidad = CapacidadPasajeros 
        FROM Aeronaves 
        WHERE AeronaveID = @AeronaveID;
        INSERT INTO Vuelos (NumeroVuelo, AerolineaID, AeronaveID, Origen, Destino, 
                           FechaSalida, FechaLlegada, Estado, AsientosDisponibles, PrecioBase)
        VALUES (@NumeroVuelo, @AerolineaID, @AeronaveID, @Origen, @Destino, 
                @FechaSalida, @FechaLlegada, 'Programado', @Capacidad, @PrecioBase);
        SET @VueloID = SCOPE_IDENTITY();
        INSERT INTO HistorialEstadosVuelo (VueloID, EstadoAnterior, EstadoNuevo, Observaciones)
        VALUES (@VueloID, NULL, 'Programado', 'Vuelo creado'); 
        SELECT 'Vuelo creado exitosamente' AS Mensaje, @VueloID AS VueloID;
        
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

-- Prueba: Crear un vuelo nuevo
DECLARE @NuevoVueloID INT;

EXEC sp_CrearVuelo 
    @NumeroVuelo = 'AV999',
    @AerolineaID = 1,
    @AeronaveID = 1,
    @Origen = 'Barranquilla (BAQ)',
    @Destino = 'Cartagena (CTG)',
    @FechaSalida = '2024-11-16 08:00:00',
    @FechaLlegada = '2024-11-16 09:00:00',
    @PrecioBase = 150000,
    @VueloID = @NuevoVueloID OUTPUT;

SELECT @NuevoVueloID AS 'ID del vuelo creado';
GO