FROM microsoft/dotnet:sdk AS builder
WORKDIR /source
ARG CONFIG

# copy projects 
COPY . .

# Set working directory to our service
WORKDIR /source/Services/ECommerce.Payment.Host

# restore libraries
RUN dotnet restore

# build
RUN dotnet publish --configuration ${CONFIG:-Debug} --output /app --no-restore

# build runtime image
FROM microsoft/dotnet:runtime
WORKDIR /app
COPY --from=builder /app .
ENTRYPOINT ["dotnet", "ECommerce.Payment.Host.dll"]