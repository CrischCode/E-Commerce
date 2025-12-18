FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiamos SOLO el csproj de la API
COPY Ecommerce.API/Ecommerce.API.csproj Ecommerce.API/
RUN dotnet restore Ecommerce.API/Ecommerce.API.csproj

# Copiamos el resto
COPY . .
WORKDIR /src/Ecommerce.API

RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Ecommerce.API.dll"]
