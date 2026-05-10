FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CaRP/CaRP.Client/CaRP.Client.csproj", "CaRP/CaRP.Client/"]
RUN dotnet restore "CaRP/CaRP.Client/CaRP.Client.csproj"
COPY . .
WORKDIR "/src/CaRP/CaRP.Client"
RUN dotnet build "./CaRP.Client.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CaRP.Client.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CaRP.Client.dll"]
