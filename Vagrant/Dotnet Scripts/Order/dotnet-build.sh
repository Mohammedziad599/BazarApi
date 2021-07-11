#!/usr/bin/env bash

cd /home/vagrant/src ||
dotnet restore "BazarOrderApi.csproj"

echo "build project"
dotnet build "BazarOrderApi.csproj" -c Release -o /home/vagrant/app/build
echo "publish project"
dotnet publish "BazarOrderApi.csproj" -c Release -o /home/vagrant/app/publish

cp /home/vagrant/src/Orders.db /home/vagrant/app/publish/Orders.db