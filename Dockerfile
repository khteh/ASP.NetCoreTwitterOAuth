FROM mcr.microsoft.com/dotnet/core/runtime:2.2
ADD bin/Release/netcoreapp2.2/publish/ app/
ENTRYPOINT ["dotnet", "app/ASP.NetCoreTwitterOAuth.dll"]