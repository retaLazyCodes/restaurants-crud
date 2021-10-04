#this file has to be in the 'bin/Debug/net5.0/publish' folder to deploy the container 
FROM mcr.microsoft.com/dotnet/aspnet:5.0

WORKDIR /home/app
EXPOSE 80/tcp
COPY OdeToFood/bin/Debug/net5.0/publish .

CMD ASPNETCORE_URLS=http://*:$PORT dotnet OdeToFood.dll
