# Verificar si el contenedor existe
$containerExists = docker ps -a --filter "name=^BaseLine$" --format "{{.Names}}"

if ($containerExists) {
    # Si existe, verificar si está corriendo
    $containerRunning = docker ps --filter "name=^BaseLine$" --format "{{.Names}}"
    
    if ($containerRunning) {
        Write-Host "El contenedor 'BaseLine' ya está en ejecución." -ForegroundColor Green
    } else {
        Write-Host "Iniciando el contenedor 'BaseLine'..." -ForegroundColor Yellow
        docker start BaseLine
        Write-Host "Contenedor 'BaseLine' iniciado correctamente." -ForegroundColor Green
    }
} else {
    # Si no existe, crear y ejecutar el contenedor
    Write-Host "Creando y ejecutando el contenedor 'BaseLine'..." -ForegroundColor Yellow
    docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=GcatcodeDB2521." -p 1433:1433 --name BaseLine --hostname server_development -d mcr.microsoft.com/mssql/server:2022-latest
    Write-Host "Contenedor 'BaseLine' creado e iniciado correctamente." -ForegroundColor Green
}

# Esperar unos segundos para asegurarse de que SQL Server esté listo
Start-Sleep -Seconds 10
# Ejecutar las migraciones de Entity Framework Core
Write-Host "Aplicando migraciones de Entity Framework Core..." -ForegroundColor Yellow
dotnet ef database update --project Gcatcode.DataBase/Gcatcode.DataBase.csproj --startup-project Gcatcode.API/Gcatcode.API.csproj
Write-Host "Migraciones aplicadas correctamente." -ForegroundColor Green