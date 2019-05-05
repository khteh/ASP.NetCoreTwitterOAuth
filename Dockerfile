FROM mcr.microsoft.com/dotnet/core/aspnet:2.2.4-stretch-slim
MAINTAINER Kok How, Teh <funcoolgeek@gmail.com>
RUN pwd
RUN ls -lR
WORKDIR /app
ADD ASP.NetCoreTwitterOAuth/bin/Release/netcoreapp2.2/ubuntu.18.10-x64/publish/ .
EXPOSE 80 443
ENTRYPOINT ["dotnet", "ASP.NetCoreTwitterOAuth.dll"]