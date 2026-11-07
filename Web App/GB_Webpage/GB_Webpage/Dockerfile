#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
# Install standard fonts on Debian/Ubuntu
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
RUN apt-get update && apt-get install -y --no-install-recommends fonts-dejavu-core fontconfig && fc-cache -f -v
WORKDIR /app
EXPOSE 7111

COPY ["GB_Webpage/https.pfx", "cert/"]

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["GB_Webpage/GB_Webpage.csproj", "GB_Webpage/"]
RUN dotnet restore "./GB_Webpage/GB_Webpage.csproj"
COPY . .
WORKDIR "/src/GB_Webpage"
RUN dotnet build "./GB_Webpage.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./GB_Webpage.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
RUN rm -rf /app/wwwroot/css
COPY ["GB_Webpage/wwwroot/css", "/app/wwwroot/css"]
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GB_Webpage.dll"]