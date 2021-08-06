# -*- mode: ruby -*-
# vi: set ft=ruby :

# All Vagrant configuration is done below. The "2" in Vagrant.configure
# configures the configuration version (we support older styles for
# backwards compatibility). Please don't change it unless you know what
# you're doing.
Vagrant.configure("2") do |config|
  # The most common configuration options are documented and commented below.
  # For a complete reference, please see the online documentation at
  # https://docs.vagrantup.com.

  # Every Vagrant development environment requires a box. You can search for
  # boxes at https://vagrantcloud.com/search.
  # config.vm.box = "ubuntu/focal64"
  
  
  config.vm.define "catalog" do |catalog|
    catalog.vm.box = "ubuntu/focal64"
    catalog.vm.network "private_network", ip: "192.168.50.100"
    catalog.vm.provision "file", source: "BazarCatalogApi", destination: "$HOME/src"
    catalog.vm.provision "file", source: "BazarCatalogApi.pfx", destination: "$HOME/app/publish/BazarCatalogApi.pfx"
    catalog.vm.provision "shell", path: "Vagrant/Dotnet Scripts/dotnet.sh", privileged: true
    catalog.vm.provision "shell", path: "Vagrant/Dotnet Scripts/Catalog/dotnet-build.sh", privileged: false
    catalog.vm.provision "shell", path: "Vagrant/Dotnet Scripts/Catalog/dotnet-run.sh", privileged: true, run: 'always'
  end
  
  config.vm.define "order" do |order|
    order.vm.box = "ubuntu/focal64"
    order.vm.network "private_network", ip: "192.168.50.101"
    order.vm.provision "file", source: "BazarOrderApi", destination: "$HOME/src"
    order.vm.provision "file", source: "BazarOrderApi.pfx", destination: "$HOME/app/publish/BazarOrderApi.pfx"
    order.vm.provision "shell", path: "Vagrant/Dotnet Scripts/dotnet.sh", privileged: true
    order.vm.provision "shell", path: "Vagrant/Dotnet Scripts/Order/dotnet-build.sh", privileged: false
    order.vm.provision "shell", path: "Vagrant/Dotnet Scripts/Order/dotnet-run.sh", privileged: true, run: 'always'
  end
  
  config.vm.define "cache" do |cache|
      cache.vm.box = "ubuntu/focal64"
      cache.vm.network "private_network", ip: "192.168.50.102"
      cache.vm.provision "file", source: "BazarCacheApi", destination: "$HOME/src"
      cache.vm.provision "file", source: "BazarCacheApi.pfx", destination: "$HOME/app/publish/BazarCacheApi.pfx"
      cache.vm.provision "shell", path: "Vagrant/Dotnet Scripts/dotnet.sh", privileged: true
      cache.vm.provision "shell", path: "Vagrant/Dotnet Scripts/Cache/dotnet-build.sh", privileged: false
      cache.vm.provision "shell", path: "Vagrant/Dotnet Scripts/Cache/dotnet-run.sh", privileged: true, run: 'always'
    end
  
  config.vm.define "ui" do |ui|
    ui.vm.box = "ubuntu/focal64"
    ui.vm.network "private_network", ip: "192.168.50.102"
    ui.vm.provision "shell", path: "Vagrant/Httpd Scripts/install-httpd.sh", privileged: true
    ui.vm.provision "file", source: "UI", destination: "$HOME/src"
    ui.vm.provision "shell", path: "Vagrant/Httpd Scripts/move-files.sh", privileged: true
  end 
  # Disable automatic box update checking. If you disable this, then
  # boxes will only be checked for updates when the user runs
  # `vagrant box outdated`. This is not recommended.
  # config.vm.box_check_update = false

  # Create a forwarded port mapping which allows access to a specific port
  # within the machine from a port on the host machine. In the example below,
  # accessing "localhost:8080" will access port 80 on the guest machine.
  # NOTE: This will enable public access to the opened port
  # config.vm.network "forwarded_port", guest: 80, host: 8080

  # Create a forwarded port mapping which allows access to a specific port
  # within the machine from a port on the host machine and only allow access
  # via 127.0.0.1 to disable public access
  # config.vm.network "forwarded_port", guest: 80, host: 8080, host_ip: "127.0.0.1"

  # Create a private network, which allows host-only access to the machine
  # using a specific IP.
  # config.vm.network "private_network", ip: "192.168.33.10"

  # Create a public network, which generally matched to bridged network.
  # Bridged networks make the machine appear as another physical device on
  # your network.
  # config.vm.network "public_network"

  # Share an additional folder to the guest VM. The first argument is
  # the path on the host to the actual folder. The second argument is
  # the path on the guest to mount the folder. And the optional third
  # argument is a set of non-required options.
  # config.vm.synced_folder "../data", "/vagrant_data"

  # Provider-specific configuration so you can fine-tune various
  # backing providers for Vagrant. These expose provider-specific options.
  # Example for VirtualBox:
  #
  # config.vm.provider "virtualbox" do |vb|
  #   # Display the VirtualBox GUI when booting the machine
  #   vb.gui = true
  #
  #   # Customize the amount of memory on the VM:
  #   vb.memory = "1024"
  # end
  #
  # View the documentation for the provider you are using for more
  # information on available options.

  # Enable provisioning with a shell script. Additional provisioners such as
  # Ansible, Chef, Docker, Puppet and Salt are also available. Please see the
  # documentation for more information about their specific syntax and use.
  # config.vm.provision "shell", inline: <<-SHELL
  #   apt-get update
  #   apt-get install -y apache2
  # SHELL
end
