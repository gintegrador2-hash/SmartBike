FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copia solo la carpeta del API y las dependencias si las hay
COPY . . 

# Publica específicamente el proyecto del API para evitar ambigüedades
RUN dotnet publish "SmartBike/SmartBike.API.csproj" -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .

# Render inyectará el puerto en la variable PORT
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}

# Asegúrate de que el nombre coincida exactamente con el archivo generado
ENTRYPOINT ["dotnet", "SmartBike.API.dll"]