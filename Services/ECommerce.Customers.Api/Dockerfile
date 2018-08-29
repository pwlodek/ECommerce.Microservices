FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY Services/ECommerce.Customers.Api/ECommerce.Customers.Api.csproj Services/ECommerce.Customers.Api/
COPY Services/ECommerce.Services.Common/ECommerce.Services.Common.csproj Services/ECommerce.Services.Common/
COPY Services/ECommerce.Common/ECommerce.Common.csproj Services/ECommerce.Common/
RUN dotnet restore Services/ECommerce.Customers.Api/ECommerce.Customers.Api.csproj
COPY . .
WORKDIR /src/Services/ECommerce.Customers.Api
RUN dotnet build ECommerce.Customers.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ECommerce.Customers.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ECommerce.Customers.Api.dll"]
