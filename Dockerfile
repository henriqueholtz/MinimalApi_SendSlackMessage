FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /application

# Copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# Copy everythink else and build
COPY . ./
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /application
COPY --from=build /application/out .
CMD ASPNETCORE_URLS="http://*:$PORT" dotnet MinimalApi_SendSlackMessage.dll
