-- Crear la base de datos
CREATE DATABASE GestionAeropuerto;
GO

-- Usar la base de datos
USE GestionAeropuerto;
GO

-- Tabla de Aerolíneas
CREATE TABLE Aerolineas (
    AerolineaID INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL,
    CodigoIATA CHAR(2) NOT NULL UNIQUE,
    Pais NVARCHAR(50) NOT NULL,
    Telefono NVARCHAR(20),
    Email NVARCHAR(100),
    Activo BIT NOT NULL DEFAULT 1,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE()
);
GO

-- Tabla de Aeronaves (Aviones)
CREATE TABLE Aeronaves (
    AeronaveID INT PRIMARY KEY IDENTITY(1,1),
    Matricula NVARCHAR(20) NOT NULL UNIQUE,
    Modelo NVARCHAR(50) NOT NULL, 
    Fabricante NVARCHAR(50) NOT NULL,
    CapacidadPasajeros INT NOT NULL,
    AerolineaID INT NOT NULL,
    AnioFabricacion INT,
    Estado NVARCHAR(20) NOT NULL DEFAULT 'Operativo',
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT FK_Aeronaves_Aerolineas FOREIGN KEY (AerolineaID) 
        REFERENCES Aerolineas(AerolineaID),
    CONSTRAINT CHK_Capacidad CHECK (CapacidadPasajeros > 0),
    CONSTRAINT CHK_Estado_Aeronave CHECK (Estado IN ('Operativo', 'Mantenimiento', 'Fuera de Servicio'))
);
GO

-- Tabla de Puertas de Embarque
CREATE TABLE Puertas (
    PuertaID INT PRIMARY KEY IDENTITY(1,1),
    Numero NVARCHAR(10) NOT NULL UNIQUE,
    Terminal NVARCHAR(10) NOT NULL,
    Estado NVARCHAR(20) NOT NULL DEFAULT 'Disponible',
    
    CONSTRAINT CHK_Estado_Puerta CHECK (Estado IN ('Disponible', 'Ocupada', 'Mantenimiento'))
);
GO

-- Tabla de Vuelos
CREATE TABLE Vuelos (
    VueloID INT PRIMARY KEY IDENTITY(1,1),
    NumeroVuelo NVARCHAR(10) NOT NULL,
    AerolineaID INT NOT NULL,
    AeronaveID INT NOT NULL,
    Origen NVARCHAR(100) NOT NULL,
    Destino NVARCHAR(100) NOT NULL, 
    FechaSalida DATETIME NOT NULL,
    FechaLlegada DATETIME NOT NULL,
    Estado NVARCHAR(20) NOT NULL DEFAULT 'Programado', 
    PuertaID INT NULL,
    AsientosDisponibles INT NOT NULL,
    PrecioBase DECIMAL(10,2) NOT NULL,
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT FK_Vuelos_Aerolineas FOREIGN KEY (AerolineaID) 
        REFERENCES Aerolineas(AerolineaID),
    CONSTRAINT FK_Vuelos_Aeronaves FOREIGN KEY (AeronaveID) 
        REFERENCES Aeronaves(AeronaveID),
    CONSTRAINT FK_Vuelos_Puertas FOREIGN KEY (PuertaID) 
        REFERENCES Puertas(PuertaID),
    CONSTRAINT CHK_Fechas CHECK (FechaLlegada > FechaSalida),
    CONSTRAINT CHK_Estado_Vuelo CHECK (Estado IN ('Programado', 'Abordando', 'En Vuelo', 'Aterrizado', 'Cancelado', 'Retrasado'))
);
GO

-- Tabla de Pasajeros
CREATE TABLE Pasajeros (
    PasajeroID INT PRIMARY KEY IDENTITY(1,1),
    TipoDocumento NVARCHAR(20) NOT NULL, 
    NumeroDocumento NVARCHAR(20) NOT NULL,
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    FechaNacimiento DATE NOT NULL,
    Nacionalidad NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100),
    Telefono NVARCHAR(20),
    FechaRegistro DATETIME NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT UQ_Documento UNIQUE (TipoDocumento, NumeroDocumento),
    CONSTRAINT CHK_Tipo_Documento CHECK (TipoDocumento IN ('CC', 'Pasaporte', 'TI', 'CE'))
);
GO

-- Tabla de Reservas
CREATE TABLE Reservas (
    ReservaID INT PRIMARY KEY IDENTITY(1,1),
    CodigoReserva NVARCHAR(10) NOT NULL UNIQUE,
    VueloID INT NOT NULL,
    PasajeroID INT NOT NULL,
    NumeroAsiento NVARCHAR(5) NOT NULL,
    Clase NVARCHAR(20) NOT NULL, 
    Estado NVARCHAR(20) NOT NULL DEFAULT 'Confirmada',
    Precio DECIMAL(10,2) NOT NULL,
    Equipaje INT NOT NULL DEFAULT 1, 
    FechaReserva DATETIME NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT FK_Reservas_Vuelos FOREIGN KEY (VueloID) 
        REFERENCES Vuelos(VueloID),
    CONSTRAINT FK_Reservas_Pasajeros FOREIGN KEY (PasajeroID) 
        REFERENCES Pasajeros(PasajeroID),
    CONSTRAINT CHK_Clase CHECK (Clase IN ('Economica', 'Ejecutiva', 'Primera')),
    CONSTRAINT CHK_Estado_Reserva CHECK (Estado IN ('Confirmada', 'CheckIn', 'Abordado', 'Cancelada')),
    CONSTRAINT CHK_Precio CHECK (Precio > 0)
);
GO

-- Tabla de Auditoría para cambios de estado de vuelos
CREATE TABLE HistorialEstadosVuelo (
    HistorialID INT PRIMARY KEY IDENTITY(1,1),
    VueloID INT NOT NULL,
    EstadoAnterior NVARCHAR(20),
    EstadoNuevo NVARCHAR(20) NOT NULL,
    FechaCambio DATETIME NOT NULL DEFAULT GETDATE(),
    Observaciones NVARCHAR(500),
    
    CONSTRAINT FK_Historial_Vuelos FOREIGN KEY (VueloID) 
        REFERENCES Vuelos(VueloID)
);
GO
