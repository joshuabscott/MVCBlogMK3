#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
#EXPOSE 80
#EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["MVCBlogMK3.csproj", ""]
RUN dotnet restore "./MVCBlogMK3.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "MVCBlogMK3.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MVCBlogMK3.csproj" -c Release -o /app/publish

#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "MVCBlogMK3.dll"]

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet MVCBlogMK3.dll


#Step 7: Adjust Dockerfile
#A few modifications need to be made in the Dockerfile before publishing your application. 
#Heroku does not allow you to specify the port(s) with which to listen for incoming requests.
#You can address this by removing both EXPOSE commands that would normally configure ports 80 and 443 to listen for incoming requests. 
#Also, the entry point has to be modified to use the PORT variable assigned by Heroku.