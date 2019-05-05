FROM mcr.microsoft.com/dotnet/core/aspnet:2.2.4-bionic
MAINTAINER Kok How, Teh <funcoolgeek@gmail.com>
RUN pwd
RUN ls -l /root
WORKDIR /app
ADD ASP.NetCoreTwitterOAuth/bin/Release/netcoreapp2.2/ubuntu.18.10-x64/publish/ .
EXPOSE 80 443
ENTRYPOINT ["dotnet", "ASP.NetCoreTwitterOAuth.dll"]