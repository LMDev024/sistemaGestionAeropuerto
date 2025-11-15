CREATE VIEW vw_EstadoPuertas
AS
SELECT 
    P.PuertaID,
    P.Numero AS NumeroPuerta,
    P.Terminal,
    P.Estado AS EstadoPuerta,
    V.VueloID,
    V.NumeroVuelo,
    A.Nombre AS Aerolinea,
    A.CodigoIATA,
    V.Origen,
    V.Destino,
    V.FechaSalida,
    V.Estado AS EstadoVuelo,
    CASE 
        WHEN P.Estado = 'Disponible' THEN 'Libre'
        WHEN P.Estado = 'Ocupada' AND V.Estado = 'Programado' THEN 'Asignada'
        WHEN P.Estado = 'Ocupada' AND V.Estado = 'Abordando' THEN 'Abordando'
        WHEN P.Estado = 'Mantenimiento' THEN 'Fuera de Servicio'
        ELSE P.Estado
    END AS EstadoDescriptivo,
    CASE 
        WHEN V.FechaSalida IS NOT NULL 
        THEN DATEDIFF(MINUTE, GETDATE(), V.FechaSalida)
        ELSE NULL
    END AS MinutosHastaSalida
FROM Puertas P
LEFT JOIN Vuelos V ON P.PuertaID = V.PuertaID 
    AND V.Estado IN ('Programado', 'Abordando', 'Retrasado')
LEFT JOIN Aerolineas A ON V.AerolineaID = A.AerolineaID;
GO



SELECT * FROM vw_EstadoPuertas
ORDER BY Terminal, NumeroPuerta;
GO

SELECT 
    NumeroPuerta,
    Terminal,
    NumeroVuelo,
    Aerolinea,
    Destino,
    FechaSalida,
    EstadoDescriptivo,
    CASE 
        WHEN MinutosHastaSalida < 0 THEN 'RETRASADO'
        WHEN MinutosHastaSalida < 30 THEN 'URGENTE'
        WHEN MinutosHastaSalida < 60 THEN 'PRÓXIMO'
        ELSE 'EN TIEMPO'
    END AS Prioridad
FROM vw_EstadoPuertas
WHERE EstadoPuerta = 'Ocupada'
ORDER BY MinutosHastaSalida;
GO

SELECT 
    Terminal,
    COUNT(*) AS PuertasDisponibles,
    STRING_AGG(NumeroPuerta, ', ') AS ListaPuertas
FROM vw_EstadoPuertas
WHERE EstadoPuerta = 'Disponible'
GROUP BY Terminal;
GO