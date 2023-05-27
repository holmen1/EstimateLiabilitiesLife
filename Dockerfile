FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /source

# Copy [cf]sproj and restore as distinct layers
COPY API/*.csproj API/
COPY EstimateLiabilitiesLife/*.fsproj EstimateLiabilitiesLife/
RUN dotnet restore API/API.csproj

# Copy everything else and build
COPY API/ API/
COPY EstimateLiabilitiesLife/ EstimateLiabilitiesLife/

FROM build-env AS publish
WORKDIR /source/API
RUN dotnet publish --no-restore -o /app

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "API.dll"]