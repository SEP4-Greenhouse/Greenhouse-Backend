
# ----------- Stage 1: Build the application -----------
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy the solution file and all *.csproj files
COPY GreenhouseBackend.sln ./
COPY Domain/Domain.csproj Domain/
COPY EFCGreenhouse/EFCGreenhouse.csproj EFCGreenhouse/
COPY GreenhouseApi/GreenhouseApi.csproj GreenhouseApi/
COPY MLModelClient/MLModelClient.csproj MLModelClient/

# Restore all dependencies for the solution
RUN dotnet restore GreenhouseBackend.sln

# Now copy everything else
COPY . .

# Build the solution in Release mode
RUN dotnet build GreenhouseBackend.sln -c Release -o /app/build

# ----------- Stage 2: Publish the application -----------
FROM build AS publish
RUN dotnet publish GreenhouseBackend.sln -c Release -o /app/publish

# ----------- Stage 3: Create the final runtime image -----------
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app

# Copy published output from the publish stage
COPY --from=publish /app/publish .

# Expose port 80 (HTTP) or 443 (HTTPS) as needed
EXPOSE 80

# Set the entrypoint to run the GreenhouseApi project DLL
# Adjust the DLL name if it differs (e.g., GreenhouseApi.dll)
ENTRYPOINT ["dotnet", "GreenhouseApi.dll"]
