# Sistema de Gestión: Centro Deportivo (WPF - MVVM)
## Autores
- Mauro (Desarrollo y documentación)
---

Aplicación de escritorio desarrollada en **C# y WPF** para la administración integral de un centro deportivo. Este proyecto destaca por la implementación estricta del patrón de arquitectura **MVVM (Model-View-ViewModel)**, logrando una separación limpia entre la interfaz de usuario y la lógica de negocio.

---

## Descripción del Proyecto

El sistema proporciona una interfaz amigable para que los administradores gestionen el flujo diario del gimnasio. Permite controlar el registro de clientes, configurar las actividades ofertadas y gestionar las reservas de clases, asegurando el cumplimiento de las normativas de aforo.

### Funcionalidades Principales

* ** Gestión de Socios:**
    * CRUD completo (Crear, Leer, Actualizar, Borrar).
    * **Validaciones:** Verificación de campos obligatorios y formato de correo electrónico mediante expresiones regulares (`Regex`).
* ** Catálogo de Actividades:**
    * Administración de servicios (Yoga, Pilates, CrossFit, etc.).
    * Definición de **Aforo Máximo** por actividad.
* ** Sistema de Reservas Inteligente:**
    * **Control de Aforo en Tiempo Real:** El sistema calcula las plazas ocupadas antes de confirmar una reserva. Si la clase está llena, impide la operación.
    * **Validación Temporal:** Bloqueo de reservas en fechas pasadas.
    * Vinculación relacional entre Socios, Actividades y Fechas.

---

##  Tecnologías Usadas

* **Lenguaje:** C# (.NET Framework)
* **Interfaz (UI):** Windows Presentation Foundation (WPF) / XAML
* **Arquitectura:** MVVM (Model - View - ViewModel)
* **Acceso a Datos:**
    * **Entity Framework** (Database First / EDMX).
    * **Patrón Repositorio:** Implementado en la carpeta `Model/Repo` para abstraer las consultas a la base de datos.
* **Base de Datos:** SQL Server.
* **IDE:** Visual Studio.

---

## Instalación

1. Clonar el repositorio:
   ```bash
   git clone https://github.com/Mauro-vs/Actividad_2_MVVM_Documentacion_mauro.git
   ```
2. Abrir la solución en **Visual Studio** (`Actividad_2_MVVM_Documentacion_mauro.sln`).
3. Restaurar paquetes NuGet si es necesario.
4. Comprobar la cadena de conexión de la base de datos en el `App.config` del proyecto WPF.

## Cómo ejecutar

1. Establecer el proyecto `Actividad_2_MVVM_mauro` como **proyecto de inicio**.
2. Compilar la solución en modo `Debug` o `Release`.
3. Ejecutar con `F5` o `Ctrl+F5`.
4. Desde la ventana principal se pueden abrir:
   - Ventana de **Socios**
   - Ventana de **Actividades**
   - Ventana de **Reservas**

---

## Estructura del Proyecto

La solución se organiza en capas para facilitar el mantenimiento:

```bash
CentroDeportivo/
├── Model/                  # Lógica de Datos
│   ├── Repo/               # Repositorios (Abstracción de consultas a BD)
│   └── Model1.edmx         # Mapeo ORM de Entity Framework
├── View/                   # Interfaz de Usuario (Archivos .xaml)
│   ├── MainWindow.xaml     # Menú Principal
│   └── ...                 # Vistas de gestión
└── ViewModel/              # Lógica de Negocio y Presentación
    ├── Commands/           # Implementación de ICommand (RelayCommand)
    └── ...ViewModel.cs     # Lógica específica de cada vista
