# 🗄️ Base de Datos: Sistema de Gestión Aeroportuaria

## 📋 Descripción General

Este proyecto implementa una base de datos completa para la gestión operacional de un aeropuerto. El sistema permite administrar vuelos, aerolíneas, aeronaves, pasajeros, reservas y la asignación de puertas de embarque, proporcionando una solución integral para las operaciones diarias de un aeropuerto.

---

## 🎯 Objetivos del Sistema

El sistema fue diseñado para:

- **Gestionar operaciones de vuelo**: Desde la programación hasta el aterrizaje, con seguimiento en tiempo real del estado de cada vuelo.
- **Controlar reservas y pasajeros**: Registro completo de pasajeros y sus reservas, con validaciones para evitar sobrecupos y asientos duplicados.
- **Administrar recursos**: Asignación eficiente de aeronaves y puertas de embarque.
- **Generar reportes y estadísticas**: KPIs operacionales y financieros para la toma de decisiones.
- **Mantener trazabilidad**: Auditoría automática de todos los cambios críticos en el sistema.

---

## 🏗️ Arquitectura de la Base de Datos

### Tablas Principales (7)

#### 1. **Aerolineas**
Almacena información de las compañías aéreas que operan en el aeropuerto.

**¿Por qué existe?**
- Permite gestionar múltiples aerolíneas con sus propias flotas y operaciones.
- Facilita reportes por aerolínea (ingresos, ocupación, puntualidad).

**Datos clave:**
- Nombre comercial y código IATA (ej: "AV" para Avianca)
- País de origen
- Contacto
- Estado activo/inactivo

---

#### 2. **Aeronaves**
Registra todos los aviones disponibles para operar vuelos.

**¿Por qué existe?**
- Cada vuelo necesita una aeronave específica.
- La capacidad de pasajeros depende del modelo del avión.
- Permite control de mantenimiento y disponibilidad.

**Relación:**
- Pertenece a una **Aerolínea** (FK: AerolineaID)

**Datos clave:**
- Matrícula única (identificador del avión)
- Modelo y fabricante (Boeing 737, Airbus A320, etc.)
- Capacidad de pasajeros
- Estado operativo (Operativo, Mantenimiento, Fuera de Servicio)

---

#### 3. **Puertas**
Representa las puertas de embarque del aeropuerto.

**¿Por qué existe?**
- Los pasajeros necesitan saber desde dónde abordan.
- Optimiza el flujo de pasajeros por terminal.
- Previene conflictos de asignación (dos vuelos en la misma puerta).

**Datos clave:**
- Número de puerta (A1, B12, C3)
- Terminal asignado
- Estado (Disponible, Ocupada, Mantenimiento)

---

#### 4. **Vuelos** ⭐ (Tabla Central)
El corazón del sistema. Representa cada vuelo programado.

**¿Por qué es central?**
- Conecta aerolíneas, aeronaves, puertas, pasajeros y reservas.
- Toda la operación del aeropuerto gira en torno a los vuelos.

**Relaciones:**
- Pertenece a una **Aerolínea** (FK: AerolineaID)
- Usa una **Aeronave** específica (FK: AeronaveID)
- Puede tener una **Puerta** asignada (FK: PuertaID - nullable)

**Datos clave:**
- Número de vuelo (ej: AV101)
- Origen y destino
- Fechas y horas de salida/llegada
- Estado (Programado, Abordando, En Vuelo, Aterrizado, Cancelado, Retrasado)
- Asientos disponibles (se actualiza con cada reserva)
- Precio base

**¿Por qué AsientosDisponibles?**
- Permite consultas rápidas sin contar reservas.
- Se actualiza automáticamente con transacciones.

---

#### 5. **Pasajeros**
Información de las personas que viajan.

**¿Por qué existe?**
- Un pasajero puede tener múltiples reservas a lo largo del tiempo.
- Evita duplicar datos en cada reserva.
- Facilita búsquedas por documento.

