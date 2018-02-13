# Use latest aspnetcore image
FROM microsoft/aspnetcore:latest

# Expose port 80 so that it is accessible from outside the container
EXPOSE 80

# Set workdir
WORKDIR /app

# Copy published app into /app
COPY /bin/Debug/netcoreapp2.0/publish .

# Run the app
ENTRYPOINT ["dotnet", "ECommerce.Sales.Api.dll"]