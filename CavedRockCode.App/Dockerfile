FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal AS base
WORKDIR /app
# EXPOSE 8081
# ENV ASPNETCORE_URLS=http://+:8081

# Creates a non-root user with an explicit UID and adds permission to access the /app folder
# For more info, please refer to https://aka.ms/vscode-docker-dotnet-configure-containers
RUN adduser -u 5678 --disabled-password --gecos "" appuser && chown -R appuser /app
USER appuser

FROM mcr.microsoft.com/dotnet/sdk:5.0-focal AS build
WORKDIR /src
COPY ["CavedRockCode.App/CavedRockCode.App.csproj", "CavedRockCode.App/"]
RUN dotnet restore "CavedRockCode.App/CavedRockCode.App.csproj"
COPY . .
WORKDIR "/src/CavedRockCode.App"
RUN dotnet build "CavedRockCode.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CavedRockCode.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CavedRockCode.App.dll"]