**Datos clave:**
- Tipo y número de documento (único)
- Nombre completo
- Fecha de nacimiento
- Nacionalidad
- Contacto (email, teléfono)

---

#### 6. **Reservas** ⭐ (Tabla Transaccional)
Vincula pasajeros con vuelos específicos.

**¿Por qué existe?**
- Registra quién viaja en qué vuelo.
- Controla la ocupación de asientos.
- Base para facturación e ingresos.

**Relaciones:**
- Pertenece a un **Vuelo** (FK: VueloID)
- Pertenece a un **Pasajero** (FK: PasajeroID)

**Datos clave:**
- Código de reserva único (ej: ABC123XYZ)
- Número de asiento (12A, 5C)
- Clase (Económica, Ejecutiva, Primera)
- Estado (Confirmada, CheckIn, Abordado, Cancelada)
- Precio (puede variar por clase)
- Cantidad de equipaje

**¿Por qué guardar el precio en la reserva?**
- El precio base del vuelo puede cambiar.
- Cada reserva mantiene el precio al momento de la compra.
- Facilita reportes financieros exactos.

---

#### 7. **HistorialEstadosVuelo** (Auditoría)
Registra todos los cambios de estado de los vuelos.

**¿Por qué existe?**
- Trazabilidad: saber cuándo y cómo cambió cada vuelo.
- Análisis de puntualidad y eficiencia.
- Cumplimiento normativo (auditorías).

**Relación:**
- Referencia a **Vuelos** (FK: VueloID)

**Datos clave:**
- Estado anterior y nuevo
- Fecha y hora del cambio
- Observaciones

---

### Tabla Auxiliar

#### 8. **LogNotificaciones**
Registra eventos importantes que requieren notificación.

**¿Por qué existe?**
- Simula un sistema de notificaciones.
- Registra cambios de puerta, cancelaciones, etc.
- Base para futuras integraciones (SMS, email, app móvil).

---

## 🔗 Relaciones entre Tablas
```
Aerolineas (1) ----< Aeronaves (N)
                         |
                         |
                    Aeronaves (1) ----< Vuelos (N)
                                           |
                                           |
              +----------------------------+------------------------+
              |                            |                        |
          Puertas (1)              Reservas (N)         HistorialEstadosVuelo (N)
              |                            |
              |                            |
           (nullable)                  Pasajeros (1) ----< Reservas (N)
```

### Explicación de las Relaciones:

**Aerolínea → Aeronaves (1:N)**
- Una aerolínea puede tener muchos aviones.
- Un avión pertenece a una sola aerolínea.

**Aeronave → Vuelos (1:N)**
- Una aeronave puede realizar muchos vuelos.
- Un vuelo usa una sola aeronave.

**Vuelo → Puerta (N:1 opcional)**
- Muchos vuelos pueden usar la misma puerta (en diferentes momentos).
- Un vuelo puede no tener puerta asignada aún (NULL).

**Vuelo → Reservas (1:N)**
- Un vuelo puede tener muchas reservas.
- Una reserva pertenece a un solo vuelo.

**Pasajero → Reservas (1:N)**
- Un pasajero puede tener múltiples reservas.
- Una reserva es de un solo pasajero.

**Vuelo → Historial (1:N)**
- Un vuelo tiene múltiples registros de cambios de estado.
- Cada registro pertenece a un solo vuelo.

---

## 🔧 Stored Procedures

### ¿Por qué usar Stored Procedures?

Los SP encapsulan lógica de negocio compleja en el servidor de base de datos, ofreciendo:
- **Seguridad**: Control de acceso y validaciones centralizadas.
- **Performance**: Ejecución optimizada y planes de ejecución cacheados.
- **Mantenibilidad**: Cambios en un solo lugar.
- **Transaccionalidad**: Operaciones atómicas con ROLLBACK automático.

---

### Lista de Stored Procedures:

