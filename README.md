# ✈️ Sistema Integral de Gestión Aeroportuaria

Sistema diseñado para administrar y controlar todas las operaciones clave de un aeropuerto: vuelos, reservas, pasajeros, aeronaves, puertas y estadísticas operativas.  
Este proyecto demuestra conocimientos en **C#, VB.NET, ASP.NET MVC 5, ASP.NET Web API, .NET 6, ADO.NET, SQL Server, Bootstrap, jQuery, DataTables, y Chart.js**, implementado bajo una arquitectura multinivel completa.

---

## 🏗️ Arquitectura General del Sistema

El sistema está compuesto por cuatro proyectos principales:

```
+---------------------------+
| GestionAeropuerto.Database|
| SQL Server                |
+---------------------------+

+---------------------------+        +---------------------------+
| GestionAeropuerto.API     | <----> | GestionAeropuerto.MVC     |
| ASP.NET Core Web API      |        | ASP.NET MVC 5 (.NET 4.8)  |
+---------------------------+        +---------------------------+

+---------------------------+
| GestionAeropuerto.VB      |
| Windows Forms VB.NET      |
+---------------------------+
```

Cada módulo cumple una función independiente, pero todos operan integrados para dar soporte al sistema aeroportuario.

---

# 📁 1. GestionAeropuerto.Database (SQL Server)

Contiene toda la estructura y lógica de datos:

### ✔ Incluye:
- Tablas normalizadas  
- Stored Procedures (CRUD + lógica avanzada)  
- Funciones escalares y de tabla  
- Disparadores (Triggers)  
- Vistas optimizadas  
- Scripts de creación, inserción y mantenimiento  

### ✔ Procesos administrados:
- Registro y actualización de vuelos  
- Asignación de puertas  
- Gestión de reservas y pasajeros  
- Cálculo de ocupación y KPIs  

---

# 🌐 2. GestionAeropuerto.API (ASP.NET Core Web API – .NET 8)

API REST moderna construida con .NET 8 para gestionar todas las operaciones del aeropuerto.

### ✔ Tecnologías:
- ASP.NET Core Web API  
- Entity Framework / ADO.NET  
- DTOs, servicios y controladores REST  
- Estructura por capas  

### ✔ Endpoints principales:

#### ✈️ Vuelos
- GET/POST/PUT/DELETE  
- Filtrar por fecha o estado  
- Cambiar estado (PATCH)  

#### 🎟️ Reservas
- Listado, creación y cancelación  
- Buscar por vuelo  
- Reservas de pasajero  

#### 👤 Pasajeros
- CRUD completo  
- Historial de reservas  

#### 🛩️ Aeronaves
- Listado y búsqueda por aerolínea  

#### 🛫 Puertas
- Puertas disponibles  
- Asignación de puerta a vuelo  

#### 📊 Dashboard
- KPIs operativos  
- Ocupación del aeropuerto  
- Estadísticas y métricas  

---

# 💻 3. GestionAeropuerto.MVC (ASP.NET MVC 5 – .NET Framework 4.8)

Aplicación web administrativa que consume la API REST y permite gestionar las operaciones diarias del aeropuerto.

### ✔ Tecnologías:
- ASP.NET MVC 5  
- Bootstrap 3  
- jQuery  
- DataTables  
- Chart.js  
- AJAX + consumo de API REST  

### ✔ Módulos principales:

#### ✈️ Gestión de Vuelos
- CRUD completo  
- Estado y filtros  
- Asignación de puertas  
- Tablero de salidas (live board)  

#### 🎟️ Gestión de Reservas
- Listado dinámico  
- Crear nuevas reservas  
- Buscar por código  
- Cancelación  

#### 👤 Gestión de Pasajeros
- Registro y edición  
- Historial  
- Búsqueda avanzada  

#### 📊 Dashboard
- KPIs del aeropuerto  
- Gráficos interactivos  
- Próximos vuelos  
- Accesos rápidos  

---

# 🖥️ 4. GestionAeropuerto.VB (Windows Forms – VB.NET)

Aplicación de escritorio diseñada para procesos internos como check-in y gestión rápida.

### ✔ Tecnologías:
- VB.NET  
- ADO.NET + SQL Server  
- WinForms  
- Stored Procedures  
- Manejo de errores y validaciones  

### ✔ Formularios incluidos:
- **FrmPrincipal** – menú general  
- **FrmGestionVuelos** – CRUD de vuelos  
- **FrmCheckIn** – proceso de embarque  
- **FrmGestionReservas** – gestión de reservas  
- **FrmAsignacionPuertas** – asignación manual/automática  

---

# 🔧 Instalación y Configuración

1. Clonar el repositorio:  
   ```bash
   git clone https://github.com/LMDev024/sistemaGestionAeropuerto.git
   ```
2. Configurar la base de datos ejecutando los scripts del proyecto **Database**.  
3. Ajustar cadenas de conexión en:
   - `appsettings.json` (API)  
   - `Web.config` (MVC)  
   - Formularios VB.NET  

4. Ejecutar proyectos en este orden:
   - API  
   - MVC  
   - VB.NET (opcional)  

---

# 🚀 Ejecución

- **API:** disponible en `http://localhost:{puerto}/api/`  
- **MVC:** panel administrativo accesible desde navegador  
- **WinForms:** ejecutar desde Visual Studio  

---

# 🧪 Pruebas

Puedes probar la API con:
- Postman  
- Swagger (si está habilitado)  
- Thunder Client  

La aplicación MVC y el cliente VB.NET consumen directamente la API.

---

# 📌 Posibles Mejoras Futuras

- Autenticación basada en **JWT**, roles y permisos.  
- Migración del MVC a **ASP.NET Core MVC**.  
- Notificaciones en tiempo real con **SignalR**.  
- CI/CD con GitHub Actions y despliegue a Azure.  
- Crear módulo de reportes PDF/Excel.  
- Mejorar UI con Bootstrap 5 o Tailwind CSS.  
- Pruebas unitarias con xUnit o NUnit.  
- Dashboard con estadísticas en vivo.  
- Implementar logs avanzados con Serilog + Elastic.  
- Implementación de colas (RabbitMQ / Azure Service Bus).  

---

# 👨‍💻 Autor

**Luis Malagón (LMDev024)**  
Desarrollador Full Stack – C#, VB.NET, SQL Server, .NET  
GitHub: https://github.com/LMDev024
