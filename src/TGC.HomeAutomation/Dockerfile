FROM mcr.microsoft.com/dotnet/aspnet:8.0-noble-arm64v8 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

ARG BUILD_CONFIGURATION=Release

WORKDIR /src

COPY . .
COPY ["TGC.HomeAutomation.API/TGC.HomeAutomation.API.csproj", "TGC.HomeAutomation.API/"]

RUN dotnet restore "TGC.HomeAutomation.API/TGC.HomeAutomation.API.csproj"

#We are passing custom build parameter buildAngularClient in since it doesn't make sense to build in docker
RUN dotnet build "TGC.HomeAutomation.API/TGC.HomeAutomation.API.csproj" -c $BUILD_CONFIGURATION -o /app/build -p:buildAngularClient=false

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TGC.HomeAutomation.API/TGC.HomeAutomation.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false -p:buildAngularClient=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TGC.HomeAutomation.API.dll"]