#### 1. **sp_CrearVuelo**
**Propósito**: Crear un nuevo vuelo con validaciones.

**¿Qué hace?**
- Valida que las fechas sean coherentes.
- Verifica que la aeronave esté operativa.
- Asigna automáticamente la capacidad de pasajeros.
- Registra el vuelo en el historial.

**¿Por qué?**
- Centraliza las validaciones de negocio.
- Previene vuelos con datos inconsistentes.

---

#### 2. **sp_ActualizarEstadoVuelo**
**Propósito**: Cambiar el estado de un vuelo (Programado → Abordando → En Vuelo → Aterrizado).

**¿Qué hace?**
- Valida transiciones de estado válidas.
- Previene cambios en vuelos cancelados.
- Registra automáticamente en el historial.
- Usa transacciones para garantizar consistencia.

**¿Por qué?**
- El estado del vuelo es crítico para operaciones.
- Debe ser rastreable para auditorías.

---

#### 3. **sp_RealizarReserva** ⭐ (Más complejo)
**Propósito**: Crear una reserva con todas las validaciones necesarias.

**¿Qué hace?**
- Verifica disponibilidad de asientos.
- Valida que el asiento no esté ocupado.
- Calcula precio según la clase (Económica, Ejecutiva, Primera).
- Genera código único de reserva.
- Actualiza asientos disponibles del vuelo.
- **TODO en una transacción** (si algo falla, nada se guarda).

**¿Por qué es crítico?**
- Previene sobrecupo (overbooking accidental).
- Garantiza integridad: o todo se guarda o nada.

---

#### 4. **sp_CancelarReserva**
**Propósito**: Cancelar una reserva y liberar el asiento.

**¿Qué hace?**
- Valida que la reserva exista y sea cancelable.
- Cambia estado de la reserva a "Cancelada".
- Devuelve el asiento al vuelo (incrementa AsientosDisponibles).
- Usa transacciones.

**¿Por qué?**
- Mantiene sincronización entre reservas y disponibilidad.
- Impide cancelar reservas de pasajeros que ya abordaron.

---

#### 5. **sp_AsignarPuerta**
**Propósito**: Asignar una puerta de embarque a un vuelo.

**¿Qué hace?**
- Valida que la puerta esté disponible.
- Libera puerta anterior si existe.
- Marca la nueva puerta como ocupada.
- Transaccional.

**¿Por qué?**
- Previene conflictos de asignación.
- Mantiene sincronizado el estado de las puertas.

---

#### 6. **sp_ObtenerVuelosPorFecha**
**Propósito**: Consulta optimizada de vuelos de una fecha.

**¿Qué hace?**
- JOIN de vuelos con aerolíneas, aeronaves y puertas.
- Retorna toda la información en una consulta.

**¿Por qué?**
- Evita múltiples queries desde la aplicación.
- Usado por el tablero de salidas/llegadas.

---

#### 7. **sp_ObtenerPasajerosPorVuelo**
**Propósito**: Lista de pasajeros de un vuelo específico.

**¿Qué hace?**
- JOIN de reservas con pasajeros.
- Excluye reservas canceladas.
- Ordena por asiento.

**¿Por qué?**
- Usado en check-in y abordaje.
- Genera manifiestos de pasajeros.

---

#### 8. **sp_ObtenerEstadisticasVuelos**
**Propósito**: KPIs y métricas operacionales.

**¿Qué hace?**
- Agrupa vuelos por estado.
- Calcula ocupación por aerolínea.
- Suma pasajeros transportados.

**¿Por qué?**
- Dashboard gerencial.
- Reportes de performance.

---

## 📊 Funciones

### ¿Por qué funciones?

Las funciones permiten **cálculos reutilizables** que pueden ser usados en queries, vistas y stored procedures.

---

### Funciones Escalares (retornan un valor):

#### 1. **fn_CalcularOcupacionVuelo**
**Propósito**: Calcular porcentaje de ocupación de un vuelo.

