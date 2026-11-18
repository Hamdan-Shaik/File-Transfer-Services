# Build context is the repository root.
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src
COPY workers/fxfer-projection-worker workers/fxfer-projection-worker

WORKDIR /src/workers/fxfer-projection-worker/src/Worker
RUN dotnet publish -c Release -r linux-x64 --self-contained true -o /app/publish /p:PublishAot=true

FROM mcr.microsoft.com/dotnet/runtime-deps:10.0
WORKDIR /app

COPY --from=build /app/publish/ ./

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["./Fxfer.ProjectionWorker.Worker"]

