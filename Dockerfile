FROM mcr.microsoft.com/dotnet/core/runtime:2.2
ADD ASP.NetCoreTwitterOAuth/bin/Release/netcoreapp2.2/ubuntu.18.10-x64/publish/ app/
ENTRYPOINT ["dotnet", "app/ASP.NetCoreTwitterOAuth.dll"]