# Base image
FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Build image
FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the solution file
COPY ["Pokedex API.sln", "./"]

# Copy csproj files from each project folder
COPY ["Pokedex API/Pokedex API.csproj", "Pokedex API/"]
COPY ["Pokedex API Business/Pokedex API Business.csproj", "Pokedex API Business/"]
COPY ["Pokedex API Business TEST/Pokedex API Business TEST.csproj", "Pokedex API Business TEST/"]
COPY ["Pokedex API Data/Pokedex API Data.csproj", "Pokedex API Data/"]
COPY ["Pokedex API Model/Pokedex API Model.csproj", "Pokedex API Model/"]

# Restore all dependencies
RUN dotnet restore "Pokedex API.sln"

# Copy the entire project folder
COPY . .

# Build the application
WORKDIR "/src/Pokedex API"
RUN dotnet build "Pokedex API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Pokedex API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Final image
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pokedex API.dll"]