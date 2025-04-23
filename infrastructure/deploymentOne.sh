#!/bin/bash

# Crea la carpeta de despliegue
cd ..

mkdir -p microservicios
cd microservicios

# Crea todos los proyectos de microservicios
dotnet new webapi -n GetAdults
dotnet new webapi -n GetChildren
dotnet new webapi -n GetAdultById
dotnet new webapi -n GetChildById
dotnet new webapi -n AddMember
dotnet new webapi -n PickAge
dotnet new webapi -n AddChild
dotnet new webapi -n AddAdult