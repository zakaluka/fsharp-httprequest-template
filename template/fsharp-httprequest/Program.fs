namespace fsharp_httprequest

open System
open System.Collections.Generic
open System.IO
open System.Linq
open System.Threading.Tasks
open Microsoft.AspNetCore
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Configuration
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Giraffe
open Microsoft.Extensions.Logging

module Program =
  let errorHandler (ex: Exception) (logger: ILogger) =
    logger.LogError(EventId(), ex, "An unhandled exception has occurred while executing the request.")

    clearResponse
    >=> setStatusCode 500
    >=> text ex.Message

  let webApp =
    choose [ route "/" >=> Successful.OK "get hola que tal" 
             RequestErrors.notFound <| text "Not Found" ]

  let configureApp (app: IApplicationBuilder) =
    app
      .UseGiraffeErrorHandler(errorHandler)
      .UseStaticFiles()
      .UseResponseCaching()
      .UseGiraffe webApp

  let configureServices (svc: IServiceCollection) =
    svc.AddResponseCaching().AddGiraffe() |> ignore
    svc.AddDataProtection() |> ignore

  let configureLogging (loggerBuilder: ILoggingBuilder) =
    loggerBuilder
      .AddFilter(fun lvl -> lvl.Equals LogLevel.Error)
      .AddConsole()
      .AddDebug()
    |> ignore

  let createHostBuilder args =
    Host
      .CreateDefaultBuilder(args)
      .ConfigureWebHostDefaults(fun webBuilder ->
        webBuilder
          .Configure(configureApp)
          .ConfigureServices(configureServices)
          .ConfigureLogging(configureLogging)
          .UseUrls([| "http://localhost:5000" |])
        |> ignore)

  [<EntryPoint>]
  let main args =
    printfn "Starting server..."
    let host = createHostBuilder(args).Build()

    host.Run()
    printfn "Server running..."


    0 // Exit code
