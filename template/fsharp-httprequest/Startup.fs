namespace fsharp_httprequest

open System
open Microsoft.AspNetCore.Builder
open Microsoft.AspNetCore.Hosting
open Microsoft.AspNetCore.Http
open Microsoft.Extensions.DependencyInjection
open Microsoft.Extensions.Hosting
open Giraffe
open Microsoft.Extensions.Logging


module Configuration = 
    let errorHandler (ex : Exception) (logger : ILogger) =
        logger.LogError(EventId(), ex, "An unhandled exception has occurred while executing the request.")
        clearResponse >=> setStatusCode 500 >=> text ex.Message

    let webApp = 
        choose [
            GET >=>
                choose [
                    route "/" >=> text "hola que tal"
                ]
            RequestErrors.notFound (text "Not Found")
        ]

    let configureApp (app: IApplicationBuilder) = 
        app.UseGiraffeErrorHandler(errorHandler)
            .UseStaticFiles()
            .UseAuthentication()
            .UseResponseCaching()
            .UseGiraffe webApp

    let configureServices (svc: IServiceCollection) =
        svc
            .AddResponseCaching()
            .AddGiraffe() |> ignore
        svc.AddDataProtection() |> ignore

    let configureLogging (loggerBuilder : ILoggingBuilder) =
        loggerBuilder.AddFilter(fun lvl -> lvl.Equals LogLevel.Error)
            .AddConsole()
            .AddDebug() |> ignore

type Startup() =

    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    member _.ConfigureServices(services: IServiceCollection) =
        ()

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    member _.Configure(app: IApplicationBuilder, env: IWebHostEnvironment) =
        if env.IsDevelopment() then
            app.UseDeveloperExceptionPage() |> ignore

        app.UseRouting()
           .UseEndpoints(fun endpoints ->
                endpoints.MapGet("/", fun context ->
                    context.Response.WriteAsync("Hello World!")) |> ignore
            ) |> ignore
