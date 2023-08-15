FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EstimateLiabilities/EstimateLiabilities.fsproj", "EstimateLiabilities/"]
COPY ["EstimateLiabilities.API/EstimateLiabilities.API.fsproj", "EstimateLiabilities.API/"]
RUN dotnet restore "EstimateLiabilities.API/EstimateLiabilities.API.fsproj"
COPY . .
WORKDIR "/src/EstimateLiabilities.API"
RUN dotnet build "EstimateLiabilities.API.fsproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EstimateLiabilities.API.fsproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EstimateLiabilities.API.dll"]