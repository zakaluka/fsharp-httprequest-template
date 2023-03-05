namespace fsharp_httprequest

open System
open Function
open Microsoft.AspNetCore.Hosting
open Microsoft.Extensions.Hosting
open Microsoft.Extensions.Logging
open Microsoft.AspNetCore.Builder
open Microsoft.Extensions.DependencyInjection
open Giraffe
open parseurl

module Program =
  let errorHandler (ex: Exception) (logger: ILogger) =
    logger.LogError(
      EventId(),
      ex,
      "An unhandled exception has occurred while executing the request."
    )

    clearResponse >=> setStatusCode 500 >=> text ex.Message

  let webApp =
    let warbler f a = f a a

    choose
      [ routexp @".*" Say.hello
        RequestErrors.notFound <| text "Not a real path" ]

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
    let filter (l: LogLevel) =
      l.Equals LogLevel.Critical
      || l.Equals LogLevel.Error
      || l.Equals LogLevel.Warning
      || l.Equals LogLevel.Information

    loggerBuilder.ClearProviders().AddFilter(filter).AddConsole().AddDebug()
    |> ignore

  let createHostBuilder args =
    Host
      .CreateDefaultBuilder(args)
      .ConfigureWebHostDefaults(fun webBuilder ->
        webBuilder
          .Configure(configureApp)
          .ConfigureServices(configureServices)
          .ConfigureLogging(configureLogging)
          .UseUrls([| "http://0.0.0.0:5000" |])
        |> ignore)

  [<EntryPoint>]
  let main args =
    printfn "Starting server..."
    createHostBuilder(args).Build().Run()
    printfn "Server running..."

    0 // Exit code
