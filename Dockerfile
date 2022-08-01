FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN dotnet dev-certs https --clean && dotnet dev-certs https -t

FROM mcr.microsoft.com/dotnet/aspnet:6.0
RUN echo apt-get update && apt-get install -y

ARG BIN_DIR
COPY $BIN_DIR /app
COPY --from=build /root/.dotnet/corefx/cryptography/x509stores/my/* /root/.dotnet/corefx/cryptography/x509stores/my/
WORKDIR /app
ENTRYPOINT ["/bin/bash", "-c", "dotnet CatalogService.dll"]