**Fórmula**: `(CapacidadTotal - AsientosDisponibles) / CapacidadTotal * 100`

**¿Por qué?**
- Métrica clave para rentabilidad.
- Usada en múltiples reportes y vistas.

---

#### 2. **fn_ValidarDisponibilidadAsiento**
**Propósito**: Verificar si un asiento específico está libre.

**Retorna**: 1 (disponible) o 0 (ocupado)

**¿Por qué?**
- Validación rápida antes de reservar.
- Usado en interfaces de selección de asiento.

---

#### 3. **fn_CalcularTiempoVuelo**
**Propósito**: Calcular duración del vuelo en minutos.

**¿Por qué?**
- Planificación de tripulaciones.
- Estimación de consumo de combustible.
- Información para pasajeros.

---

#### 4. **fn_CalcularIngresosPorVuelo**
**Propósito**: Sumar ingresos de todas las reservas confirmadas.

**¿Por qué?**
- Análisis de rentabilidad por vuelo.
- Excluye reservas canceladas automáticamente.

---

### Funciones de Tabla (retornan tablas):

#### 5. **fn_ObtenerVuelosDisponibles**
**Propósito**: Vuelos que aún tienen asientos libres en una fecha.

**¿Por qué?**
- Usado en motores de búsqueda.
- Filtra automáticamente vuelos llenos o cancelados.

---

#### 6. **fn_ObtenerReservasPorPasajero**
**Propósito**: Historial de reservas de un pasajero.

**¿Por qué?**
- Atención al cliente.
- Programas de viajero frecuente.

---

#### 7. **fn_BuscarVuelosPorDestino**
**Propósito**: Buscar vuelos que van a un destino específico.

**¿Por qué?**
- Motor de búsqueda en aplicaciones web.
- Permite búsquedas parciales (ej: "Miami" o "MIA").

---

## 🔔 Triggers (Automatización)

### ¿Por qué triggers?

Los triggers ejecutan acciones **automáticamente** cuando ocurren eventos en las tablas, garantizando integridad y trazabilidad sin intervención manual.

---

### Lista de Triggers:

#### 1. **trg_AuditarCambiosVuelo** (AFTER UPDATE en Vuelos)
**Propósito**: Registrar automáticamente cambios de estado.

**¿Qué hace?**
- Detecta cuando cambia el estado de un vuelo.
- Inserta registro en HistorialEstadosVuelo.

**¿Por qué?**
- Auditoría automática (sin código en la aplicación).
- Trazabilidad completa.

---

#### 2. **trg_ValidarCapacidadVuelo** (INSTEAD OF INSERT en Reservas)
**Propósito**: Prevenir sobrecupo y asientos duplicados.

**¿Qué hace?**
- Intercepta inserciones en Reservas.
- Valida disponibilidad antes de insertar.
- Si falla, hace ROLLBACK.

**¿Por qué?**
- Última línea de defensa contra errores.
- Funciona incluso si la aplicación omite validaciones.

---

#### 3. **trg_NotificarCambioPuerta** (AFTER UPDATE en Vuelos)
**Propósito**: Registrar cambios de puerta para notificaciones.

**¿Qué hace?**
- Detecta cuando cambia PuertaID.
- Inserta en LogNotificaciones.

**¿Por qué?**
- Pasajeros deben ser notificados de cambios de puerta.
- Base para sistema de alertas.

---

#### 4. **trg_CancelarReservasVueloCancelado** (AFTER UPDATE en Vuelos)
**Propósito**: Cancelar automáticamente reservas si se cancela el vuelo.

**¿Qué hace?**
- Detecta cuando un vuelo cambia a "Cancelado".
- Cambia todas sus reservas a "Cancelada".
- Registra en LogNotificaciones.

**¿Por qué?**
- Garantiza consistencia.
- Evita que haya reservas activas en vuelos cancelados.

---

## 👁️ Vistas (Consultas Predefinidas)

