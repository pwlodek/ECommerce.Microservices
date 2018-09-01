FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 54703
EXPOSE 44314

FROM microsoft/dotnet:2.1-sdk AS build
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
