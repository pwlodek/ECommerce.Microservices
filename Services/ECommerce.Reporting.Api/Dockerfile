FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY Services/ECommerce.Reporting.Api/ECommerce.Reporting.Api.csproj Services/ECommerce.Reporting.Api/
COPY Services/ECommerce.Common/ECommerce.Common.csproj Services/ECommerce.Common/
COPY Services/ECommerce.Services.Common/ECommerce.Services.Common.csproj Services/ECommerce.Services.Common/
RUN dotnet restore Services/ECommerce.Reporting.Api/ECommerce.Reporting.Api.csproj
COPY . .
WORKDIR /src/Services/ECommerce.Reporting.Api
RUN dotnet build ECommerce.Reporting.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ECommerce.Reporting.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ECommerce.Reporting.Api.dll"]
