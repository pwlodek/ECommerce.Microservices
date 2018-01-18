FROM microsoft/aspnetcore:latest

EXPOSE 80
WORKDIR /app
COPY /bin/Debug/netcoreapp2.0/publish .

ENTRYPOINT ["dotnet", "ECommerce.Catalog.Api.dll"]