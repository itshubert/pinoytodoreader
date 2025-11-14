# Use the official .NET 9.0 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build-env
WORKDIR /app

# Copy solution file and project files
COPY PinoyTodo.Reader.sln ./
COPY src/PinoyTodo.Reader.Api/PinoyTodo.Reader.Api.csproj ./src/PinoyTodo.Reader.Api/
COPY src/PinoyTodo.Reader.Application/PinoyTodo.Reader.Application.csproj ./src/PinoyTodo.Reader.Application/
COPY src/PinoyTodo.Reader.Contracts/PinoyTodo.Reader.Contracts.csproj ./src/PinoyTodo.Reader.Contracts/
COPY src/PinoyTodo.Reader.Domain/PinoyTodo.Reader.Domain.csproj ./src/PinoyTodo.Reader.Domain/
COPY src/PinoyTodo.Reader.Infrastructure/PinoyTodo.Reader.Infrastructure.csproj ./src/PinoyTodo.Reader.Infrastructure/
#COPY PinoyCleanArch.0.0.4.nupkg /PinoyPackages/PinoyCleanArch.0.0.4.nupkg
COPY PinoyPackages/ /PinoyPackages/
RUN ls /PinoyPackages

# Restore NuGet packages
# RUN dotnet restore PinoyTodo.Reader.sln
RUN dotnet restore --source "/PinoyPackages" --source "https://api.nuget.org/v3/index.json"


# Copy the entire source code
COPY . ./

# Build and publish the API project
RUN dotnet publish src/PinoyTodo.Reader.Api/PinoyTodo.Reader.Api.csproj -c Release -o out

# Use the official .NET 9.0 ASP.NET Core runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy the published application from the build stage
COPY --from=build-env /app/out .

# Install essential debugging tools as root
RUN apt-get update && \
    apt-get install -y \
    curl \
    iputils-ping \
    telnet \
    dnsutils \
    net-tools \
    wget \
    && rm -rf /var/lib/apt/lists/* \
    && apt-get clean

# Create a non-root user for security
RUN adduser --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser


# Set the entry point for the container
ENTRYPOINT ["dotnet", "PinoyTodo.Reader.Api.dll"]