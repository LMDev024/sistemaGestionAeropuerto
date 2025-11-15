
-- Usar la base de datos
USE GestionAeropuerto;
GO
INSERT INTO Aerolineas (Nombre, CodigoIATA, Pais, Telefono, Email, Activo) VALUES
('Avianca', 'AV', 'Colombia', '+57-1-5878888', 'contacto@avianca.com', 1),
('LATAM Airlines', 'LA', 'Chile', '+57-1-7449090', 'info@latam.com', 1),
('Copa Airlines', 'CM', 'Panamá', '+507-2172672', 'servicio@copaair.com', 1),
('American Airlines', 'AA', 'Estados Unidos', '+1-800-433-7300', 'support@aa.com', 1),
('Iberia', 'IB', 'España', '+34-901-111500', 'contacto@iberia.es', 1),
('Viva Air', 'VH', 'Colombia', '+57-1-4891111', 'ayuda@vivaair.co', 1);
GO
INSERT INTO Aeronaves (Matricula, Modelo, Fabricante, CapacidadPasajeros, AerolineaID, AnioFabricacion, Estado) VALUES
('HK-5001', 'Boeing 787-8', 'Boeing', 240, 1, 2019, 'Operativo'),
('HK-5002', 'Airbus A320', 'Airbus', 180, 1, 2020, 'Operativo'),
('HK-5003', 'Boeing 737-800', 'Boeing', 189, 1, 2018, 'Mantenimiento'),
('CC-BGL', 'Boeing 767-300', 'Boeing', 252, 2, 2017, 'Operativo'),
('CC-BGM', 'Airbus A321', 'Airbus', 220, 2, 2021, 'Operativo'),
('HP-1825', 'Boeing 737-MAX 9', 'Boeing', 166, 3, 2022, 'Operativo'),
('HP-1826', 'Boeing 737-800', 'Boeing', 160, 3, 2019, 'Operativo'),
('N12345', 'Boeing 777-200', 'Boeing', 305, 4, 2016, 'Operativo'),
('N67890', 'Airbus A330-200', 'Airbus', 290, 4, 2018, 'Operativo'),
('EC-LUB', 'Airbus A330-300', 'Airbus', 288, 5, 2015, 'Operativo');
GO
-- Insertar Puertas de Embarque
INSERT INTO Puertas (Numero, Terminal, Estado) VALUES
('A1', 'Terminal 1', 'Disponible'),
('A2', 'Terminal 1', 'Disponible'),
('A3', 'Terminal 1', 'Disponible'),
('A4', 'Terminal 1', 'Disponible'),
('B1', 'Terminal 1', 'Disponible'),
('B2', 'Terminal 1', 'Disponible'),
('B3', 'Terminal 1', 'Disponible'),
('C1', 'Terminal 2', 'Disponible'),
('C2', 'Terminal 2', 'Disponible'),
('C3', 'Terminal 2', 'Disponible'),
('D1', 'Terminal 2', 'Mantenimiento'),
('D2', 'Terminal 2', 'Disponible');
GO
-- Insertar Vuelos (Vuelos para HOY y próximos días)
DECLARE @Hoy DATETIME = CAST(GETDATE() AS DATE);
INSERT INTO Vuelos (NumeroVuelo, AerolineaID, AeronaveID, Origen, Destino, FechaSalida, FechaLlegada, Estado, PuertaID, AsientosDisponibles, PrecioBase) VALUES
-- Vuelos de hoy
('AV101', 1, 1, 'Bogotá (BOG)', 'Miami (MIA)', DATEADD(HOUR, 2, @Hoy), DATEADD(HOUR, 6, @Hoy), 'Programado', 1, 240, 450000),
('AV102', 1, 2, 'Medellín (MDE)', 'Ciudad de México (MEX)', DATEADD(HOUR, 3, @Hoy), DATEADD(HOUR, 7, @Hoy), 'Programado', 2, 180, 380000),
('LA201', 2, 4, 'Bogotá (BOG)', 'Lima (LIM)', DATEADD(HOUR, 1, @Hoy), DATEADD(HOUR, 4, @Hoy), 'Abordando', 3, 252, 320000),
('CM301', 3, 6, 'Cartagena (CTG)', 'Panamá (PTY)', DATEADD(HOUR, 4, @Hoy), DATEADD(HOUR, 6, @Hoy), 'Programado', 4, 166, 280000),
('AA401', 4, 8, 'Bogotá (BOG)', 'Nueva York (JFK)', DATEADD(HOUR, 5, @Hoy), DATEADD(HOUR, 12, @Hoy), 'Programado', 5, 305, 850000),
('IB501', 5, 10, 'Bogotá (BOG)', 'Madrid (MAD)', DATEADD(HOUR, 6, @Hoy), DATEADD(HOUR, 16, @Hoy), 'Programado', 6, 288, 950000),

