# GCatcode Backend API

Backend desarrollado en .NET 8 con arquitectura en capas, **sistema de permisos híbrido RBAC + ABAC**, autenticación JWT y gestión de usuarios y roles.

## 📋 Tabla de Contenidos

- [Características](#características)
- [Tecnologías](#tecnologías)
- [Estructura del Proyecto](#estructura-del-proyecto)
- [Requisitos Previos](#requisitos-previos)
- [Instalación](#instalación)
- [Configuración](#configuración)
- [Ejecución](#ejecución)
- [Migraciones](#migraciones)
- [API Endpoints](#api-endpoints)
- [Sistema de Permisos](#sistema-de-permisos)
- [Testing](#testing)

## ✨ Características

### 🔐 **Autenticación y Seguridad**
- **Autenticación JWT**: Sistema completo con tokens JWT y Refresh Tokens
- **Registro de Usuarios**: Endpoint para registro de nuevos usuarios
- **Refresh Tokens**: Sistema de renovación automática con gestión de expiración
- **Hashing Seguro**: Contraseñas con Argon2

### 🛡️ **Sistema de Permisos Híbrido (RBAC + ABAC)** - **Nuevo**
- **RBAC**: Permisos heredados automáticamente de roles (Dev, Admin, User, Guest)
- **ABAC**: Permisos directos a usuarios con prioridad sobre roles
- **12 Permisos Base**: users, roles, permissions, views, system
- **Validación por Atributos**: `[RequirePermission("code")]` en endpoints
- **Validación Programática**: Métodos `HasPermission()`, `HasAllPermissions()`, `HasAnyPermission()`
- **Override de Permisos**: Capacidad de conceder o denegar permisos individuales

### 👥 **Gestión de Acceso**
- **Gestión de Usuarios**: CRUD completo con cambio de contraseña
- **Sistema de Roles**: Gestión de roles y asignaciones
- **Gestión de Vistas/Menús**: Control de acceso a UI por roles
- **Gestión de Permisos**: API completa para administrar permisos RBAC y ABAC

### 🏗️ **Arquitectura**
- **Arquitectura en Capas**: Api, Services, DataBase, Utils
- **Entity Framework Core**: Para migraciones y esquema
- **Dapper**: Para consultas SQL optimizadas
- **Repository Pattern**: Servicios de datos desacoplados
- **Base Controller**: Funcionalidad compartida y métodos de autorización

### 📊 **Base de Datos**
- **SQL Server 2022** (Docker)
- **Migraciones Automáticas**: Entity Framework Core
- **Seed de Datos**: Usuarios, roles y permisos predefinidos

### 📝 **Documentación**
- **Swagger/OpenAPI**: Documentación interactiva de la API
- **CORS**: Configurado para desarrollo frontend

## 🛠️ Tecnologías

- **.NET 8.0**
- **ASP.NET Core Web API**
- **Entity Framework Core 9.0**
- **SQL Server 2022** (Docker)
- **Dapper** (consultas SQL)
- **JWT Bearer Authentication**
- **Argon2** (Password Hashing)
- **Swagger/OpenAPI**
- **xUnit** (Testing)

## 📁 Estructura del Proyecto

```
BackendNet8/
├── GCatcode.Api/                    # Capa de presentación (API)
│   ├── Controllers/                 # Controladores de la API
│   │   ├── Permissions/            # Gestión de permisos (RBAC + ABAC) ⭐ Nuevo
│   │   ├── Views/                  # Gestión de vistas/menús ⭐ Nuevo
│   │   ├── Roles/                  # Gestión de roles
│   │   ├── Users/                  # Gestión de usuarios
│   │   └── TestController.cs       # Controlador de pruebas
│   ├── Core/                       # Funcionalidades core
│   │   ├── Authorization/          # Sistema de autorización ⭐ Nuevo
│   │   │   └── RequirePermissionAttribute.cs  # Atributo [RequirePermission]
│   │   ├── Auth/                   # Autenticación JWT
│   │   │   ├── Models/            # Modelos de autenticación
│   │   │   ├── AuthController.cs  # Controlador de autenticación
│   │   │   ├── AuthMethods.cs     # Lógica de autenticación
│   │   │   └── AuthResponse.cs    # Respuestas de autenticación
│   │   ├── BaseResponse.cs        # Respuesta base para la API
│   │   ├── BaseController.cs      # Controlador base con métodos de permisos ⭐
│   │   └── ApiResponse.cs         # Respuestas estandarizadas
│   └── Configuration/              # Configuración de la aplicación
│       └── AppSettings.cs          # Configuración global
│
├── GCatcode.DataBase/              # Capa de acceso a datos (EF Core)
│   ├── Models/                     # Entidades del dominio
│   │   ├── BaseModel.cs           # Modelo base con propiedades comunes
│   │   ├── TR_User.cs             # Entidad de usuario
│   │   ├── CL_Rol.cs              # Entidad de rol
│   │   ├── RL_UserRol.cs          # Relación usuarios-roles
│   │   ├── TR_RefreshToken.cs     # Entidad de refresh tokens
│   │   ├── CL_Permission.cs       # Catálogo de permisos ⭐ Nuevo
│   │   ├── RL_RolePermission.cs   # Permisos por rol (RBAC) ⭐ Nuevo
│   │   ├── RL_UserPermission.cs   # Permisos por usuario (ABAC) ⭐ Nuevo
│   │   ├── CL_View.cs             # Vistas/menús del sistema
│   │   └── RL_ViewRol.cs          # Relación vistas-roles
│   ├── Configurations/             # Configuraciones de EF Core
│   │   ├── BuilderPermissions.cs  # Config de permisos ⭐ Nuevo
│   │   ├── BuilderRolePermissions.cs  # Config RBAC ⭐ Nuevo
│   │   └── BuilderUserPermissions.cs  # Config ABAC ⭐ Nuevo
│   ├── Migrations/                 # Migraciones de BD
│   └── AppDBContext.cs            # Contexto de base de datos
│
├── GCatcode.Services/              # Capa de servicios (Repositorios)
│   └── DB/                        # Servicios de base de datos
│       ├── PermissionServices/    # Servicios de permisos ⭐ Nuevo
│       ├── ViewServices/          # Servicios de vistas ⭐ Nuevo
│       ├── UserServices/          # Servicios de usuarios
│       ├── RolServices/           # Servicios de roles
│       ├── UserRolServices/       # Relación usuarios-roles
│       ├── RefreshTokenServices/  # Servicios de refresh tokens
│       └── DBService.cs           # Servicio base de BD
│
├── Utils/                          # Utilidades compartidas
│   └── GCatcode.Utils.csproj
│
└── TestUnit/                       # Pruebas unitarias
    └── TestUnit.csproj
```

## 📋 Requisitos Previos

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [SQL Server Management Studio](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms) (Opcional)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) o [Visual Studio Code](https://code.visualstudio.com/)

## 🚀 Instalación

1. **Clonar el repositorio**
```bash
git clone https://github.com/Furiduri/BackendNet8.git
cd BackendNet8
```

2. **Restaurar paquetes NuGet**
```bash
dotnet restore
```

## ⚙️ Configuración

### 1. Base de Datos

El proyecto utiliza SQL Server en Docker. La cadena de conexión está configurada en:

**appsettings.Development.json:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Initial Catalog=BaseLine;User ID=sa;Password=GcatcodeDB2521.;TrustServerCertificate=True;"
  }
}
```

### 2. JWT Configuration

La configuración de JWT se encuentra en `appsettings.json`:
```json
{
  "Jwt": {
    "Key": "9UVsQ6Lm0fpi/Ty7dHFpWl9r3rIohurX3/qNm/HDQNE=",
    "Issuer": "Gcatcode.com",
    "Audience": "Gcatcode.com",
    "AccessTokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  }
}
```

**Configuración por Entorno:**
- **Producción**: Access Token expira en 60 minutos
- **Desarrollo**: Access Token expira en 15 minutos
- **Refresh Token**: 7 días en ambos entornos

### 3. CORS

El frontend está configurado para ejecutarse en:
- `http://localhost:5173` (Vite/React)
- `http://localhost:7185`
- `https://localhost:5173`

La configuración de CORS permite credenciales (`AllowCredentials`) para el manejo de cookies y refresh tokens.

Puedes modificar los orígenes permitidos en `Program.cs` en la política `CorsPolicy`.

## 🎮 Ejecución

### Opción 1: Script Automatizado (Recomendado)

Ejecuta el script PowerShell que inicia el contenedor Docker y aplica las migraciones:

```powershell
.\Start-BaseLineContainer.ps1
```

Este script:
- ✅ Verifica si el contenedor Docker existe
- ✅ Inicia o crea el contenedor SQL Server
- ✅ Espera a que SQL Server esté listo
- ✅ Aplica las migraciones de Entity Framework Core automáticamente

### Opción 2: Manual

1. **Iniciar contenedor Docker**
```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=GcatcodeDB2521." -p 1433:1433 --name BaseLine -d mcr.microsoft.com/mssql/server:2022-latest
```

2. **Aplicar migraciones**
```bash
dotnet ef database update --project GCatcode.DataBase/GCatcode.DataBase.csproj --startup-project GCatcode.Api/GCatcode.Api.csproj
```

3. **Ejecutar la API**
```bash
cd GCatcode.Api
dotnet run
```

La API estará disponible en:
- **HTTPS**: https://localhost:7000
- **HTTP**: http://localhost:5000
- **Swagger**: https://localhost:7000/swagger

## 🔄 Migraciones

### Crear una nueva migración
```bash
dotnet ef migrations add NombreMigracion --project GCatcode.DataBase/GCatcode.DataBase.csproj --startup-project GCatcode.Api/GCatcode.Api.csproj
```

### Aplicar migraciones
```bash
dotnet ef database update --project GCatcode.DataBase/GCatcode.DataBase.csproj --startup-project GCatcode.Api/GCatcode.Api.csproj
```

### Revertir migración
```bash
dotnet ef database update NombreMigracionAnterior --project GCatcode.DataBase/GCatcode.DataBase.csproj --startup-project GCatcode.Api/GCatcode.Api.csproj
```

## 📡 API Endpoints

### Autenticación

| Método | Endpoint | Descripción | Auth |
|--------|----------|-------------|------|
| POST | `/api/Auth/login` | Iniciar sesión y obtener JWT + Refresh Token | No |
| POST | `/api/Auth/register` | Registrar nuevo usuario y obtener JWT | No |
| POST | `/api/Auth/refresh` | Renovar access token usando refresh token | No |
| POST | `/api/Auth/revoke` | Revocar refresh token | Sí |
| GET | `/api/Auth/user_info` | Obtener información del usuario actual | Sí |

### Permisos ⭐ **Nuevo**

| Método | Endpoint | Descripción | Auth | Permiso |
|--------|----------|-------------|------|---------|
| GET | `/api/Permissions` | Listar todos los permisos | Sí | permissions.read |
| GET | `/api/Permissions/{id}` | Obtener permiso por ID | Sí | permissions.read |
| GET | `/api/Permissions/my-permissions` | Mis permisos (RBAC + ABAC) | Sí | - |
| GET | `/api/Permissions/user/{userId}` | Permisos de usuario | Sí | permissions.read |
| GET | `/api/Permissions/role/{roleId}` | Permisos de rol | Sí | permissions.read |
| POST | `/api/Permissions/user` | Asignar permiso a usuario (ABAC) | Sí | permissions.write |
| DELETE | `/api/Permissions/user/{userId}/{permissionId}` | Revocar permiso de usuario | Sí | permissions.write |
| POST | `/api/Permissions/role` | Asignar permiso a rol (RBAC) | Sí | permissions.write |
| DELETE | `/api/Permissions/role/{roleId}/{permissionId}` | Revocar permiso de rol | Sí | permissions.write |

### Vistas/Menús ⭐ **Nuevo**

| Método | Endpoint | Descripción | Auth | Permiso |
|--------|----------|-------------|------|---------|
| GET | `/api/Views` | Listar todas las vistas | Sí | views.read |
| GET | `/api/Views/with-roles` | Vistas con roles asignados | Sí | views.read |
| GET | `/api/Views/{id}` | Obtener vista por ID | Sí | views.read |
| GET | `/api/Views/my-views` | Mis vistas asignadas | Sí | - |
| GET | `/api/Views/user/{userId}` | Vistas de usuario | Sí | views.read |
| GET | `/api/Views/role/{roleId}` | Vistas de rol | Sí | views.read |
| POST | `/api/Views` | Crear nueva vista | Sí | views.write |
| PUT | `/api/Views` | Actualizar vista | Sí | views.write |
| DELETE | `/api/Views/{id}` | Eliminar vista | Sí | views.write |
| POST | `/api/Views/{viewId}/roles/{roleId}` | Asignar rol a vista | Sí | views.write |
| DELETE | `/api/Views/{viewId}/roles/{roleId}` | Remover rol de vista | Sí | views.write |
| PUT | `/api/Views/{viewId}/roles` | Asignar múltiples roles | Sí | views.write |

### Usuarios

| Método | Endpoint | Descripción | Auth | Permiso |
|--------|----------|-------------|------|---------|
| GET | `/api/Users` | Listar usuarios | Sí | users.read |
| GET | `/api/Users/{id}` | Obtener usuario por ID | Sí | users.read |
| POST | `/api/Users` | Crear nuevo usuario | Sí | users.write |
| PUT | `/api/Users` | Actualizar usuario | Sí | users.write |
| DELETE | `/api/Users/{id}` | Eliminar usuario | Sí | users.delete |

### Roles

| Método | Endpoint | Descripción | Auth | Permiso |
|--------|----------|-------------|------|---------|
| GET | `/api/Roles` | Listar roles | Sí | roles.read |
| GET | `/api/Roles/{id}` | Obtener rol por ID | Sí | roles.read |
| POST | `/api/Roles` | Crear nuevo rol | Sí | roles.write |
| PUT | `/api/Roles` | Actualizar rol | Sí | roles.write |
| DELETE | `/api/Roles/{id}` | Eliminar rol | Sí | roles.delete |

### Ejemplo de Login

**Solicitud:**
```bash
curl -X POST https://localhost:7000/api/Auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "email": "dev@local.com",
    "password": "RGV2MTIzNDU="
  }'
```

**Respuesta:**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "a1b2c3d4e5f6...",
  "tokenExpiryDate": "2025-02-08T18:30:00Z",
  "refreshExpiryDate": "2025-02-15T17:30:00Z",
  "user": {
    "userId": 1,
    "userName": "Dev",
    "email": "dev@local.com"
  }
}
```

## 🛡️ Sistema de Permisos

### Permisos Predefinidos (12 Base)

#### **Usuarios**
- `users.read` - Ver usuarios
- `users.write` - Crear/Editar usuarios
- `users.delete` - Eliminar usuarios

#### **Roles**
- `roles.read` - Ver roles
- `roles.write` - Crear/Editar roles
- `roles.delete` - Eliminar roles

#### **Permisos**
- `permissions.read` - Ver permisos
- `permissions.write` - Asignar permisos

#### **Vistas**
- `views.read` - Ver vistas/menús
- `views.write` - Gestionar vistas/menús

#### **Sistema**
- `system.admin` - Administración total
- `system.config` - Configurar sistema

### Asignaciones por Rol (Default)

| Rol | Permisos |
|-----|----------|
| **Dev (RolId = 1)** | Todos los 12 permisos del sistema |
| **Admin (RolId = 4)** | users.*, roles.*, permissions.*, views.* |
| **User (RolId = 3)** | users.read, roles.read, views.read |
| **Guest (RolId = 2)** | views.read |

### Proteger Endpoints con Permisos

```csharp
using GCatcode.Api.Core.Authorization;

[HttpGet, Authorize]
[RequirePermission("users.read")]
public IActionResult GetAll()
{
    // Solo usuarios con permiso "users.read" pueden acceder
}

[HttpPost, Authorize]
[RequirePermission("users.write")]
public IActionResult Create([FromBody] UserInsert data)
{
    // Solo usuarios con permiso "users.write" pueden crear
}
```

### Validación Programática

```csharp
// Verificar un permiso específico
if (!HasPermission("users.delete"))
{
    return StatusCode(403, new { Message = "Acceso denegado" });
}

// Verificar múltiples permisos (todos requeridos)
if (!HasAllPermissions("users.write", "roles.write"))
{
    return Forbid();
}

// Verificar al menos uno de varios permisos
if (!HasAnyPermission("users.write", "system.admin"))
{
    return Forbid();
}
```

### Asignar Permiso Directo a Usuario (ABAC)

```bash
curl -X POST https://localhost:7000/api/Permissions/user \
  -H "Authorization: Bearer {token}" \
  -H "Content-Type: application/json" \
  -d '{
    "userId": 5,
    "permissionId": 2,
    "isGranted": true
  }'
```

### Flujo de Autorización

```
1. Usuario autenticado → JWT con UserId
   ↓
2. [RequirePermission("code")] intercepta request
   ↓
3. Verificar permiso directo del usuario (ABAC)
   ├─ Si existe → Usar IsGranted (prioridad)
   └─ Si no existe → Verificar permisos de roles (RBAC)
   ↓
4. Autorizar (200) o Denegar (403)
```

**Documentación completa**: Ver `RBAC_ABAC_README.md`

## 🧪 Testing

Ejecutar las pruebas unitarias:

```bash
dotnet test TestUnit/TestUnit.csproj
```

## 🐳 Docker

### Gestión del Contenedor

**Ver contenedores en ejecución:**
```bash
docker ps
```

**Detener el contenedor:**
```bash
docker stop BaseLine
```

**Iniciar el contenedor:**
```bash
docker start BaseLine
```

**Eliminar el contenedor:**
```bash
docker rm -f BaseLine
```

## 📝 Notas de Desarrollo

- La API usa **autenticación JWT Bearer** con refresh tokens
- Las contraseñas deben enviarse en formato **Base64**
- Los endpoints protegidos requieren el header: `Authorization: Bearer {token}`
- El sistema de **permisos híbrido RBAC + ABAC** permite control granular de acceso
- Los **permisos directos de usuario (ABAC) tienen prioridad** sobre permisos de rol (RBAC)
- Usar el atributo **`[RequirePermission("code")]`** para proteger endpoints
- Los refresh tokens expiran en 7 días y se almacenan en la base de datos
- El entorno de desarrollo tiene validaciones JWT relajadas para facilitar el desarrollo
- CORS está habilitado para múltiples orígenes de desarrollo con soporte para credenciales

## 🔒 Seguridad

⚠️ **Importante para Producción:**
- Cambiar la clave JWT en `appsettings.json` (nunca usar la clave por defecto)
- Usar secretos de Azure Key Vault o variables de entorno para datos sensibles
- Modificar la contraseña de SQL Server
- Habilitar todas las validaciones JWT (`ValidateIssuer`, `ValidateAudience`, etc.)
- Configurar HTTPS correctamente con certificados válidos
- Revisar y restringir políticas de CORS según los dominios permitidos
- Implementar rate limiting para prevenir ataques de fuerza bruta
- Los refresh tokens deben limpiarse periódicamente de la base de datos (tokens expirados)
- **Auditar cambios de permisos** en el sistema RBAC + ABAC
- **Revisar periódicamente** permisos directos asignados a usuarios

## 📚 Documentación Adicional

- **`AGENTS.md`**: Configuración técnica para asistentes de IA
- **`RBAC_ABAC_README.md`**: Guía completa del sistema de permisos
- **`IMPLEMENTACION_COMPLETADA.md`**: Detalles de la implementación RBAC + ABAC

## 📄 Licencia

Este proyecto es privado. Todos los derechos reservados.

## 👥 Autor

**Jorge Perez - Gcatcode**
- GitHub: [@Furiduri](https://github.com/Furiduri)

## 🤝 Contribuciones

Este es un proyecto privado. Las contribuciones están restringidas a colaboradores autorizados.

---

**Versión:** 1.0.0  
**Última actualización:** Febrero 2025
