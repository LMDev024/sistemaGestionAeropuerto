CREATE FUNCTION fn_ValidarDisponibilidadAsiento
(
    @VueloID INT,
    @NumeroAsiento NVARCHAR(5)
)
RETURNS BIT
AS
BEGIN
    DECLARE @Disponible BIT = 1;
    IF EXISTS (
        SELECT 1 
        FROM Reservas 
        WHERE VueloID = @VueloID 
        AND NumeroAsiento = @NumeroAsiento 
        AND Estado != 'Cancelada'
    )
    BEGIN
        SET @Disponible = 0;
    END
    
    RETURN @Disponible;
END
GO

SELECT 
    '12A' AS Asiento,
    CASE dbo.fn_ValidarDisponibilidadAsiento(1, '12A')
        WHEN 1 THEN 'Disponible'
        ELSE 'Ocupado'
    END AS Estado;
SELECT 
    Asiento,
    CASE dbo.fn_ValidarDisponibilidadAsiento(1, Asiento)
        WHEN 1 THEN 'Disponible'
        ELSE 'Ocupado'
    END AS Estado
FROM (
    VALUES ('12A'), ('12B'), ('5C'), ('20D'), ('15E')
) AS Asientos(Asiento);
GO