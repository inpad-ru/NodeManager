#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 85
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NodeManager.Web/NodeManager.Web.csproj", "NodeManager.Web/"]
RUN dotnet restore "NodeManager.Web/NodeManager.Web.csproj"
COPY . .
WORKDIR "/src/NodeManager.Web"
RUN dotnet build "NodeManager.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NodeManager.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NodeManager.Web.dll"]