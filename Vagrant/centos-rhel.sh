#!/usr/bin/env bash

echo "Installing yum-utils"
sudo yum install -y yum-utils

echo "Adding https://rpm.releases.hashicorp.com/RHEL/hashicorp.repo to yum"
echo "This repo contains vagrant"
sudo yum-config-manager --add-repo https://rpm.releases.hashicorp.com/RHEL/hashicorp.repo

echo "Now installing vagrant"
sudo yum -y install vagrant
