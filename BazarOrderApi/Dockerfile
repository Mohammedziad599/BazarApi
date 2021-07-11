FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["BazarOrderApi.csproj", "BazarOrderApi/"]
RUN dotnet restore "BazarOrderApi/BazarOrderApi.csproj"
COPY . ./BazarOrderApi
WORKDIR "/src/BazarOrderApi"
RUN dotnet build "BazarOrderApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BazarOrderApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BazarOrderApi.dll"]
