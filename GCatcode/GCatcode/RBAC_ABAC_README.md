# ?? Sistema de Permisos Híbrido RBAC + ABAC

## ?? Descripción General

Este sistema implementa un **enfoque híbrido** que combina:
- **RBAC (Role-Based Access Control)**: Control de acceso basado en roles
- **ABAC (Attribute-Based Access Control)**: Control de acceso basado en atributos

### Características Principales

? **Herencia de permisos de roles** (RBAC)  
? **Permisos directos por usuario** (ABAC)  
? **Prioridad de permisos directos** sobre heredados  
? **Concesión y revocación granular** de permisos  
? **Gestión de vistas y menús** por rol  
? **Validación mediante atributos** `[RequirePermission]`

---

## ??? Arquitectura

### Modelos de Base de Datos

#### 1. **CL_Permissions** (Catálogo de Permisos)
```sql
- PermissionId (PK)
- Code (unique, ej: "users.read")
- Resource (ej: "users", "roles")
- Action (ej: "read", "write", "delete")
- Description
```

#### 2. **RL_RolePermissions** (RBAC)
```sql
- RolePermissionId (PK)
- RolId (FK -> CL_Roles)
- PermissionId (FK -> CL_Permissions)
```

#### 3. **RL_UserPermissions** (ABAC - Overrides)
```sql
- UserPermissionId (PK)
- UserId (FK -> TR_Users)
- PermissionId (FK -> CL_Permissions)
- IsGranted (true = conceder, false = denegar)
- Conditions (JSON para condiciones adicionales)
```

---

## ?? Uso del Sistema

### 1. Proteger Endpoints con Permisos

```csharp
[HttpGet, Authorize]
[RequirePermission("users.read")]
public IActionResult GetAll()
{
    // Solo usuarios con el permiso "users.read" pueden acceder
}

[HttpPost, Authorize]
[RequirePermission("users.write")]
public IActionResult Create([FromBody] UserInsert data)
{
    // Solo usuarios con el permiso "users.write" pueden crear
}

[HttpDelete("{id}"), Authorize]
[RequirePermission("users.delete")]
public IActionResult Delete(int id)
{
    // Solo usuarios con el permiso "users.delete" pueden eliminar
}
```

### 2. Validación Programática en Controladores

```csharp
// Verificar un permiso específico
if (!HasPermission("users.write"))
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

### 3. Gestión de Permisos desde la API

#### Obtener todos los permisos
```http
GET /api/permissions
Authorization: Bearer {token}
```

#### Obtener permisos del usuario actual
```http
GET /api/permissions/my-permissions
Authorization: Bearer {token}
```

#### Asignar permiso directo a un usuario (ABAC)
```http
POST /api/permissions/user
Content-Type: application/json
Authorization: Bearer {token}

{
  "userId": 5,
  "permissionId": 2,
  "isGranted": true,
  "conditions": null
}
```

#### Denegar un permiso a un usuario
```http
POST /api/permissions/user
Content-Type: application/json
Authorization: Bearer {token}

{
  "userId": 5,
  "permissionId": 3,
  "isGranted": false
}
```

#### Asignar permiso a un rol (RBAC)
```http
POST /api/permissions/role
Content-Type: application/json
Authorization: Bearer {token}

{
  "roleId": 3,
  "permissionId": 1
}
```

#### Revocar permiso de un rol
```http
DELETE /api/permissions/role/{roleId}/{permissionId}
Authorization: Bearer {token}
```

---

## ?? Gestión de Vistas (Menús)

### Endpoints Disponibles

#### Obtener todas las vistas
```http
GET /api/views
Authorization: Bearer {token}
```

#### Obtener vistas del usuario actual
```http
GET /api/views/my-views
Authorization: Bearer {token}
```

#### Crear una nueva vista
```http
POST /api/views
Content-Type: application/json
Authorization: Bearer {token}

{
  "name": "Dashboard",
  "route": "/dashboard",
  "icon": "dashboard",
  "description": "Panel principal",
  "parentViewId": null,
  "order": 1,
  "isActive": true
}
```

#### Asignar roles a una vista
```http
PUT /api/views/{viewId}/roles
Content-Type: application/json
Authorization: Bearer {token}

[1, 3, 4]  // IDs de roles
```

#### Asignar un rol a una vista
```http
POST /api/views/{viewId}/roles/{roleId}
Authorization: Bearer {token}
```

#### Remover un rol de una vista
```http
DELETE /api/views/{viewId}/roles/{roleId}
Authorization: Bearer {token}
```

---

## ?? Flujo de Autorización

### Proceso de Verificación de Permisos

```
1. Usuario hace request a endpoint protegido
   ?
2. [RequirePermission("code")] intercepta la petición
   ?
3. Extrae UserId del token JWT
   ?
4. Busca permiso directo del usuario (ABAC)
   ?? ? Si existe ? Usa ese valor (IsGranted)
   ?? ? Si no existe ? Busca en permisos de roles (RBAC)
   ?
5. Retorna resultado:
   ?? ? Acceso concedido (200/201)
   ?? ? Acceso denegado (403)
