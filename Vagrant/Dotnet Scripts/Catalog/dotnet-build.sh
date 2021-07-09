#!/usr/bin/env bash

cd /home/vagrant/src ||
dotnet restore "BazarCatalogApi.csproj"

echo "build project"
dotnet build "BazarCatalogApi.csproj" -c Release -o /home/vagrant/app/build
echo "publish project"
dotnet publish "BazarCatalogApi.csproj" -c Release -o /home/vagrant/app/publish

cp /home/vagrant/src/Books.db /home/vagrant/app/publish/Books.db