FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY Services/ECommerce.Sales.Api/ECommerce.Sales.Api.csproj Services/ECommerce.Sales.Api/
COPY Services/ECommerce.Common/ECommerce.Common.csproj Services/ECommerce.Common/
COPY Services/ECommerce.Services.Common/ECommerce.Services.Common.csproj Services/ECommerce.Services.Common/
RUN dotnet restore Services/ECommerce.Sales.Api/ECommerce.Sales.Api.csproj
COPY . .
WORKDIR /src/Services/ECommerce.Sales.Api
RUN dotnet build ECommerce.Sales.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ECommerce.Sales.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ECommerce.Sales.Api.dll"]
