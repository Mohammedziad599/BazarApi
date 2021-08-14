# Author : Mohammed Ziad <Mohammedziad599@gmail.com>
Vagrant.configure("2") do |config|
  
  config.vm.define "catalog" do |catalog|
    catalog.vm.box = "ubuntu/focal64"
    catalog.vm.network "private_network", ip: "192.168.50.100"
    catalog.vm.provision "file", source: "BazarCatalogApi", destination: "$HOME/src"
    catalog.vm.provision "file", source: "BazarCatalogApi.pfx", destination: "$HOME/app/publish/BazarCatalogApi.pfx"
    catalog.vm.provision "shell", path: "Vagrant/Dotnet Scripts/dotnet.sh", privileged: true
    catalog.vm.provision "shell", path: "Vagrant/Dotnet Scripts/Catalog/dotnet-build.sh", privileged: false
    catalog.vm.provision "shell", path: "Vagrant/Dotnet Scripts/Catalog/dotnet-run.sh", privileged: true, run: 'always'
  end
  
  config.vm.define "catalog_replica" do |catalog_replica|
      catalog_replica.vm.box = "ubuntu/focal64"
      catalog_replica.vm.network "private_network", ip: "192.168.50.200"
      catalog_replica.vm.provision "file", source: "BazarCatalogApi", destination: "$HOME/src"
      catalog_replica.vm.provision "file", source: "BazarCatalogApi.pfx", destination: "$HOME/app/publish/BazarCatalogApi.pfx"
      catalog_replica.vm.provision "shell", path: "Vagrant/Dotnet Scripts/dotnet.sh", privileged: true
      catalog_replica.vm.provision "shell", path: "Vagrant/Dotnet Scripts/Catalog/dotnet-build.sh", privileged: false
      catalog_replica.vm.provision "shell", path: "Vagrant/Dotnet Scripts/Catalog/dotnet-run.sh", privileged: true, run: 'always'
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
  
  config.vm.define "order_replica" do |order_replica|
      order_replica.vm.box = "ubuntu/focal64"
      order_replica.vm.network "private_network", ip: "192.168.50.201"
      order_replica.vm.provision "file", source: "BazarOrderApi", destination: "$HOME/src"
      order_replica.vm.provision "file", source: "BazarOrderApi.pfx", destination: "$HOME/app/publish/BazarOrderApi.pfx"
      order_replica.vm.provision "shell", path: "Vagrant/Dotnet Scripts/dotnet.sh", privileged: true
      order_replica.vm.provision "shell", path: "Vagrant/Dotnet Scripts/Order/dotnet-build.sh", privileged: false
      order_replica.vm.provision "shell", path: "Vagrant/Dotnet Scripts/Order/dotnet-run.sh", privileged: true, run: 'always'
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
    ui.vm.network "private_network", ip: "192.168.50.103"
    ui.vm.provision "shell", path: "Vagrant/Httpd Scripts/install-httpd.sh", privileged: true
    ui.vm.provision "file", source: "UI", destination: "$HOME/src"
    ui.vm.provision "shell", path: "Vagrant/Httpd Scripts/move-files.sh", privileged: true
  end 
end
