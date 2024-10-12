#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SPA-app-comments/SPA-app-comments.csproj", "SPA-app-comments/"]
COPY ["SPA-app-comments.Infrastructure/SPA-app-comments.Infrastructure.csproj", "SPA-app-comments.Infrastructure/"]
COPY ["SPA-app-comments.Core/SPA-app-comments.Core.csproj", "SPA-app-comments.Core/"]
RUN dotnet restore "./SPA-app-comments/./SPA-app-comments.csproj"
COPY . .
WORKDIR "/src/SPA-app-comments"
RUN dotnet build "./SPA-app-comments.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SPA-app-comments.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SPA-app-comments.dll"]