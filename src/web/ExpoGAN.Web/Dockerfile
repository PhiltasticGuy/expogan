#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
#COPY ["src/web/ExpoGAN.Web/ExpoGAN.Web.csproj", "src/web/ExpoGAN.Web/"]
# ATTENTION:
# On copie ici le contenu à partir du context d'exécution et on s'attend
# que le "docker build" soit déclenché par le docker-compose soit avec
# "docker-compose up" ou "docker-compose build". Ceci permet de faire le build
# des projets .NET Core en copiant aussi les projets dépendants dans l'image.
COPY . .
WORKDIR "/src/src/web/ExpoGAN.Web"
RUN dotnet restore
RUN dotnet build "ExpoGAN.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExpoGAN.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpoGAN.Web.dll"]