-- Vuelos de mañana
('AV103', 1, 1, 'Cali (CLO)', 'Bogotá (BOG)', DATEADD(HOUR, 26, @Hoy), DATEADD(HOUR, 27, @Hoy), 'Programado', NULL, 240, 180000),
('LA202', 2, 5, 'Bogotá (BOG)', 'Santiago (SCL)', DATEADD(HOUR, 28, @Hoy), DATEADD(HOUR, 34, @Hoy), 'Programado', NULL, 220, 520000),
('CM302', 3, 7, 'Bogotá (BOG)', 'San José (SJO)', DATEADD(HOUR, 30, @Hoy), DATEADD(HOUR, 33, @Hoy), 'Programado', NULL, 160, 350000),
('AA402', 4, 9, 'Medellín (MDE)', 'Fort Lauderdale (FLL)', DATEADD(HOUR, 32, @Hoy), DATEADD(HOUR, 36, @Hoy), 'Programado', NULL, 290, 480000),

-- Vuelos pasados (para historial)
('AV100', 1, 2, 'Bogotá (BOG)', 'Cancún (CUN)', DATEADD(HOUR, -3, @Hoy), DATEADD(HOUR, -1, @Hoy), 'Aterrizado', 7, 0, 420000),
('LA200', 2, 4, 'Lima (LIM)', 'Bogotá (BOG)', DATEADD(HOUR, -5, @Hoy), DATEADD(HOUR, -2, @Hoy), 'Aterrizado', 8, 0, 330000);
GO
-- Insertar Pasajeros
INSERT INTO Pasajeros (TipoDocumento, NumeroDocumento, Nombre, Apellido, FechaNacimiento, Nacionalidad, Email, Telefono) VALUES
('CC', '1234567890', 'Juan', 'Pérez García', '1985-03-15', 'Colombiana', 'juan.perez@email.com', '+57-300-1234567'),
('CC', '9876543210', 'María', 'Rodríguez López', '1990-07-22', 'Colombiana', 'maria.rodriguez@email.com', '+57-310-9876543'),
('Pasaporte', 'AB123456', 'John', 'Smith', '1982-11-30', 'Estadounidense', 'john.smith@email.com', '+1-305-5551234'),
('CC', '1122334455', 'Carlos', 'González Ruiz', '1995-01-10', 'Colombiana', 'carlos.gonzalez@email.com', '+57-320-1122334'),
('Pasaporte', 'CD789012', 'Emily', 'Johnson', '1988-05-18', 'Canadiense', 'emily.johnson@email.com', '+1-416-5555678'),
('CC', '5566778899', 'Ana', 'Martínez Silva', '1992-09-25', 'Colombiana', 'ana.martinez@email.com', '+57-315-5566778'),
('Pasaporte', 'EF345678', 'David', 'Brown', '1980-12-05', 'Británico', 'david.brown@email.com', '+44-20-55551234'),
('CC', '2233445566', 'Laura', 'Hernández Castro', '1987-04-14', 'Colombiana', 'laura.hernandez@email.com', '+57-318-2233445'),
('Pasaporte', 'GH901234', 'Sophie', 'Martin', '1993-08-20', 'Francesa', 'sophie.martin@email.com', '+33-1-55551234'),
('CC', '3344556677', 'Diego', 'López Vargas', '1991-06-08', 'Colombiana', 'diego.lopez@email.com', '+57-321-3344556');
GO
-- Insertar Reservas
INSERT INTO Reservas (CodigoReserva, VueloID, PasajeroID, NumeroAsiento, Clase, Estado, Precio, Equipaje) VALUES
-- Reservas para vuelos de hoy
('ABC123', 1, 1, '12A', 'Economica', 'Confirmada', 450000, 1),
('ABC124', 1, 2, '12B', 'Economica', 'CheckIn', 450000, 2),
('ABC125', 2, 3, '5C', 'Ejecutiva', 'Confirmada', 680000, 1),
('ABC126', 3, 4, '8A', 'Economica', 'CheckIn', 320000, 1),
('ABC127', 3, 5, '8B', 'Economica', 'Abordado', 320000, 1),
('ABC128', 4, 6, '15D', 'Economica', 'Confirmada', 280000, 1),
('ABC129', 5, 7, '2A', 'Primera', 'Confirmada', 1500000, 2),
('ABC130', 5, 8, '2B', 'Primera', 'Confirmada', 1500000, 2),
('ABC131', 6, 9, '10E', 'Ejecutiva', 'Confirmada', 1350000, 1),
('ABC132', 6, 10, '10F', 'Ejecutiva', 'Confirmada', 1350000, 1),

