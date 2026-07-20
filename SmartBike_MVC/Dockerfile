# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiamos todo el repositorio (esto incluye Consumer, Modelos y SmartBike_MVC)
COPY . .

# Publicamos específicamente el proyecto del MVC
# Como estamos en la raíz del repo, la ruta es SmartBike_MVC/SmartBike_MVC.csproj
RUN dotnet publish "SmartBike_MVC/SmartBike_MVC.csproj" -c Release -o out

# Etapa final de ejecución
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
# Copiamos el resultado de la publicación desde la etapa anterior
COPY --from=build /app/out .

# Configuración del puerto para Render
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}

# Punto de entrada para ejecutar la aplicación
ENTRYPOINT ["dotnet", "SmartBike_MVC.dll"]