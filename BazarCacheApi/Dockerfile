FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BazarCacheApi/BazarCacheApi.csproj", "BazarCacheApi/"]
RUN dotnet restore "BazarCacheApi/BazarCacheApi.csproj"
COPY . .
WORKDIR "/src/BazarCacheApi"
RUN dotnet build "BazarCacheApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BazarCacheApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BazarCacheApi.dll"]
