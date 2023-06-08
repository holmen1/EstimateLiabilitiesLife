FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EstimateLiabilitiesLife/EstimateLiabilitiesLife.fsproj", "EstimateLiabilitiesLife/"]
COPY ["EstimateLiabilitiesLife.API/EstimateLiabilitiesLife.API.fsproj", "EstimateLiabilitiesLife.API/"]
RUN dotnet restore "EstimateLiabilitiesLife.API/EstimateLiabilitiesLife.API.fsproj"
COPY . .
WORKDIR "/src/EstimateLiabilitiesLife.API"
RUN dotnet build "EstimateLiabilitiesLife.API.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EstimateLiabilitiesLife.API.fsproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EstimateLiabilitiesLife.API.dll"]