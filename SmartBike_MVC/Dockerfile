FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app
COPY . .

# Ajuste: Buscar el csproj dentro de la carpeta actual (que ya es SmartBike_MVC)
# Si tu .csproj está en la raíz de esta carpeta, este comando debería funcionar:
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /app/out .
ENV ASPNETCORE_URLS=http://+:${PORT:-8080}
ENTRYPOINT ["dotnet", "SmartBike_MVC.dll"]