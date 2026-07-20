FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .
# Publicamos solo el proyecto del API
# Cambia tu línea 5 por estas dos líneas:
RUN ls -R
# Cambia esta línea:
RUN dotnet publish -c Release -o out
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
# Render inyectará el puerto en la variable PORT
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
ENTRYPOINT ["dotnet", "SmartBike_API.dll"]