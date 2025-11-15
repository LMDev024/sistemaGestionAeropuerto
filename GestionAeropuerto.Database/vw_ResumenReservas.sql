CREATE VIEW vw_ResumenReservas
AS
SELECT 
    R.ReservaID,
    R.CodigoReserva,
    R.FechaReserva,
    P.TipoDocumento,
    P.NumeroDocumento,
    P.Nombre + ' ' + P.Apellido AS NombreCompleto,
    P.Email,
    P.Telefono,
    P.Nacionalidad,
    V.VueloID,
    V.NumeroVuelo,
    A.Nombre AS Aerolinea,
    V.Origen,
    V.Destino,
    V.FechaSalida,
    V.FechaLlegada,
    V.Estado AS EstadoVuelo,
    R.NumeroAsiento,
    R.Clase,
    R.Estado AS EstadoReserva,
    R.Precio,
    R.Equipaje,
    PU.Numero AS Puerta,
    PU.Terminal,
    dbo.fn_CalcularTiempoVuelo(V.VueloID) AS DuracionVuelo,
    CASE 
        WHEN R.Estado = 'Confirmada' AND V.Estado = 'Programado' THEN 'Activa'
        WHEN R.Estado = 'CheckIn' THEN 'Check-in Realizado'
        WHEN R.Estado = 'Abordado' THEN 'Pasajero Abordó'
        WHEN R.Estado = 'Cancelada' THEN 'Cancelada'
        ELSE R.Estado
    END AS EstadoDescriptivo,
    CASE 
        WHEN V.FechaSalida < DATEADD(HOUR, 2, GETDATE()) 
             AND R.Estado = 'Confirmada' 
        THEN 'Realizar Check-in'
        WHEN V.FechaSalida < DATEADD(HOUR, 1, GETDATE()) 
             AND R.Estado = 'CheckIn' 
        THEN 'Dirigirse a Puerta'
        ELSE 'En Tiempo'
    END AS Accion
FROM Reservas R
INNER JOIN Pasajeros P ON R.PasajeroID = P.PasajeroID
INNER JOIN Vuelos V ON R.VueloID = V.VueloID
INNER JOIN Aerolineas A ON V.AerolineaID = A.AerolineaID
LEFT JOIN Puertas PU ON V.PuertaID = PU.PuertaID
WHERE R.Estado != 'Cancelada';
GO


SELECT * FROM vw_ResumenReservas
ORDER BY FechaSalida;
GO

SELECT 
    CodigoReserva,
    NombreCompleto,
    NumeroVuelo,
    Destino,
    FechaSalida,
    EstadoReserva,
    Accion
FROM vw_ResumenReservas
WHERE Accion != 'En Tiempo'
ORDER BY FechaSalida;
GO

SELECT 
    Clase,
    COUNT(*) AS TotalReservas,
    SUM(Precio) AS IngresoTotal,
    AVG(Precio) AS PrecioPromedio
FROM vw_ResumenReservas
GROUP BY Clase
ORDER BY IngresoTotal DESC;
GO

SELECT 
    CodigoReserva,
    NumeroVuelo,
    Origen,
    Destino,
    FechaSalida,
    NumeroAsiento,
    Clase,
    EstadoDescriptivo,
    Puerta,
    Terminal
FROM vw_ResumenReservas
WHERE NumeroDocumento = '1234567890'
ORDER BY FechaSalida DESC;
GO