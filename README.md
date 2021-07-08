# Bazar Api
Currently the project has two api's the first one is
the catalog api which contains list of all the books
available, and the second is the order api which handle
the ordering process and write the order to the database as a log.

# How To Run:
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
device, or you can leave them for a faster startup in the next run

# Docker-Compose
Docker compose is a tool that helps with making networking between the
containers much easier, and docker-compose uses the yaml files
as a configuration for more details about the syntax visit the compose file [Docs](https://docs.docker.com/compose/compose-file/compose-file-v3/ "Compose File Documentation").