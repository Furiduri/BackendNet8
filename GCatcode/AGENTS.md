# AGENTS.md - Configuración para Asistentes de IA

## 📋 Información del Proyecto

**Nombre del Proyecto:** GCatcode Backend API
**Framework:** ASP.NET Core 8.0
**Tipo:** Web API RESTful
**Repositorio:** [Github - BackendNet8](https://github.com/Furiduri/BackendNet8)
**Rama Principal:** main
**Rama Develop**: develop
**Autor:** Jorge Perez - Gcatcode
**Modelo de Desarrollo:** Agile/Scrum

## 🎯 Propósito del Proyecto

API backend construida en .NET 8 que proporciona servicios de autenticación, gestión de usuarios y **control de acceso híbrido RBAC + ABAC** con JWT. El proyecto implementa una arquitectura en capas con Entity Framework Core y Dapper.

## 🏗️ Arquitectura del Proyecto

### Estructura de Capas

```
GCatcode.Api/              # Capa de presentación (API)
├── Controllers/           # Controladores de API
├─── {Ruta}/   # Controladores específicos por ruta
|   ├── Models/             # Modelos específicos del controlador
|   ├── {Ruta}Controller.cs  # Controlador principal
|   ├── {Ruta}Methods.cs     # Lógica específica del controlador
|   └── {Ruta}Response.cs    # Mensajes de respuesta estandarizados
├── Core/                  # Funcionalidad base y autenticación
|   ├── BaseController.cs     # Controlador base con funcionalidades comunes
|   ├── BaseResponse.cs       # Respuesta base para estandarizar mensajes
|   ├── Authorization/        # Sistema de autorización RBAC + ABAC
|   |   └── RequirePermissionAttribute.cs  # Atributo para validar permisos
|   └── Auth/                # Lógica de autenticación JWT
└── Program.cs            # Punto de entrada y configuración

GCatcode.Repository/       # Capa de lógica de negocio
└── DB/                   # Servicios de acceso a datos
    ├── DBService.cs          # Servicio base para acceso a datos
    └── {Entidad}Services/    # Servicios específicos por entidad
        ├── Models/                # Modelos específicos de la entidad
        |   ├── {Entidad}DTO.cs       # DTO para la entidad
        └── {Entidad}Service.cs  # Servicio de datos para la entidad

GCatcode.DataBase/         # Capa de acceso a datos
├── Models/                # Modelos de datos (entidades)
|   ├── {TypeTable}_{Entidad}.cs          # Modelo de cada entidad
|   └── BaseModel.cs           # Propiedades comunes (Available, CreateTime, LastUpdated)
├── Migrations/             # Migraciones de Entity Framework
├── Configuration/          # Configuración de la base de datos
|   ├── Builder{Entidad}s.cs  # Configuración Builder para cada entidad
|   └── BuilderBase.cs          # Configuración base común
└── AppDBContext.cs       # Contexto de Entity Framework

GCatcode.Utils/            # Utilidades compartidas
└── Extensions/           # Métodos de extensión

TestUnit/                  # Proyecto de pruebas unitarias
```

### Patrones Implementados

- **Repository Pattern**: Servicios de datos (`UserService`, `RolesService`, `UserRolService`, `PermissionService`, `ViewService`)
- **Dependency Injection**: Configurado en `Program.cs`
- **Base Controller Pattern**: `BaseController` para funcionalidad compartida
- **JWT Authentication**: Autenticación basada en tokens
- **Hybrid RBAC + ABAC**: Control de acceso basado en roles y atributos

## 🔧 Tecnologías y Dependencias Principales

- **ASP.NET Core 8.0**: Framework principal
- **Entity Framework Core**: ORM con SQL Server
- **Dapper**: Micro-ORM para consultas optimizadas
- **JWT Bearer**: Autenticación con tokens
- **Argon2**: Hashing de contraseñas
- **Swagger/OpenAPI**: Documentación de API

## 🔐 Configuración de Seguridad

### Sistema de Permisos Híbrido (RBAC + ABAC)

El sistema implementa un enfoque híbrido que combina:

#### **RBAC (Role-Based Access Control)**
- Permisos heredados automáticamente de roles
- Gestión centralizada por rol (Dev, Admin, User, Guest)
- Estructura: `CL_Roles` → `RL_RolePermissions` → `CL_Permissions`

#### **ABAC (Attribute-Based Access Control)**
- Permisos directos asignados a usuarios individuales
- **Prioridad sobre permisos de rol** (override)
- Capacidad de conceder (`IsGranted = true`) o denegar (`IsGranted = false`)
- Estructura: `TR_Users` → `RL_UserPermissions` → `CL_Permissions`

#### **Flujo de Autorización**
```
1. Usuario autenticado → JWT con UserId
2. Endpoint protegido con [RequirePermission("code")]
3. Verificar permiso directo del usuario (ABAC)
   ├─ Si existe → Usar IsGranted (prioridad)
   └─ Si no existe → Verificar permisos heredados de roles (RBAC)
4. Autorizar o denegar acceso (403)
```

#### **Permisos Predefinidos (12 base)**
- **users**: `read`, `write`, `delete`
- **roles**: `read`, `write`, `delete`
- **permissions**: `read`, `write`
- **views**: `read`, `write`
- **system**: `admin`, `config`

### Autenticación JWT

- **Issuer/Audience**: Configurar en `appsettings.json`
- **Validación**: Estricta en producción, relajada en desarrollo
- **Expiración**: Tokens válidos por 1 día
- **Algoritmo**: HS256

### Gestión de Contraseñas

- **Hashing**: Argon2 (algoritmo resistente a ataques)
- **Validación**: Mínimo 8 caracteres, mayúsculas, minúsculas, números y símbolos
- **Cambio de contraseña**: Requiere verificación de contraseña actual

### CORS

- **Orígenes permitidos**:
  - `http://localhost:5173` (desarrollo frontend)
  - `https://localhost`
  - `https://anotherdomain.com` (por definir)
- **Métodos**: Todos permitidos
- **Headers**: Todos permitidos

## 📊 Base de Datos

### Conexiones

- **Entity Framework Core**: No usar mas que para migraciones o actualización de esquema
- **Dapper**: Para consultas de alto rendimiento
- **Proveedor**: Microsoft SQL Server

### Tipo de tablas
Los tipos de tablas se clasifican en tres categorías según su uso y patrón de acceso:

- **CL**: Tablas de catálogo, mucha lectura, poca escritura (Roles, Permissions, Views, etc.)
- **RL**: Tablas de relación, lectura y escritura moderada (UserRoles, RolePermissions, UserPermissions, ViewRoles, etc.)
- **TR**: Tablas transaccionales, muchas escrituras (Users, RefreshTokens, etc.)

### Entidades Principales

#### **Sistema de Autenticación**
- **TR_Users**: Usuarios del sistema
- **CL_Roles**: Roles disponibles (Dev, Admin, User, Guest)
- **RL_UserRoles**: Relación muchos a muchos entre usuarios y roles
- **TR_RefreshTokens**: Tokens de actualización JWT

#### **Sistema de Permisos (RBAC + ABAC)**
- **CL_Permissions**: Catálogo de permisos con código único (ej: "users.read")
- **RL_RolePermissions**: Permisos asignados a roles (RBAC)
- **RL_UserPermissions**: Permisos directos de usuarios con IsGranted (ABAC)

#### **Sistema de Vistas/Menús**
- **CL_Views**: Vistas/menús del sistema con jerarquía
- **RL_ViewRoles**: Relación entre vistas y roles para control de acceso a UI

## 🚀 Endpoints Principales

### Autenticación (`/api/Auth`)

- `POST /login`: Autenticación de usuarios
- `POST /register`: Registro de nuevos usuarios
- `GET /user_info`: Información del usuario autenticado (requiere auth)
- `POST /refresh`: Renovar access token con refresh token
- `POST /revoke`: Revocar refresh token

### Permisos (`/api/Permissions`) - **Nuevo**

- `GET /`: Todos los permisos del sistema
- `GET /{id}`: Permiso por ID
- `GET /my-permissions`: Permisos del usuario actual (RBAC + ABAC)
- `GET /user/{userId}`: Permisos de usuario específico
- `GET /role/{roleId}`: Permisos de un rol
- `POST /user`: Asignar permiso directo a usuario (ABAC)
- `DELETE /user/{userId}/{permissionId}`: Revocar permiso de usuario
- `POST /role`: Asignar permiso a rol (RBAC)
- `DELETE /role/{roleId}/{permissionId}`: Revocar permiso de rol

### Vistas/Menús (`/api/Views`) - **Nuevo**

- `GET /`: Todas las vistas del sistema
- `GET /with-roles`: Vistas con roles asignados
- `GET /{id}`: Vista por ID
- `GET /my-views`: Vistas asignadas al usuario actual
- `GET /user/{userId}`: Vistas de usuario específico
- `GET /role/{roleId}`: Vistas de un rol
- `POST /`: Crear nueva vista
- `PUT /`: Actualizar vista
- `DELETE /{id}`: Eliminar vista
- `POST /{viewId}/roles/{roleId}`: Asignar rol a vista
- `DELETE /{viewId}/roles/{roleId}`: Remover rol de vista
- `PUT /{viewId}/roles`: Asignar múltiples roles

### Usuarios (`/api/Users`)

- `GET /`: Listar usuarios
- `GET /{id}`: Obtener usuario por ID
- `POST /`: Crear usuario
- `PUT /`: Actualizar usuario
- `DELETE /{id}`: Eliminar usuario

### Roles (`/api/Roles`)

- `GET /`: Listar roles
- `GET /{id}`: Obtener rol por ID
- `POST /`: Crear rol
- `PUT /`: Actualizar rol
- `DELETE /{id}`: Eliminar rol

## 💻 Guías para Agentes de IA

### Al Generar Controladores

1. **Crear Carpeta**: Cada controlador en su propia carpeta bajo `Controllers/{Entity}/`
2. **Crear Archivo de Métodos**: `{Entity}Methods.cs` para lógica específica
3. **Crear Archivo de Mensajes**: `{Entity}Response.cs` para mensajes estandarizados, heredado de `BaseResponse` en Core
4. **Heredar de `BaseController`**: Todos los controladores deben heredar de esta clase
5. **Usar atributos de ruta**: `[ApiController, Route("api/[controller]")]`
6. **Proteger con permisos**: Usar `[RequirePermission("code")]` para endpoints protegidos
7. **Manejo de errores**: Implementar estructura de respuestas en cada método
8. **Validar ModelState**: Verificar antes de procesar datos

**Ejemplo de controlador protegido:**
```csharp
[HttpGet, Authorize]
[RequirePermission("users.read")]
public IActionResult GetAll()
{
    try
    {
        var response = _methods.GetAll();
        return StatusCode(response.StatusCode, response);
    }
    catch (Exception ex)
    {
        LogError(ex);
        var response = UserResponse.InternalServerError(ex.Message);
        return StatusCode((int)response.StatusCode, response);
    }
}
```

**Snippet disponible**: `blcontroller` para generar estructura base

### Al Crear Servicios

1. **Crear carpeta**: Cada servicio en su propia carpeta bajo `Repository/DB/{Entity}Services/`
2. **Crear archivo de servicio**: `{Entity}Service.cs`
3. **Heredar de `DBService`**: Base para servicios de datos
4. **Usar propiedades**: `DbConnection` y `Transaction` (con mayúsculas)
5. **Transacciones**: Soportar transacciones opcionales en constructores
6. **Usar Dapper**: Para todas las operaciones de base de datos, no usar el AppDBContext.

**Ejemplo:**
```csharp
public class PermissionService : DBService
{
    public PermissionService(SqlConnection connection, IDbTransaction? transaction = null)
        : base(connection, transaction!)
    {
    }

    public IEnumerable<PermissionDTO> GetAll()
    {
        const string sql = "SELECT * FROM CL_Permissions WHERE Available = 1";
        return DbConnection.Query<PermissionDTO>(sql, transaction: Transaction);
    }
}
```

### Al Implementar Validaciones

1. **Usar extensiones**: Desde `GCatcode.Utils.Extensions`
2. **Validar temprano**: Fallar rápido con excepciones descriptivas
3. **Mensajes estandarizados**: Retornar mensajes de error que existan en `{Entity}Response.cs` de cada directorio del controlador.

### Al Trabajar con Autenticación y Autorización

#### **Proteger Endpoints**
```csharp
// SIEMPRE usar [Authorize] junto con [RequirePermission]
[HttpPost, Authorize]
[RequirePermission("users.write")]
public IActionResult Create([FromBody] UserInsert data) { ... }
```

#### **Validación Programática en Controladores**
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

// Obtener todos los permisos del usuario actual
var permissions = GetUserPermissions();
```

#### **Métodos Disponibles en BaseController**
```csharp
// Permisos (RBAC + ABAC)
protected bool HasPermission(string code)
protected bool HasAllPermissions(params string[] codes)
protected bool HasAnyPermission(params string[] codes)
protected IEnumerable<UserPermissionResult> GetUserPermissions()

// Información del usuario
protected int GetUserId()
protected string GetUserName()
protected string GetUserEmail()
protected IEnumerable<string> GetRoles()
protected bool IsAdmin
```

#### **No exponer información sensible**
- Nunca retornar contraseñas o tokens internos
- Base64 en passwords: El login acepta contraseñas en Base64 o texto plano

### Al Trabajar con Permisos

#### **Códigos de Permisos**
Seguir convención: `{resource}.{action}`

Ejemplos:
- `users.read` - Ver usuarios
- `users.write` - Crear/Editar usuarios
- `users.delete` - Eliminar usuarios
- `permissions.write` - Gestionar permisos
- `system.admin` - Acceso administrativo total

#### **Asignar Permisos a Roles (RBAC)**
```sql
-- Insertar nuevo permiso
INSERT INTO CL_Permissions (Code, Resource, Action, Description, CreateTime, LastUpdated, Available)
VALUES ('reports.generate', 'reports', 'generate', 'Generar reportes', GETUTCDATE(), GETUTCDATE(), 1)

-- Asignar a rol
INSERT INTO RL_RolePermissions (RolId, PermissionId, CreateTime, LastUpdated, Available)
VALUES (4, {PermissionId}, GETUTCDATE(), GETUTCDATE(), 1)
```

#### **Asignar Permisos Directos a Usuarios (ABAC)**
Usar el endpoint: `POST /api/permissions/user`
```json
{
  "userId": 5,
  "permissionId": 2,
  "isGranted": true,  // true = conceder, false = denegar
  "conditions": null  // JSON para condiciones adicionales
}
```

#### **Prioridad de Permisos**
1. **Primero**: Permisos directos del usuario (ABAC) - Siempre prevalecen
2. **Segundo**: Permisos heredados de roles (RBAC)

## 📝 Convenciones de Código

### Nomenclatura

- **Controladores**: `{Entity}Controller.cs`
- **Methods del los controladores**: `{Entity}Methods.cs`
- **Mensajes de Respuesta**: `{Entity}Response.cs`
- **Servicios**: `{Entity}Service.cs`
- **DTOs**: `{Entity}DTO.cs`, `{Entity}Insert.cs`, `{Entity}Update.cs`
- **Rutas**: Usar convención RESTful (`/api/{controller}/{action?}/{id?}`)

### Estructura de Respuestas

```csharp
try{
    var response = _methods.{action}();
    return StatusCode(response.StatusCode, response);
}catch (Exception ex)
{
    LogError(ex);
    var response = UserResponse.InternalServerError(ex.Message);
    return StatusCode((int)response.StatusCode, response);
}
```

### Manejo de Fechas

- Usar **UTC** para todas las operaciones de fecha/hora
- `DateTime.UtcNow` para timestamps
- Actualizar `LastUpdated` en modificaciones

## 🔍 Patrones de Testing

Al escribir pruebas para este proyecto:

1. **Controladores**: Probar códigos de estado, validaciones y respuestas
2. **Servicios**: Mockear `SqlConnection` y `IDbTransaction`
3. **Autenticación**: Verificar generación de tokens y claims
4. **Autorización**: Probar permisos RBAC y ABAC, y prioridad de overrides
5. **Validaciones**: Probar casos límite y datos inválidos

## 🛠️ Comandos Útiles

### Desarrollo Local

```bash
# Ejecutar la API
dotnet run --project GCatcode.Api

# Restaurar paquetes
dotnet restore

# Compilar
dotnet build

# Ejecutar con hot reload
dotnet watch run --project GCatcode.Api
```

### Base de Datos

```bash
# Crear migración
dotnet ef migrations add {{Nombre Migración}} --project GCatcode.DataBase

# Aplicar migraciones
dotnet ef database update --project GCatcode.Api
```

## ⚙️ Configuración de Ambiente

### appsettings.Development.json en GCatcode.Api

```json
{
  "AppSettings": {
    "BackendName": "Gcatcode.API",
    "Environment": "Development",
    "IsTesting": true,
    "FRONT_PUBLIC_ORIGIN": "http://localhost:5173"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Connection string aquí"
  },
  "Jwt": {
    "Key": "SecretKey",
    "Issuer": "YourIssuer",
    "Audience": "YourAudience",
    "AccessTokenExpirationMinutes": 60,
    "RefreshTokenExpirationDays": 7
  }
}
```

### Variables Requeridas

- `ConnectionStrings:DefaultConnection`: Cadena de conexión a SQL Server
- `Jwt:Key`: Clave secreta para firmar tokens (mínimo 32 caracteres)
- `Jwt:Issuer`: Emisor del token
- `Jwt:Audience`: Audiencia del token

## 📚 Recursos Adicionales

- **Swagger UI**: Disponible en `/swagger` en modo Development
- **Documentación RBAC + ABAC**: Ver `RBAC_ABAC_README.md` para guía completa
- **Implementación**: Ver `IMPLEMENTACION_COMPLETADA.md` para detalles técnicos
- **Autor**: Jorge Perez - Gcatcode
- **Snippets**: Consultar carpeta `Snippet/` para plantillas de código

## 🎓 Principios de Diseño

1. **Separación de responsabilidades**: Cada capa tiene un propósito claro
2. **Seguridad por defecto**: Autenticación y autorización en toda la API
3. **Control de acceso granular**: Sistema híbrido RBAC + ABAC para máxima flexibilidad
4. **Configuración externa**: Valores sensibles en archivos de configuración
5. **Código limpio**: Try-catch, validaciones y mensajes estandarizados
6. **Escalabilidad**: Arquitectura modular para facilitar futuras expansiones
7. **Auditoría**: Rastreo claro de origen de permisos (Role vs User)

## 🔄 Ciclo de Vida de Servicios

- **Scoped**: Todos los servicios de datos y repositorios
- **Transient**: No usado actualmente
- **Singleton**: No usado actualmente

## Git Conventions

- Ramas: `main`, `develop`, `feature/{nombre}`, `bugfix/{nombre}`, `release/{version}`, `hotfix/{nombre}`
- Commits: Mensajes claros y con listado de cambios relevantes

---

**Última actualización**: Enero 2025  
**Versión del documento**: 2.0  
**Mantenido por**: Jorge Perez - Gcatcode
