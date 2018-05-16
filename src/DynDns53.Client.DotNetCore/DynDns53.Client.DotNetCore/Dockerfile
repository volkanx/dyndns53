#FROM microsoft/dotnet:2.0-runtime
FROM microsoft/dotnet:2.0.7-runtime-stretch-arm32v7
ARG source
WORKDIR /app
COPY ${source:-obj/Docker/publish} .
ENTRYPOINT ["dotnet", "DynDns53.Client.DotNetCore.dll"]
