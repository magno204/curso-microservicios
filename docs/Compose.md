# Documentación para Docker Compose

Crea un archivo **docker-compose.yml** dentro de tu carpeta de **microservicios**

```yml
  services:
    addmember:
      image: addmember
      build:
        context: ./AddMember
        dockerfile: Dockerfile
      ports:
        - 8080:8080
    pickage:
      image: pickage
      build:
        context: ./PickAge
        dockerfile: Dockerfile
    addadult:
      image: addadult
      build:
        context: ./AddAdult
        dockerfile: Dockerfile
    addchild:
      image: addchild
      build:
        context: ./AddChild
        dockerfile: Dockerfile
    getAdults:
      image: getadults
      build:
        context: ./GetAdults
        dockerfile: Dockerfile
      ports:
        - "5000:8080"
```

¡Ejecuta Docker Compose!
