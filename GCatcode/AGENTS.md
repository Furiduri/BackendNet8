# AGENTS.md - Configuración para Asistentes de IA

## 📋 Información del Proyecto

**Nombre del Proyecto:** GCatcode Backend API  
**Framework:** ASP.NET Core 8.0  
**Tipo:** Web API RESTful  
**Repositorio:** https://github.com/Furiduri/BackendNet8  
**Rama Principal:** main
**Rama Develop**: develop
**Autor:** Jorge Perez - Gcatcode
**Modelo de Desarrollo:** Agile/Scrum

## 🎯 Propósito del Proyecto

API backend construida en .NET 8 que proporciona servicios de autenticación, gestión de usuarios y control de roles con JWT. El proyecto implementa una arquitectura en capas con Entity Framework Core y Dapper.

## 🏗️ Arquitectura del Proyecto

### Estructura de Capas

```
GCatcode.Api/              # Capa de presentación (API)
├── Controllers/           # Controladores de API
├─── {Nombre de ruta}/   # Controladores específicos por ruta
├── Core/                  # Funcionalidad base y autenticación
├─── Auth/                # Lógica de autenticación JWT
└── Program.cs            # Punto de entrada y configuración

GCatcode.Repository/       # Capa de lógica de negocio
├── DB/                   # Servicios de acceso a datos
└─── {Entidad}Services/    # Servicios específicos por entidad

GCatcode.DataBase/         # Capa de acceso a datos
└── AppDBContext.cs       # Contexto de Entity Framework

GCatcode.Utils/            # Utilidades compartidas
└── Extensions/           # Métodos de extensión

TestUnit/                  # Proyecto de pruebas unitarias
```

### Patrones Implementados

- **Repository Pattern**: Servicios de datos (`UserService`, `RolesService`, `UserRolService`)
- **Dependency Injection**: Configurado en `Program.cs`
- **Base Controller Pattern**: `BaseController` para funcionalidad compartida
- **JWT Authentication**: Autenticación basada en tokens

## 🔧 Tecnologías y Dependencias Principales

- **ASP.NET Core 8.0**: Framework principal
- **Entity Framework Core**: ORM con SQL Server
- **Dapper**: Micro-ORM para consultas optimizadas
- **JWT Bearer**: Autenticación con tokens
- **Argon2**: Hashing de contraseñas
- **Swagger/OpenAPI**: Documentación de API

## 🔐 Configuración de Seguridad

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

### Entidades Principales

- **Users**: Usuarios del sistema
- **Roles**: Roles disponibles (Admin, User, Guest)
- **UserRoles**: Relación muchos a muchos entre usuarios y roles

## 🚀 Endpoints Principales

### Autenticación (`/api/Auth`)

- `POST /login`: Autenticación de usuarios
- `POST /register`: Registro de nuevos usuarios
- `GET /user_info`: Información del usuario autenticado (requiere auth)

### Test (`/api/Test`)

- `GET /`: Endpoint de prueba
- `GET /{id}`: Obtener por ID
- `POST /`: Crear recurso
- `PUT /`: Actualizar recurso
- `DELETE /{id}`: Eliminar recurso

## 💻 Guías para Agentes de IA

### Al Generar Controladores

1. **Crear Carpeta**: Cada controlador en su propia carpeta bajo `Controllers/{Entity}/`
2. **Crear Archivo de Métodos**: `{Entity}Methods.cs` para lógica específica
3. **Crear Archivo de Mensajes**: `{Entity}ResponseMessage.cs` para mensajes estandarizados
4. **Heredar de `BaseController`**: Todos los controladores deben heredar de esta clase
5. **Usar atributos de ruta**: `[ApiController, Route("api/[controller]")]`
6. **Manejo de errores**: Implementar estructura de respuestas en cada método
7. **Validar ModelState**: Verificar antes de procesar datos

**Snippet disponible**: `blcontroller` para generar estructura base

### Al Crear Servicios

1. **Crear carpeta**: Cada servicio en su propia carpeta bajo `Repository/DB/{Entity}Services/`
2. **Crear archivo de servicio**: `{Entity}Service.cs`
3. **Heredar de `DBService`**: Base para servicios de datos
4. **Transacciones**: Soportar transacciones opcionales en constructores
5. **Usar Dapper**: Para todas las operaciones de base de datos

### Al Implementar Validaciones

1. **Usar extensiones**: Desde `GCatcode.Utils.Extensions`
2. **Validar temprano**: Fallar rápido con excepciones descriptivas
3. **Mensajes estandarizados**: Retornar mensajes de error que existan en el enumerable `{Entity}ResponseMessage` de cada directorio del controlador o `ResponseMessageCommon` en Core.

### Al Trabajar con Autenticación

1. **Proteger endpoints**: Usar `[Authorize]` cuando sea necesario
2. **Obtener usuario actual**: Usar `GetUserId()` y `GetRoles()` de `BaseController`
3. **No exponer información sensible**: Nunca retornar contraseñas o tokens internos
4. **Base64 en passwords**: El login acepta contraseñas en Base64 o texto plano

## 📝 Convenciones de Código

### Nomenclatura

- **Controladores**: `{Entity}Controller.cs`
- **Methods del los controladores**: `{Entity}Methods.cs`
- **Mensajes de Respuesta**: `{Entity}ResponseMessage.cs`
- **Servicios**: `{Entity}Service.cs`
- **DTOs**: `{Entity}DTO.cs`, `{Entity}Insert.cs`, `{Entity}Update.cs`
- **Rutas**: Usar convención RESTful (`/api/{controller}/{action?}/{id?}`)

### Estructura de Respuestas

```csharp
try{
    ApiResponse response = _methods.{action}();
    if (response.Success)
    {// Retornar 200 OK
        return Ok(response);
    }
    else
    {
      // Retornar 400 Bad Request
        return BadRequest(response);
    }
}catch (Exception ex)
{
    LogError(ex);
    return BadRequest($"{ResponseMessageCommon.InternalServerError.ToMsgString()} : {ex.Message}");
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
4. **Validaciones**: Probar casos límite y datos inválidos

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
    "Audience": "YourAudience"
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
- **Autor**: Jorge Perez - Gcatcode
- **Snippets**: Consultar carpeta `Snippet/` para plantillas de código

## 🎓 Principios de Diseño

1. **Separación de responsabilidades**: Cada capa tiene un propósito claro
2. **Seguridad por defecto**: Autenticación y autorización en toda la API
3. **Configuración externa**: Valores sensibles en archivos de configuración
4. **Código limpio**: Try-catch, validaciones y mensajes estandarizados
5. **Escalabilidad**: Arquitectura modular para facilitar futuras expansiones

## 🔄 Ciclo de Vida de Servicios

- **Scoped**: Todos los servicios de datos y repositorios
- **Transient**: No usado actualmente
- **Singleton**: No usado actualmente

## Git Conventions

- Ramas: `main`, `develop`, `feature/{nombre}`, `bugfix/{nombre}`, `release/{version}`, `hotfix/{nombre}`
- Commits: Mensajes claros y con listado de cambios relevantes

---

**Última actualización**: Enero 2025  
**Versión del documento**: 1.0  
**Mantenido por**: Jorge Perez - Gcatcode
