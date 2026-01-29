# GCatcode Backend API

Backend API construida en .NET 8 que proporciona servicios de autenticación, gestión de usuarios y control de roles mediante JWT. El proyecto implementa una arquitectura en capas robusta utilizando Entity Framework Core para la gestión de esquemas y Dapper para consultas de alto rendimiento.

## 🚀 Características Principales

- **Autenticación y Seguridad**: Implementación de JWT Bearer para autenticación segura y Argon2 para el hashing de contraseñas.
- **Arquitectura Limpia**: Diseño en capas separando responsabilidades (Api, Repository, DataBase, Utils).
- **Alto Rendimiento**: Uso híbrido de ORMs (EF Core + Dapper) para optimizar el acceso a datos.
- **Patrones de Diseño**: Repository Pattern, Dependency Injection y Base Controller para estandarización.

## 🛠️ Tecnologías

- **Framework**: ASP.NET Core 8.0
- **Base de Datos**: Microsoft SQL Server
- **Acceso a Datos**: Entity Framework Core y Dapper
- **Autenticación**: JWT (JSON Web Tokens)
- **Documentación**: Swagger/OpenAPI

## 📂 Estructura del Proyecto

La solución está organizada en los siguientes proyectos:

- **GCatcode.Api**: Capa de presentación que contiene los controladores REST, configuración de autenticación y punto de entrada.
- **GCatcode.Repository**: Capa de lógica de negocio y servicios de acceso a datos utilizando Dapper.
- **GCatcode.DataBase**: Contiene el contexto de Entity Framework, migraciones y definiciones de modelos.
- **GCatcode.Utils**: Utilidades transversales, helpers y métodos de extensión.
- **TestUnit**: Proyecto dedicado a pruebas unitarias.

## ⚡ Guía de Inicio Rápido

### Requisitos Previos

- .NET 8 SDK
- SQL Server

### Configuración

Asegúrate de configurar tu cadena de conexión y las claves JWT en el archivo `appsettings.Development.json` dentro del proyecto `GCatcode.Api`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=...;Database=...;"
  },
  "Jwt": {
    "Key": "TuClaveSecreta",
    "Issuer": "TuEmisor",
    "Audience": "TuAudiencia"
  }
}
```

### Ejecución Local

```bash
# Restaurar paquetes
dotnet restore

# Levantar la base de datos en docker (opcional)
./GCatcode/Start-BaseLineContainer.ps1

# Iniciar la aplicación
dotnet run --project GCatcode.Api
```

Una vez iniciado, puedes acceder a la documentación de Swagger generalmente en: `https://localhost:7028/swagger` (o el puerto configurado).

## 📄 Créditos

**Autor**: Jorge Perez - Gcatcode
**Repositorio**: [Github - BackendNet8](https://github.com/Furiduri/BackendNet8)
