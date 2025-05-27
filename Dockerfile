FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src

COPY Greenhouse-Backend.sln ./
COPY Domain/Domain.csproj Domain/
COPY EFCGreenhouse/EFCGreenhouse.csproj EFCGreenhouse/
COPY GreenhouseApi/GreenhouseApi.csproj GreenhouseApi/
COPY IotClient/IotClient.csproj IotClient/
COPY MLModelClient/MLModelClient.csproj MLModelClient/
COPY GreenhouseService/GreenhouseService.csproj GreenhouseService/

RUN dotnet sln Greenhouse-Backend.sln remove Tests/Tests.csproj
RUN dotnet restore Greenhouse-Backend.sln

COPY . .

RUN dotnet build Greenhouse-Backend.sln -c Release -o /app/build

FROM build AS publish
RUN dotnet publish Greenhouse-Backend.sln -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime

WORKDIR /app

COPY --from=publish /app/publish .

EXPOSE 5001

ENTRYPOINT ["dotnet", "GreenhouseApi.dll"]
