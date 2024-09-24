# Use the Windows-based image for ASP.NET Core runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-ltsc2022 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

# Use the Windows-based image for .NET SDK
FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-ltsc2022 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy the project file and restore dependencies
COPY ["Pokedex API.csproj", "./"]
RUN dotnet restore "Pokedex API.csproj"

# Copy the rest of the files and build the project
COPY . .   # This should copy all files from the current directory (where the Dockerfile is located)
WORKDIR "/src"
RUN dotnet build "Pokedex API.csproj" -c ${BUILD_CONFIGURATION} -o /app/build

FROM build AS publish
RUN dotnet publish "Pokedex API.csproj" -c ${BUILD_CONFIGURATION} -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Pokedex API.dll"]
