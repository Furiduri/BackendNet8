FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY GCatcode/GCatcode.Api/GCatcode.Api.csproj GCatcode/GCatcode.Api/
COPY GCatcode/GCatcode.DataBase/GCatcode.DataBase.csproj GCatcode/GCatcode.DataBase/
COPY GCatcode/GCatcode.Services/GCatcode.Services.csproj GCatcode/GCatcode.Services/
COPY GCatcode/Utils/GCatcode.Utils.csproj GCatcode/Utils/
RUN dotnet restore GCatcode/GCatcode.Api/GCatcode.Api.csproj

COPY GCatcode/ GCatcode/
RUN dotnet publish GCatcode/GCatcode.Api/GCatcode.Api.csproj -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080

COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "GCatcode.Api.dll"]
