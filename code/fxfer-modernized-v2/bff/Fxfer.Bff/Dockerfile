FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY . .

# Native AOT publish output is a self-contained executable.
RUN dotnet publish Fxfer.Bff.csproj -c Release -r linux-x64 --self-contained true -p:PublishAot=true -o /app/publish

FROM mcr.microsoft.com/dotnet/runtime-deps:10.0
WORKDIR /app

COPY --from=build /app/publish .

RUN adduser --disabled-password --gecos "" appuser && chown -R appuser:appuser /app
USER appuser

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["./Fxfer.Bff"]
