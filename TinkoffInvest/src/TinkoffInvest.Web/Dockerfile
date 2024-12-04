FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["TinkoffInvest/src/TinkoffInvest.Web/TinkoffInvest.Web.csproj", "TinkoffInvest/src/TinkoffInvest.Web/"]
RUN dotnet restore "TinkoffInvest/src/TinkoffInvest.Web/TinkoffInvest.Web.csproj"
COPY . .
WORKDIR "/src/TinkoffInvest/src/TinkoffInvest.Web"
RUN dotnet build "TinkoffInvest.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "TinkoffInvest.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TinkoffInvest.Web.dll"]
