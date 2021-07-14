# Bazar Api
Currently the project has two api's the first one is
the catalog api which contains list of all the books
available, and the second is the order api which handle
the ordering process and write the order to the database as a log.

Once you have run the application there will be two api servers 
and at each one of them you can visit `https://serverip/swagger/`
for a documentation on each api and it's endpoints, also you can use that page
to consume the api endpoints.

# Table of Content:

- [Vagrant](#Vagrant "Vagrant Guide")
    * [Vagrant Installation](#vagrant-installation "Vagrant Installation Guide")
- [Docker Compose](#docker-compose "Docker Compose Guide")

# How To Run
there is two methods to Run the Project the first is using **[Vagrant](#Vagrant "Vagrant Guide")** and 
the second is using **[Docker Compose](#docker-compose "Docker Compose Guide")**

## Vagrant
**[vagrant](https://www.vagrantup.com/ "Vagrant Home Page")** is a project that
automate vm creation using a virtual machine host like **Virtual Box** or 
**VMware** to make deploying or creating virtual machines much easier.

we have used vagrant to automate the creation of the vm's and it's configuration
the whole project can be run using one command:

```bash
vagrant up
```

and to stop the vm's 

```bash
vagrant halt
```

to remove the vm's we can run

```bash
vagrant destroy
```

and because we are using an (ubuntu server 20.04 focal)
we can ssh to inside the vm using 

```bash
vagrant ssh <vm-name> # replace <vm-name> with either "order" or "catalog"
```

For a reference on the Vagrantfile on the root folder of this project 
please visit [Vagrant Docs](https://www.vagrantup.com/docs "Vagrant Documentation").

### Vagrant Installation 
The project currently only support vagrant for Ubuntu/Debian, CentOS/RHEL & Fedora,
so there is a script file inside the `Vagrant/` folder that automate the installation
of Vagrant one thing to add that the scripts does not work inside wsl because git will add
`\r\n` instead of the `\n` in linux which makes every command fail
, after successfully installing Vagrant you will need a vm manager that will work with vagrant to create the vm's
and the most supported manager is **Virtual Box**, so you will need to install Virtual Box from the official site.

NOTE: Vagrant in this project has been tested on linux so it's suppose to work with any
linux distro as long as the dependency of vagrant is installed after some testing on a 
windows environment did not manage to get Vagrant to work on windows, so there will be 
a folder with the images of the vm configuration for each vm in the `Vagrant/` folder.

## Docker-compose

Docker compose is a tool that helps with making networking between the
containers much easier, and docker-compose uses the yaml files
as a configuration for more details about the syntax visit the compose file [Docs](https://docs.docker.com/compose/compose-file/compose-file-v3/ "Compose File Documentation").

The project uses the docker containers to build the
project each project has it's own Dockerfile that will
build and publish the api on http and https, but the https
still need the certificate file to work.

The services in the docker-compose will get assigned a ip address by
the docker daemon so either you can get what is the ip of the service
using the docker command line to connect to the dotnet service inside the
container or you can use the ports `5000,5001` on your localhost to connect to
the catalog, and the ports `6000,6001` for the order api.

To make api identify that it is inside a docker container 
an environment variable need to be set to `true` to make the correct changes
to the uri of the catalog server which will be equal to `192.168.50.100` for 
the vm, and equal to a hostname `catalog` in the docker variance.

If you want to run the whole project you can just run the following command
in shell:

```bash
docker-compose up 
```

or if you wish to run the project in the background

```bash
docker-compose up -d
```

If you have used `docker-compose up` and then to totally stop the
project you need to hit `Ctrl+C` and then you will need
to run `docker-compose down` to remove the containers left on your
device, or you can leave them for a faster startup in the next run.