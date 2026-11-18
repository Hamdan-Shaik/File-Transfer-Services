# Build context is the repository root.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src
COPY services/fxfer-soap-facade services/fxfer-soap-facade

WORKDIR /src/services/fxfer-soap-facade/src/Host
RUN dotnet publish -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

RUN adduser --disabled-password --gecos "" appuser \
    && chown -R appuser:appuser /app
USER appuser

COPY --from=build /app/publish/ ./

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "Fxfer.SoapFacade.Host.dll"]