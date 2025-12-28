# Readme

## Run Docker SQL Server Locally

	```bash
	docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=GcatcodeDB2521." -p 1433:1433 --name BaseLine --hostname server_development -d mcr.microsoft.com/mssql/server:2022-latest
	```

## Run Entity Framework Creation db
	```bash
	dotnet ef database update --project ../Gcatcode.Data/Gcatcode.Data.csproj --startup-project ../Gcatcode.API/Gcatcode.API.csproj
	```

## Packege Manager Console Command
	```bash
	Update-Database -Project Gcatcode.SQLServerDatabase -StartupProject Gcatcode.Api
	```