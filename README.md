# Overview

Based on distantcam's [csharp-httprequest-template](https://github.com/distantcam/csharp-httprequest-template).

# OpenFaaS F# HTTP Template

This repository contains the template for OpenFaaS using the upgraded `of-watchdog` which allows for higher throughput.  It has the following features:

- Based on .NET 6.0
- Uses the excellent [Giraffe](https://github.com/giraffe-fsharp/Giraffe) functional wrapper over ASP.NET
- Everything else is extremely minimal and can be customized to fit your needs.

## Using the template
First, pull the template with the faas CLI and create a new function:

```
$ faas template store pull fsharp-httprequest
$ faas-cli new --lang fsharp-httprequest <function name>
```

In the directory that was created, using the name of you function, you'll find `FunctionHandler.fs`. It will look like this:

```fsharp
module Say =
  let hello (name: string) : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
      (text $"Hey %s{name}, how's it going?") next ctx
```

This is a simple implementation of a hello-world function running on HTTP.  Be sure to modify the routing in `Program.fs` to match your solution's needs.

You are able to add packages to your function using the `dotnet add package` syntax. The packages will be added to your final function's container automatically.

## Running the solution locally

To run this on `localhost:8082`, you can execute the following steps from the solution folder with the `Dockerfile`:

```shell
$ docker build . # this will output a build number
$ docker run -p 8082:8080 <build number>
```

Once running, point your browser to http://localhost:8082 or http://localhost:8082/aName to see the results.
