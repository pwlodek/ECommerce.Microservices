FROM mcr.microsoft.com/dotnet/core/aspnet:2.2 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2 AS build
WORKDIR /src
COPY Web/ECommerce.WebApp/ECommerce.WebApp.csproj Web/ECommerce.WebApp/
RUN dotnet restore Web/ECommerce.WebApp/ECommerce.WebApp.csproj
COPY . .
WORKDIR /src/Web/ECommerce.WebApp
RUN dotnet build ECommerce.WebApp.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish ECommerce.WebApp.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "ECommerce.WebApp.dll"]
