FROM microsoft/aspnetcore-build AS builder
WORKDIR /source
ARG CONFIG

# copy projects 
COPY . .

# Set working directory to our service
WORKDIR /source/Services/ECommerce.Sales.Api

# restore libraries
RUN dotnet restore

# build
RUN dotnet publish --configuration ${CONFIG:-Debug} --output /app --no-restore

# build runtime image
FROM microsoft/aspnetcore:latest
EXPOSE 80
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "ECommerce.Sales.Api.dll"]