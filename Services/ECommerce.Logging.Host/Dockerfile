FROM microsoft/dotnet:sdk AS builder
WORKDIR /source
ARG CONFIG

# copy projects 
COPY . .

# Set working directory to our service
WORKDIR /source/Services/ECommerce.Logging.Host

# restore libraries
RUN dotnet restore

# build
RUN dotnet publish --configuration ${CONFIG:-Debug} --output /app

# build runtime image
FROM microsoft/dotnet:runtime
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "ECommerce.Logging.Host.dll"]