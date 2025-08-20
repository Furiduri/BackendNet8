# Readme

## Run Docker SQL Server Locally

	```bash
	docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=GcatcodeDB2521." -p 1433:1433 --name BaseLine --hostname server_development -d mcr.microsoft.com/mssql/server:2022-latest
	```