-- Reservas para vuelos de mañana
('ABC133', 7, 1, '20A', 'Economica', 'Confirmada', 180000, 1),
('ABC134', 8, 2, '25C', 'Economica', 'Confirmada', 520000, 1),
('ABC135', 9, 3, '18B', 'Economica', 'Confirmada', 350000, 1),

-- Reservas para vuelos pasados
('ABC136', 11, 4, '14A', 'Economica', 'Abordado', 420000, 1),
('ABC137', 12, 5, '22C', 'Economica', 'Abordado', 330000, 1);
GO



SELECT * FROM Aerolineas;
GO
SELECT * FROM Aeronaves;
GO
SELECT * FROM Puertas;
GO
SELECT * FROM Vuelos ORDER BY FechaSalida;
GO
SELECT * FROM Pasajeros;
GO
SELECT * FROM Reservas;
GO
-- Resumen de datos insertados
SELECT 'Aerolíneas' AS Tabla, COUNT(*) AS Total FROM Aerolineas
UNION ALL
SELECT 'Aeronaves', COUNT(*) FROM Aeronaves
UNION ALL
SELECT 'Puertas', COUNT(*) FROM Puertas
UNION ALL
SELECT 'Vuelos', COUNT(*) FROM Vuelos
UNION ALL
SELECT 'Pasajeros', COUNT(*) FROM Pasajeros
UNION ALL
SELECT 'Reservas', COUNT(*) FROM Reservas;
GO
-- Ver vuelos con toda la información relacionada
SELECT 
    V.NumeroVuelo,
    A.Nombre AS Aerolinea,
    AE.Modelo AS Aeronave,
    V.Origen,
    V.Destino,
    V.FechaSalida,
    V.Estado,
    P.Numero AS Puerta,
    V.AsientosDisponibles,
    V.PrecioBase
FROM Vuelos V
INNER JOIN Aerolineas A ON V.AerolineaID = A.AerolineaID
INNER JOIN Aeronaves AE ON V.AeronaveID = AE.AeronaveID
LEFT JOIN Puertas P ON V.PuertaID = P.PuertaID
ORDER BY V.FechaSalida;
GO
