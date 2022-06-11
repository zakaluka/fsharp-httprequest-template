FROM ghcr.io/openfaas/of-watchdog:0.9.6 as watchdog

FROM mcr.microsoft.com/dotnet/sdk:6.0 as builder

# Supress collection of data.
ENV DOTNET_CLI_TELEMETRY_OPTOUT 1

# Optimize for Docker builder caching by adding projects first.

RUN mkdir -p /home/app/Function
WORKDIR /home/app/Function
COPY ./Function/Function.fsproj  .

WORKDIR /home/app/src/
COPY ./root.fsproj  .
RUN dotnet restore ./root.fsproj

COPY .  .

RUN dotnet publish -c release -o published

FROM mcr.microsoft.com/dotnet/aspnet:6.0

COPY --from=watchdog /fwatchdog /usr/bin/fwatchdog
RUN chmod +x /usr/bin/fwatchdog

# Create a non-root user
RUN addgroup --system app \
    && adduser --system --ingroup app app

WORKDIR /home/app/
COPY --from=builder /home/app/src/published .
RUN chown app:app -R /home/app

USER app

ENV fprocess="dotnet ./root.dll"
ENV cgi_headers="true"
ENV mode="http"
ENV upstream_url="http://localhost:5000"
EXPOSE 8080

HEALTHCHECK --interval=3s CMD [ -e /tmp/.lock ] || exit 1

CMD ["fwatchdog"]