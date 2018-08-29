FROM microsoft/dotnet:2.0-runtime AS base
WORKDIR /app

FROM microsoft/dotnet:2.0-sdk AS build
WORKDIR /src
COPY Services/ECommerce.Logging.Host/ECommerce.Logging.Host.csproj Services/ECommerce.Logging.Host/
COPY Services/ECommerce.Common/ECommerce.Common.csproj Services/ECommerce.Common/
COPY Services/ECommerce.Services.Common/ECommerce.Services.Common.csproj Services/ECommerce.Services.Common/
RUN dotnet restore Services/ECommerce.Logging.Host/ECommerce.Logging.Host.csproj
COPY . .
WORKDIR /src/Services/ECommerce.Logging.Host
RUN dotnet build ECommerce.Logging.Host.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ECommerce.Logging.Host.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ECommerce.Logging.Host.dll"]