### ¿Por qué vistas?

Las vistas son "tablas virtuales" que simplifican consultas complejas y ocultan la complejidad de los JOINs.

---

### Lista de Vistas:

#### 1. **vw_VuelosDelDia**
**Propósito**: Información completa de los vuelos de hoy.

**Incluye**:
- Datos del vuelo, aerolínea, aeronave, puerta.
- Cálculos de ocupación, duración, ingresos.
- Estado descriptivo (ej: "Próximo a Abordar").

**Uso**: Tableros de salidas/llegadas, aplicaciones móviles.

---

#### 2. **vw_EstadisticasAerolineas**
**Propósito**: KPIs agregados por aerolínea.

**Incluye**:
- Total de vuelos, aeronaves, reservas.
- Ingresos totales.
- Ocupación promedio.

**Uso**: Reportes gerenciales, análisis de rentabilidad.

---

#### 3. **vw_EstadoPuertas**
**Propósito**: Estado actual de todas las puertas.

**Incluye**:
- Puerta, terminal, estado.
- Vuelo asignado (si aplica).
- Minutos hasta salida.

**Uso**: Control de operaciones, asignación de puertas.

---

#### 4. **vw_ResumenReservas**
**Propósito**: Información completa de reservas activas.

**Incluye**:
- Datos del pasajero, vuelo, asiento.
- Estado y acciones requeridas (ej: "Realizar Check-in").

**Uso**: Atención al cliente, check-in, abordaje.

---

#### 5. **vw_DashboardOperacional**
**Propósito**: KPIs del día en una sola consulta.

**Incluye**:
- Total de vuelos por estado.
- Puertas disponibles/ocupadas.
- Reservas, check-ins, abordajes.
- Ingresos estimados del día.
- Ocupación promedio.

**Uso**: Dashboard principal del sistema.

---

## 🔐 Validaciones e Integridad

### Constraints (Restricciones):

**Primary Keys**: Cada tabla tiene un ID único autoincremental.

**Foreign Keys**: Garantizan que las relaciones sean válidas (ej: no se puede crear un vuelo con una aeronave que no existe).

**CHECK Constraints**:
- Estados válidos (ej: Vuelo solo puede ser: Programado, Abordando, En Vuelo, etc.)
- Fechas coherentes (FechaLlegada > FechaSalida)
- Capacidad positiva
- Precio positivo

**UNIQUE Constraints**:
- Matrícula de aeronave
- Código IATA de aerolínea
- Número de puerta
- Código de reserva
- Documento de pasajero (TipoDocumento + NumeroDocumento)

---

## 💾 Transacciones

Todos los Stored Procedures críticos usan **transacciones**:
```sql
BEGIN TRANSACTION
    -- Operaciones
    IF (error)
        ROLLBACK TRANSACTION
    ELSE
        COMMIT TRANSACTION
```

**¿Por qué?**
- Garantiza atomicidad: o todo se ejecuta o nada.
- Ejemplo: Al hacer una reserva, si falla actualizar el vuelo, la reserva tampoco se crea.

---

## 📈 Datos de Prueba

La base de datos incluye datos realistas para pruebas:

- **6 Aerolíneas**: Avianca, LATAM, Copa Airlines, American Airlines, Iberia, Viva Air
- **10 Aeronaves**: Modelos variados (Boeing, Airbus)
- **12 Puertas**: Distribuidas en 2 terminales
- **12 Vuelos**: Con diferentes estados y fechas
- **10 Pasajeros**: Con documentos variados
- **15 Reservas**: En diferentes estados

**¿Por qué datos de prueba?**
- Permite probar la API sin crear datos manualmente.
- Demuestra casos de uso reales.
- Facilita desarrollo y debugging.

---

## 🎯 Casos de Uso Principales

