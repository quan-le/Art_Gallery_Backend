FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy project files and restore
COPY *.csproj ./
COPY *.sln ./
RUN dotnet restore

# Copy everything else
COPY . ./

# Publish app
RUN dotnet publish -c Release -o /app

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy published output
COPY --from=build /app .

# Copy appsettings.json
COPY appsettings.json /app/appsettings.json


# Install Node.js & newman
RUN apt-get update && apt-get install -y curl \
    && curl -fsSL https://deb.nodesource.com/setup_20.x | bash - \
    && apt-get install -y nodejs \
    && npm install -g newman

#EXPOSE 7291
EXPOSE 8080
ENTRYPOINT ["dotnet", "ArtGallery.dll"]


