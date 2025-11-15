CREATE VIEW vw_VuelosDelDia
AS
SELECT 
    V.VueloID,
    V.NumeroVuelo,
    A.Nombre AS Aerolinea,
    A.CodigoIATA AS CodigoAerolinea,
    AE.Modelo AS Aeronave,
    AE.Matricula,
    V.Origen,
    V.Destino,
    V.FechaSalida,
    V.FechaLlegada,
    V.Estado,
    P.Numero AS Puerta,
    P.Terminal,
    V.AsientosDisponibles,
    AE.CapacidadPasajeros AS CapacidadTotal,
    (AE.CapacidadPasajeros - V.AsientosDisponibles) AS PasajerosRegistrados,
    dbo.fn_CalcularOcupacionVuelo(V.VueloID) AS PorcentajeOcupacion,
    dbo.fn_CalcularTiempoVuelo(V.VueloID) AS DuracionMinutos,
    dbo.fn_CalcularIngresosPorVuelo(V.VueloID) AS IngresosEstimados,
    V.PrecioBase,
    CASE 
        WHEN V.Estado = 'Programado' AND V.FechaSalida < DATEADD(HOUR, 2, GETDATE()) THEN 'Próximo a Abordar'
        WHEN V.Estado = 'Programado' THEN 'En Tiempo'
        WHEN V.Estado = 'Retrasado' THEN 'Demorado'
        WHEN V.Estado = 'Abordando' THEN 'Abordando Ahora'
        WHEN V.Estado = 'En Vuelo' THEN 'En el Aire'
        WHEN V.Estado = 'Aterrizado' THEN 'Aterrizó'
        ELSE V.Estado
    END AS EstadoDescriptivo
FROM Vuelos V
INNER JOIN Aerolineas A ON V.AerolineaID = A.AerolineaID
INNER JOIN Aeronaves AE ON V.AeronaveID = AE.AeronaveID
LEFT JOIN Puertas P ON V.PuertaID = P.PuertaID
WHERE CAST(V.FechaSalida AS DATE) = CAST(GETDATE() AS DATE);
GO


SELECT * FROM vw_VuelosDelDia
ORDER BY FechaSalida;
GO

SELECT 
    NumeroVuelo,
    Aerolinea,
    Origen,
    Destino,
    FechaSalida,
    Puerta,
    Terminal,
    AsientosDisponibles,
    PorcentajeOcupacion,
    EstadoDescriptivo
FROM vw_VuelosDelDia
WHERE Estado IN ('Programado', 'Retrasado')
AND AsientosDisponibles > 0
ORDER BY FechaSalida;
GO

SELECT 
    Estado,
    COUNT(*) AS TotalVuelos,
    SUM(PasajerosRegistrados) AS TotalPasajeros,
    AVG(PorcentajeOcupacion) AS OcupacionPromedio,
    SUM(IngresosEstimados) AS IngresosDelDia
FROM vw_VuelosDelDia
GROUP BY Estado;
GO