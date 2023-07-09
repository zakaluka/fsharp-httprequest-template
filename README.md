# Overview

Based on distantcam's [csharp-httprequest-template](https://github.com/distantcam/csharp-httprequest-template).

# OpenFaaS F# HTTP Template

This repository contains the template for OpenFaaS using the upgraded `of-watchdog` which allows for higher throughput. It has the following features:

- Based on .NET 7.0
- Uses the excellent [Giraffe](https://github.com/giraffe-fsharp/Giraffe) functional wrapper over ASP.NET
- Everything else is extremely minimal and can be customized to fit your needs.

## Using the template

First, pull the template with the faas CLI and create a new function:

```
$ faas template store pull https://github.com/zakaluka/fsharp-httprequest-template
$ faas-cli new --lang fsharp-httprequest <function name>
```

In the directory that was created, using the name of your function, you'll find `parseurl.fs`. It provides a sample function that you can replace (in the `run` method).

You are able to add packages to your function using the `dotnet add package` syntax. The packages will be added to your final function's container automatically.

## Runnin the solution locally

To run this on `localhost:5003`, you can execute the following steps from the solution folder:

```shell
$ dotnet run
```

## Running the solution on docker

To run this on docker, you can execute the following steps from the solution folder with the `Dockerfile`:

```shell
$ docker build . # this will output a build number
$ docker run -p 8082:5001 <build number>
```

Once running, point your browser to http://localhost:8082 or http://localhost:8082/aName to see the results.
