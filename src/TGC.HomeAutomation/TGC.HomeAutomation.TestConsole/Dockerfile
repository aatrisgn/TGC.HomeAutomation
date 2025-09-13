FROM mcr.microsoft.com/dotnet/aspnet:8.0.0-jammy AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TGC.HomeAutomation.TestConsole.csproj", "TGC.HomeAutomation.TestConsole/"]
RUN dotnet restore "TGC.HomeAutomation.TestConsole/TGC.HomeAutomation.TestConsole.csproj"
WORKDIR "/src/TGC.HomeAutomation.TestConsole"
COPY . .
RUN dotnet build "TGC.HomeAutomation.TestConsole.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TGC.HomeAutomation.TestConsole.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TGC.HomeAutomation.TestConsole.dll"]
