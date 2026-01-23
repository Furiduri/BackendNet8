# Repository Guidelines

## How to Use This Guide

- Start here for cross-project norms. BackendNet8 is a monorepo with several components.
- Each component has an `AGENTS.md` file with specific guidelines (e.g., `web-app/AGENTS.md`, `GCatCode/AGENTS.md`, `GCatCode/GCatCode.Api/AGENTS.md`)
- Component docs override this file when guidance conflicts.

## Available Skills

Use these skills for detailed patterns on-demand:

### Generic Skills (Any Project)

| Skill        | Description                                 | URL                                    |
| ------------ | ------------------------------------------- | -------------------------------------- |
| `typescript` | Const types, flat interfaces, utility types | [SKILL.md](skills/typescript/SKILL.md) |

### BackendNet8-Specific Skills

| Skill      | Description                                                           | URL                                  |
| ---------- | --------------------------------------------------------------------- | ------------------------------------ |
| `GCatCode` | Project in Net Core 8, Api, Services, SQLServer, Utils, and TestUnits | [SKILL.md](skills/GCatCode/SKILL.md) |
| `web-app`  | Nuxt 3 + TypeScript + Pinia + unocss + i18n + pwa                     | [SKILL.md](skills/web-app/SKILL.md)  |

### Auto-invoke Skills

When performing these actions, ALWAYS invoke the corresponding skill FIRST:

| Action                 | Skill      |
| ---------------------- | ---------- |
| Backend Modifications  | `GCatCode` |
| Frontend Modifications | `web-app`  |

## Project Overview

BackendNet8 is a monorepo with several components:

| Component | Location                      | Tech Stack                                   |
| --------- | ----------------------------- | -------------------------------------------- |
| API       | `GCatCode/GCatCode.Api/`      | .NET 8, API                                  |
| Services  | `GCatCode/GCatCode.Services/` | .NET 8, Services                             |
| DataBase  | `GCatCode/Gcatcode.DataBase/` | .NET 8, SQLServer                            |
| Utils     | `GCatCode/Utils/`             | .NET 8                                       |
| TestUnits | `GCatCode/TestUnits/`         | .NET 8, MSTest                               |
| web-app   | `web-app/`                    | Nuxt 3, TypeScript, Pinia, unocss, i18n, PWA |

## Net Core 8 Development

```bash
    # Setup
    docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=GcatcodeDB2521." -p 1433:1433 --name BaseLine --hostname server_development -d mcr.microsoft.com/mssql/server:2022-latest

   dotnet ef database update --project ../Gcatcode.DataBase/Gcatcode.DataBase.csproj --startup-project ../Gcatcode.API/Gcatcode.API.csproj

   Update-Database -Project Gcatcode.DataBase -StartupProject Gcatcode.Api
```

## Commit & Pull Request Guidelines

Follow conventional-commit style: `<type>[scope]: <description>`

**Types:** `feature`, `fix`, `docs`, `chore`, `perf`, `refactor`, `style`, `test`

Not create pull requests, just commit and push.