### 1. **Reservar un Vuelo**
```
1. Cliente busca vuelos disponibles (fn_ObtenerVuelosDisponibles)
2. Selecciona vuelo y asiento
3. Sistema ejecuta sp_RealizarReserva
   - Valida disponibilidad (trg_ValidarCapacidadVuelo)
   - Crea reserva
   - Actualiza asientos disponibles
   - Genera código de reserva
```

### 2. **Cambiar Estado de Vuelo**
```
1. Operador cambia estado (sp_ActualizarEstadoVuelo)
2. Trigger registra cambio (trg_AuditarCambiosVuelo)
3. Si hay cambio de puerta → Trigger notifica (trg_NotificarCambioPuerta)
4. Si se cancela → Trigger cancela reservas (trg_CancelarReservasVueloCancelado)
```

### 3. **Consultar Dashboard**
```
1. Sistema consulta vw_DashboardOperacional
2. Obtiene todos los KPIs en una query
3. Presenta información en tiempo real
```

---

## 📊 Modelo Entidad-Relación (Conceptual)
```
┌─────────────┐       ┌─────────────┐       ┌─────────────┐
│ Aerolineas  │───┬───│  Aeronaves  │───┬───│   Vuelos    │
└─────────────┘   │   └─────────────┘   │   └─────────────┘
                  │                     │          │
                  │                     │          ├─────┐
                  │                     │          │     │
                  │                     │    ┌─────▼───┐ │
                  │                     │    │ Puertas │ │
                  │                     │    └─────────┘ │
                  │                     │                │
                  │                     │       ┌────────▼──────┐
                  │                     │       │   Reservas    │
                  │                     │       └────────┬──────┘
                  │                     │                │
                  │                     │       ┌────────▼──────┐
                  │                     │       │   Pasajeros   │
                  │                     │       └───────────────┘
                  │                     │
                  │                     │       ┌───────────────────────┐
                  │                     └───────│ HistorialEstadosVuelo │
                  │                             └───────────────────────┘
                  │
                  │                             ┌──────────────────┐
                  └─────────────────────────────│ LogNotificaciones│
                                                └──────────────────┘
```

---

## 🏆 Mejores Prácticas Implementadas

✅ **Normalización**: Base de datos en 3FN (Tercera Forma Normal)
✅ **Nomenclatura consistente**: Tablas en plural, campos descriptivos
✅ **Índices implícitos**: Primary Keys y Foreign Keys
✅ **Validaciones en múltiples capas**: Constraints, SPs, Triggers
✅ **Auditoría automática**: Historial de cambios
✅ **Transacciones**: Operaciones atómicas
✅ **Documentación**: Comentarios en código SQL
✅ **Datos de prueba**: Facilita testing
✅ **Vistas**: Simplifican consultas complejas
✅ **Funciones reutilizables**: DRY (Don't Repeat Yourself)

---

## 🚀 Próximos Pasos

Esta base de datos está lista para ser consumida por:

1. **Web API (ASP.NET Core 6)**: Endpoints RESTful que exponen la funcionalidad
2. **Aplicación MVC (ASP.NET Framework 4.8)**: Portal web para gestión
3. **Aplicación VB.NET (Windows Forms)**: Cliente desktop para operaciones

---

## 📝 Notas Técnicas

**Motor**: SQL Server 2019+
**Collation**: SQL_Latin1_General_CP1_CI_AS
**Compatibilidad**: SQL Server 2016+

**Tamaño estimado**: ~5 MB con datos de prueba
**Tiempo de ejecución del script completo**: ~30 segundos

---

## 👨‍💻 Autor

Desarrollado como proyecto de portafolio para demostrar conocimientos en:
- Diseño de bases de datos relacionales
- Stored Procedures y transacciones
- Funciones y triggers
- Vistas y consultas complejas
- SQL Server avanzado

---

## 📄 Licencia

Este proyecto es de código abierto y fue creado con fines educativos y de demostración de habilidades técnicas.

---

**Fecha de creación**: Noviembre 2024
**Versión**: 1.0
**Estado**: ✅ Completado y funcional