FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

COPY Greenhouse-Backend.sln ./
COPY Domain/Domain.csproj Domain/
COPY EFCGreenhouse/EFCGreenhouse.csproj EFCGreenhouse/
COPY GreenhouseApi/GreenhouseApi.csproj GreenhouseApi/
COPY MqttClient/MqttClient.csproj MqttClient/
COPY MLModelClient/MLModelClient.csproj MLModelClient/
COPY GreenhouseService/GreenhouseService.csproj GreenhouseService/

RUN dotnet restore Greenhouse-Backend.sln

COPY . .

RUN dotnet build Greenhouse-Backend.sln -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Greenhouse-Backend.sln -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 5001

ENTRYPOINT ["dotnet", "GreenhouseApi.dll"]
