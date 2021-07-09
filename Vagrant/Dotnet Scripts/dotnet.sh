#!/usr/bin/env bash

# add microsoft signing key as a trusted key
wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
yes | dpkg -i packages-microsoft-prod.deb
rm packages-microsoft-prod.deb

# Installing the SDK
apt-get update
apt-get install -y apt-transport-https
apt-get update
apt-get install -y dotnet-sdk-5.0