#!/usr/bin/env bash

cd /home/vagrant/src ||
  dotnet restore "BazarCacheApi.csproj"

echo "build project"
dotnet build "BazarCacheApi.csproj" -c Release -o /home/vagrant/app/build
echo "publish project"
dotnet publish "BazarCacheApi.csproj" -c Release -o /home/vagrant/app/publish
