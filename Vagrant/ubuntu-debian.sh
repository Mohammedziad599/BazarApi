#!/usr/bin/env bash

echo "Installing CURL using apt"
sudo apt install -y curl

echo "Installing gpg key for the vagrant repo and adding it to apt"
curl -fsSL https://apt.releases.hashicorp.com/gpg | sudo apt-key add -

echo "Adding the apt repo"
echo "Note: Some Variance of ubuntu this command will not work"
echo "because this command will call the (lsb_release -cs) command and some ubuntu distros"
echo "change the output of this command so maybe you need to execute this command manually"
sudo apt-add-repository "deb [arch=amd64] https://apt.releases.hashicorp.com $(lsb_release -cs) main"

echo "Updating apt repo sources and installing vagrant"
sudo apt-get update && sudo apt-get install vagrant
