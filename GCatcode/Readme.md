# GCatcode Backend API

Backend desarrollado en .NET 8 con arquitectura en capas, sistema de autenticación JWT y gestión de usuarios y roles.

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
- [Testing](#testing)

## ✨ Características

- 🔐 **Autenticación JWT**: Sistema completo de autenticación con tokens JWT
- 👥 **Gestión de Usuarios**: CRUD completo de usuarios con cambio de contraseña
- 🛡️ **Sistema de Roles**: Gestión de roles y permisos
- 🗄️ **Entity Framework Core**: ORM para manejo de base de datos
- 🎯 **Dapper**: Para consultas optimizadas
- 📊 **SQL Server**: Base de datos en contenedor Docker
- 📝 **Swagger**: Documentación interactiva de la API
- 🔄 **CORS**: Configurado para desarrollo frontend

## 🛠️ Tecnologías

- **.NET 8.0**
- **ASP.NET Core Web API**
- **Entity Framework Core 9.0**
- **SQL Server 2022** (Docker)
- **Dapper** (consultas SQL)
- **JWT Bearer Authentication**
- **Swagger/OpenAPI**
- **xUnit** (Testing)

## 📁 Estructura del Proyecto

```
BackendNet8/
├── GCatcode.Api/                    # Capa de presentación (API)
│   ├── Controllers/                 # Controladores de la API
│   │   ├── Roles/                  # Gestión de roles
│   │   └── Users/                  # Gestión de usuarios
│   ├── Core/                       # Funcionalidades core
│   │   ├── Auth/                   # Autenticación y autorización
│   │   ├── BaseController.cs      # Controlador base
│   │   └── ApiResponse.cs         # Respuestas estandarizadas
│   └── Configuration/              # Configuración de la aplicación
│
├── GCatcode.DataBase/              # Capa de acceso a datos (EF Core)
│   ├── Models/                     # Entidades del dominio
│   ├── Configurations/             # Configuraciones de EF Core
│   ├── Migrations/                 # Migraciones de BD
│   └── AppDBContext.cs            # Contexto de base de datos
│
├── GCatcode.Services/              # Capa de servicios (Repositorios)
│   └── DB/                        # Servicios de base de datos
│       ├── UserServices/          # Servicios de usuarios
│       ├── RolServices/           # Servicios de roles
│       └── UserRolServices/       # Relación usuarios-roles
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
    "Key": "tu-clave-secreta-aqui",
    "Issuer": "Gcatcode.com",
    "Audience": "Gcatcode.com"
  }
}
```

### 3. CORS

El frontend está configurado para ejecutarse en:
- `http://localhost:5173` (Vite/React)

Puedes modificar los orígenes permitidos en `Program.cs`.

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
| POST | `/api/Auth/login` | Iniciar sesión y obtener JWT | No |
| GET | `/api/Auth/user_info` | Obtener información del usuario actual | Sí |

### Usuarios

| Método | Endpoint | Descripción | Auth |
|--------|----------|-------------|------|
| GET | `/api/Users` | Listar usuarios | Sí |
| GET | `/api/Users/{id}` | Obtener usuario por ID | Sí |
| POST | `/api/Users` | Crear nuevo usuario | Sí |
| PUT | `/api/Users/{id}` | Actualizar usuario | Sí |
| DELETE | `/api/Users/{id}` | Eliminar usuario | Sí |

### Roles

| Método | Endpoint | Descripción | Auth |
|--------|----------|-------------|------|
| GET | `/api/Roles` | Listar roles | Sí |
| GET | `/api/Roles/{id}` | Obtener rol por ID | Sí |
| POST | `/api/Roles` | Crear nuevo rol | Sí |
| PUT | `/api/Roles/{id}` | Actualizar rol | Sí |
| DELETE | `/api/Roles/{id}` | Eliminar rol | Sí |

### Ejemplo de Login

```bash
curl -X POST https://localhost:7000/api/Auth/login \
  -H "Content-Type: application/json" \
  -d '{
    "username": "admin",
    "password": "password123"
  }'
```

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

- La API usa **autenticación JWT Bearer**
- Las contraseñas deben enviarse en formato **Base64**
- Los endpoints protegidos requieren el header: `Authorization: Bearer {token}`
- El entorno de desarrollo tiene validaciones JWT relajadas para facilitar el desarrollo
- CORS está habilitado para `http://localhost:5173`

## 🔒 Seguridad

⚠️ **Importante para Producción:**
- Cambiar la clave JWT en `appsettings.json`
- Usar secretos de Azure Key Vault o variables de entorno
- Modificar la contraseña de SQL Server
- Habilitar validaciones JWT completas
- Configurar HTTPS correctamente
- Revisar políticas de CORS

## 📄 Licencia

Este proyecto es privado. Todos los derechos reservados.

## 👥 Autor

**Furiduri**
- GitHub: [@Furiduri](https://github.com/Furiduri)

## 🤝 Contribuciones

Este es un proyecto privado. Las contribuciones están restringidas a colaboradores autorizados.

---

**Versión:** 0.1.0  
**Última actualización:** Febrero 2026
