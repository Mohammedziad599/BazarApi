#!/usr/bin/env bash

echo "Installing dnf-plugins-core package"
sudo dnf install -y dnf-plugins-core

echo "Adding https://rpm.releases.hashicorp.com/fedora/hashicorp.repo to dnf repos"
echo "This repo contains vagrant"
sudo dnf config-manager --add-repo https://rpm.releases.hashicorp.com/fedora/hashicorp.repo

echo "Now installing vagrant"
sudo dnf -y install vagrant

