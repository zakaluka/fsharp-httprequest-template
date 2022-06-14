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
  let hello (names: string seq) : HttpHandler =
    fun (next: HttpFunc) (ctx: HttpContext) ->
      let namesSplit =
        names
        |> Seq.collect (fun e -> e.Split('/'))
        |> Seq.filter (String.IsNullOrWhiteSpace >> not)
      (match Seq.length namesSplit with
      | 0 -> text "Howdy stranger!"
      | 1 -> text $"Hello, my one and only friend %s{Seq.head namesSplit}"
      | _ -> String.Join(", ", namesSplit) |> sprintf "Hello all my friends %s!" |> text
      ) next ctx

```

This is a simple implementation of a "0-n person" hello-world function running on HTTP.  The entire path following the site's fully-qualified domain name is passed to the function and can be parsed as desired (String split, regular expression, etc.).

You are able to add packages to your function using the `dotnet add package` syntax. The packages will be added to your final function's container automatically.

## Runnin the solution locally
To run this on `localhost:5000`, you can execute the following steps from the solution folder:

```shell
$ dotnet run
```

## Running the solution on docker

To run this on `localhost:8082`, you can execute the following steps from the solution folder with the `Dockerfile`:

```shell
$ docker build . # this will output a build number
$ docker run -p 8082:8080 <build number>
```

Once running, point your browser to http://localhost:8082 or http://localhost:8082/aName to see the results.
