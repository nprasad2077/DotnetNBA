# Use the official .NET 8.0 SDK image to build and publish the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy everything and restore as distinct layers
COPY . ./
RUN dotnet restore

# Build and publish the app to the /app/out directory
RUN dotnet publish -c Release -o /app/out

# Use the official ASP.NET Core runtime image for the final stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

# Expose ports
EXPOSE 5214
EXPOSE 7012

# Define the entry point for the container
ENTRYPOINT ["dotnet", "DotnetNBA.dll"]
