# Bazar Api
Currently the project has two api's the first one is
the catalog api which contains list of all the books
available, and the second is the order api which handle
the ordering process and write the order to the database as a log.

Once you have run the application there will be two api servers 
and at each one of them you can visit `https://serverip/swagger/`
for a documentation on each api and it's endpoints, also you can use that page
to consume the api endpoints.

# How To Run
there is two methods to Run the Project the first is using **Vagrant** and 
the second is using **Docker-compose**

## Vagrant
[vagrant](https://www.vagrantup.com/ "Vagrant Home Page") is a project that
automate vm creation using a virtual machine host like **Virtual Box** or 
**VMware** to make deploying virtual machines easy.

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

for a reference on the Vagrantfile on the root folder of this project 
please visit [Vagrant Docs](https://www.vagrantup.com/docs "Vagrant Documentation").

### Vagrant Installation 
the project currently only support vagrant for Ubuntu/Debian, CentOS/RHEL & Fedora,
so there is a script file inside the `Vagrant/` folder that automate the installation
of Vagrant, but Vagrant needs a vm manager to work with and the most supported manager 
is **Virtual Box**, so you will need to install Virtual Box.

NOTE: Vagrant in this project has been tested on linux so it's suppose to work With any
linux distro as long as the dependency of vagrant is installed.

## Docker-compose

NOTE: Currently not working because the docker-compose.yaml need to be
updated to support giving each container an ip address as we have done in
the Vagrantfile.

Docker compose is a tool that helps with making networking between the
containers much easier, and docker-compose uses the yaml files
as a configuration for more details about the syntax visit the compose file [Docs](https://docs.docker.com/compose/compose-file/compose-file-v3/ "Compose File Documentation").

The project uses the docker containers to build the
project each project has it's own Dockerfile that will
build and publish the api on http and https, but the https
still need the certificate file to work

If you want to run the whole project you can just run the following command
in shell:
```bash
docker-compose up 
```
Or if you wish to run the project in the background
```bash
docker-compose up -d
```
If you have used `docker-compose up` and then to totally stop t
project you need to hit `Ctrl+C` and then you will need
to run `docker-compose down` to remove the containers left on your
device, or you can leave them for a faster startup in the next run.