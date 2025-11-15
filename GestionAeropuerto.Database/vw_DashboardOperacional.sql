CREATE VIEW vw_DashboardOperacional
AS
SELECT 

    CAST(GETDATE() AS DATE) AS Fecha,
    

    (SELECT COUNT(*) FROM Vuelos 
     WHERE CAST(FechaSalida AS DATE) = CAST(GETDATE() AS DATE)) AS TotalVuelosHoy,
     
    (SELECT COUNT(*) FROM Vuelos 
     WHERE CAST(FechaSalida AS DATE) = CAST(GETDATE() AS DATE) 
     AND Estado = 'Programado') AS VuelosProgramados,
     
    (SELECT COUNT(*) FROM Vuelos 
     WHERE CAST(FechaSalida AS DATE) = CAST(GETDATE() AS DATE) 
     AND Estado = 'Abordando') AS VuelosAbordando,
     
    (SELECT COUNT(*) FROM Vuelos 
     WHERE CAST(FechaSalida AS DATE) = CAST(GETDATE() AS DATE) 
     AND Estado = 'En Vuelo') AS VuelosEnAire,
     
    (SELECT COUNT(*) FROM Vuelos 
     WHERE CAST(FechaSalida AS DATE) = CAST(GETDATE() AS DATE) 
     AND Estado = 'Aterrizado') AS VuelosAterrizados,
     
    (SELECT COUNT(*) FROM Vuelos 
     WHERE CAST(FechaSalida AS DATE) = CAST(GETDATE() AS DATE) 
     AND Estado = 'Retrasado') AS VuelosRetrasados,
     
    (SELECT COUNT(*) FROM Vuelos 
     WHERE CAST(FechaSalida AS DATE) = CAST(GETDATE() AS DATE) 
     AND Estado = 'Cancelado') AS VuelosCancelados,
     

    (SELECT COUNT(*) FROM Puertas 
     WHERE Estado = 'Disponible') AS PuertasDisponibles,
     
    (SELECT COUNT(*) FROM Puertas 
     WHERE Estado = 'Ocupada') AS PuertasOcupadas,
     
    (SELECT COUNT(*) FROM Puertas 
     WHERE Estado = 'Mantenimiento') AS PuertasMantenimiento,
     

    (SELECT COUNT(*) FROM Reservas R
     INNER JOIN Vuelos V ON R.VueloID = V.VueloID
     WHERE CAST(V.FechaSalida AS DATE) = CAST(GETDATE() AS DATE)
     AND R.Estado != 'Cancelada') AS TotalReservasHoy,
     
    (SELECT COUNT(*) FROM Reservas R
     INNER JOIN Vuelos V ON R.VueloID = V.VueloID
     WHERE CAST(V.FechaSalida AS DATE) = CAST(GETDATE() AS DATE)
     AND R.Estado = 'Confirmada') AS ReservasConfirmadas,
     
    (SELECT COUNT(*) FROM Reservas R
     INNER JOIN Vuelos V ON R.VueloID = V.VueloID
     WHERE CAST(V.FechaSalida AS DATE) = CAST(GETDATE() AS DATE)
     AND R.Estado = 'CheckIn') AS CheckInsRealizados,
     
    (SELECT COUNT(*) FROM Reservas R
     INNER JOIN Vuelos V ON R.VueloID = V.VueloID
     WHERE CAST(V.FechaSalida AS DATE) = CAST(GETDATE() AS DATE)
     AND R.Estado = 'Abordado') AS PasajerosAbordados,
     

    (SELECT ISNULL(SUM(R.Precio), 0) FROM Reservas R
     INNER JOIN Vuelos V ON R.VueloID = V.VueloID
     WHERE CAST(V.FechaSalida AS DATE) = CAST(GETDATE() AS DATE)
     AND R.Estado != 'Cancelada') AS IngresosEstimadosHoy,
     

    (SELECT AVG(dbo.fn_CalcularOcupacionVuelo(VueloID)) 
     FROM Vuelos 
     WHERE CAST(FechaSalida AS DATE) = CAST(GETDATE() AS DATE)) AS OcupacionPromedioHoy;
GO


PRINT '========================================';
PRINT '   DASHBOARD OPERACIONAL';
PRINT '========================================';

SELECT 
    '*** VUELOS ***' AS Categoria,
    '' AS Detalle,
    NULL AS Valor
UNION ALL
SELECT 'Total Vuelos Hoy', '', TotalVuelosHoy FROM vw_DashboardOperacional
UNION ALL
SELECT 'Programados', '', VuelosProgramados FROM vw_DashboardOperacional
UNION ALL
SELECT 'Abordando', '', VuelosAbordando FROM vw_DashboardOperacional
UNION ALL
SELECT 'En Vuelo', '', VuelosEnAire FROM vw_DashboardOperacional
UNION ALL
SELECT 'Aterrizados', '', VuelosAterrizados FROM vw_DashboardOperacional
UNION ALL
SELECT 'Retrasados', '', VuelosRetrasados FROM vw_DashboardOperacional
UNION ALL
SELECT 'Cancelados', '', VuelosCancelados FROM vw_DashboardOperacional

UNION ALL
SELECT '', '', NULL
UNION ALL
SELECT '*** PUERTAS ***', '', NULL
UNION ALL
SELECT 'Disponibles', '', PuertasDisponibles FROM vw_DashboardOperacional
UNION ALL
SELECT 'Ocupadas', '', PuertasOcupadas FROM vw_DashboardOperacional
UNION ALL
SELECT 'Mantenimiento', '', PuertasMantenimiento FROM vw_DashboardOperacional

UNION ALL
SELECT '', '', NULL
UNION ALL
SELECT '*** PASAJEROS ***', '', NULL
UNION ALL
SELECT 'Total Reservas', '', TotalReservasHoy FROM vw_DashboardOperacional
UNION ALL
SELECT 'Confirmadas', '', ReservasConfirmadas FROM vw_DashboardOperacional
UNION ALL
SELECT 'Check-ins', '', CheckInsRealizados FROM vw_DashboardOperacional
UNION ALL
SELECT 'Abordados', '', PasajerosAbordados FROM vw_DashboardOperacional

UNION ALL
SELECT '', '', NULL
UNION ALL
SELECT '*** FINANCIERO ***', '', NULL
UNION ALL
SELECT 'Ingresos Hoy', 'COP', IngresosEstimadosHoy FROM vw_DashboardOperacional
UNION ALL
SELECT 'Ocupación Promedio', '%', CAST(OcupacionPromedioHoy AS DECIMAL(5,2)) FROM vw_DashboardOperacional;
GO