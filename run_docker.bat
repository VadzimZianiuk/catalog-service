docker rmi catalog-service
dotnet build --configuration Release
dotnet publish -c Release -r linux-x64 --self-contained=false -o bin/Release/net6.0/Docker
docker build -t catalog-service --build-arg "BIN_DIR=bin/Release/net6.0/Docker" .
docker run --rm -m 128M --memory-swap=128M -it -p 5002:80 -p 5003:443 -e ASPNETCORE_URLS="https://+;http://+" --name catalog-service catalog-service