```

### Prioridad de Permisos

1. **Primero**: Permisos directos del usuario (ABAC)
2. **Segundo**: Permisos heredados de roles (RBAC)

**Ejemplo:**
- Usuario tiene rol "User" que tiene permiso "users.read"
- Se le asigna permiso directo "users.read" con `IsGranted = false`
- **Resultado**: El usuario NO puede leer usuarios (prioridad ABAC)

---

## ?? Permisos Predefinidos

### Usuarios
- `users.read` - Ver usuarios
- `users.write` - Crear/Editar usuarios
- `users.delete` - Eliminar usuarios

### Roles
- `roles.read` - Ver roles
- `roles.write` - Crear/Editar roles
- `roles.delete` - Eliminar roles

### Permisos
- `permissions.read` - Ver permisos
- `permissions.write` - Asignar permisos

### Vistas
- `views.read` - Ver vistas/menús
- `views.write` - Gestionar vistas/menús

### Sistema
- `system.admin` - Administración total
- `system.config` - Configurar sistema

---

## ?? Configuración Inicial

### 1. Ejecutar Migración

```bash
dotnet ef migrations add AddPermissionsRBACandABAC --project GCatcode.DataBase
dotnet ef database update --project GCatcode.Api
```

### 2. Verificar Seed de Datos

La migración creará automáticamente:
- ? 12 permisos base del sistema
- ? Asignaciones de permisos a roles existentes
- ? Estructura de tablas para RBAC + ABAC

### 3. Asignaciones por Rol (Default)

**Dev (RolId = 1)**
- Todos los permisos del sistema

**Admin (RolId = 4)**
- users.* (read, write, delete)
- roles.* (read, write)
- permissions.* (read, write)
- views.* (read, write)

**User (RolId = 3)**
- users.read
- roles.read
- views.read

**Guest (RolId = 2)**
- views.read

---

## ?? Casos de Uso

### Caso 1: Usuario con Permisos Especiales

**Escenario**: Un usuario "User" necesita poder crear usuarios temporalmente.

```csharp
// Asignar permiso directo
POST /api/permissions/user
{
  "userId": 10,
  "permissionId": 2,  // users.write
  "isGranted": true
}

// Ahora el usuario puede crear usuarios aunque su rol no lo permita
```

### Caso 2: Bloquear Permiso a un Admin

**Escenario**: Un admin específico no debe poder eliminar usuarios.

```csharp
// Denegar permiso directo
POST /api/permissions/user
{
  "userId": 5,
  "permissionId": 3,  // users.delete
  "isGranted": false
}

// Ahora ese admin NO puede eliminar usuarios
```

### Caso 3: Gestión de Menús por Rol

**Escenario**: Solo Admins y Users deben ver el menú "Reportes".

```csharp
// Crear vista
POST /api/views
{
  "name": "Reportes",
  "route": "/reportes",
  "icon": "chart",
  "order": 5
}

// Asignar roles
PUT /api/views/{viewId}/roles
[3, 4]  // User, Admin
```

---

## ?? Testing

### Probar Permisos desde Swagger

1. Autenticarse y obtener token JWT
2. Usar token en header `Authorization: Bearer {token}`
3. Probar endpoints protegidos con diferentes usuarios
4. Verificar respuestas:
   - **200/201**: Permiso concedido
   - **401**: No autenticado
   - **403**: Sin permisos

---

## ?? Documentación Adicional

### BaseController - Métodos Disponibles

```csharp
// Métodos heredados en todos los controladores

protected bool HasPermission(string code)
protected bool HasAllPermissions(params string[] codes)
protected bool HasAnyPermission(params string[] codes)
protected IEnumerable<UserPermissionResult> GetUserPermissions()

protected int GetUserId()
protected string GetUserName()
protected string GetUserEmail()
protected IEnumerable<string> GetRoles()
protected bool IsAdmin
```

### Extender Permisos

Para agregar nuevos permisos:

1. **Insertar en la tabla CL_Permissions**
```sql
INSERT INTO CL_Permissions (Code, Resource, Action, Description, CreateTime, LastUpdated, Available)
VALUES ('reports.generate', 'reports', 'generate', 'Generar reportes', GETUTCDATE(), GETUTCDATE(), 1)
```

2. **Asignar a roles según necesidad**
```sql
INSERT INTO RL_RolePermissions (RolId, PermissionId, CreateTime, LastUpdated, Available)
VALUES (4, {PermissionId}, GETUTCDATE(), GETUTCDATE(), 1)
```

3. **Usar en endpoints**
```csharp
[HttpGet("generate"), Authorize]
[RequirePermission("reports.generate")]
public IActionResult GenerateReport() { ... }
```

---

## ?? Consideraciones de Seguridad

1. ? **Siempre usar `[Authorize]`** junto con `[RequirePermission]`
2. ? **Validar permisos en lógica de negocio** crítica
3. ? **No exponer información sensible** en respuestas 403
4. ? **Auditar cambios de permisos** (implementar logging)
5. ? **Revisar periódicamente** permisos directos de usuarios

---

## ?? Roadmap Futuro

- [ ] Auditoría de cambios de permisos
- [ ] Condiciones ABAC avanzadas (horarios, IPs, contextos)
- [ ] Cache de permisos en memoria
- [ ] Dashboard de gestión de permisos
- [ ] Exportar/Importar configuración de permisos

---

## ?? Soporte

Para preguntas o problemas:
- **Autor**: Jorge Perez - Gcatcode
- **Repository**: [BackendNet8](https://github.com/Furiduri/BackendNet8)

---

**Última actualización**: Enero 2025  
**Versión**: 1.0.0
