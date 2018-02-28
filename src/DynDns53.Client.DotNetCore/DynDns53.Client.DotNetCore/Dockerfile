#FROM microsoft/dotnet:2.0-runtime
#ARG source
#WORKDIR /app
#COPY ${source:-obj/Docker/publish} .
#ENTRYPOINT ["dotnet", "DynDns53.Client.DotNetCore.dll"]


FROM microsoft/dotnet:2.0-sdk
#FROM microsoft/dotnet:2.0-runtime
WORKDIR /app

# copy csproj and restore as distinct layers
COPY *.csproj ./
RUN dotnet restore

# copy and build everything else
COPY . ./
RUN dotnet publish -c Release -o out
ENTRYPOINT ["dotnet", "/app/bin/Release/netcoreapp2.0/DynDns53.Client.DotNetCore.dll"]