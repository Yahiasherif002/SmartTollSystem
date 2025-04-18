# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80

# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the .csproj file and restore any dependencies (via 'dotnet restore')
COPY ["SmartTollSystem.Api/SmartTollSystem.Api.csproj", "SmartTollSystem.Api/"]
RUN dotnet restore "SmartTollSystem.Api/SmartTollSystem.Api.csproj"

# Copy the rest of the source code and build the application
COPY . .
WORKDIR "/src/SmartTollSystem.Api"
RUN dotnet build "SmartTollSystem.Api.csproj" -c Release -o /app/build

# Publish the application to a folder
RUN dotnet publish "SmartTollSystem.Api.csproj" -c Release -o /app/publish

# Define the entry point of the application
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "SmartTollSystem.Api.dll"]
