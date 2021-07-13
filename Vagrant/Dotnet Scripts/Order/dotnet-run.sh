#!/usr/bin/env bash

export ASPNETCORE_URLS="https://+;http://+"
export ASPNETCORE_Kestrel__Certificates__Default__Password="mohammed"
export ASPNETCORE_Kestrel__Certificates__Default__Path="/home/vagrant/app/publish/BazarOrderApi.pfx"

# shellcheck disable=SC2164
cd /home/vagrant/app/publish
dotnet "/home/vagrant/app/publish/BazarOrderApi.dll" &